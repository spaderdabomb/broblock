using JSAM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static Unity.Collections.AllocatorManager;

[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class SpawnedEnemy : MonoBehaviour
{
    // Serialized fields
    [SerializeField] EnemyType enemyType;
    [ReadOnly] public Enemy EnemyInstance;
    [ReadOnly] public Key? keySO = null;
    [SerializeField] private Transform throwPositionTransform;

    // Components
    private Enemy enemySO;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;
    private Player player;

    // Private member fields
    private float patrolAbsoluteMinPositionX = -8f;
    private float patrolAbsoluteMaxPositionX = 8f;
    private float patrolSizeMinimum = 8f;
    public float patrolMinPositionX = 0f;
    public float patrolMaxPositionX = 0f;
    private float timeSinceLastCombatStateChange = 0f;
    private float timeUntilNextCombatStateChange = 0f;
    private float lowerThrowPositionCutoff = -5f;
    private float upperThrowPositionCutoff = 8f;

    private bool playerInThrowRange = false;

    private Vector2 currentVelocity = Vector2.zero;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        gameObject.layer = LayerMask.NameToLayer("Enemy");

        int enemySOIndex = UnityEngine.Random.Range(0, GameManager.Instance.EnemyDict[enemyType].Count);
        enemySO = GameManager.Instance.EnemyDict[enemyType][enemySOIndex];
        EnemyInstance = ScriptableObject.Instantiate(enemySO);
        EnemyInstance.InitDefaults();
    }

    private void OnEnable()
    {
        EnemyInstance.HealthChanged += CheckIfAlive;
    }

    private void OnDisable()
    {
        EnemyInstance.HealthChanged -= CheckIfAlive;
    }

    private void Start()
    {
        player = GameManager.Instance.player;

        timeUntilNextCombatStateChange = UnityEngine.Random.Range(EnemyInstance.stateChangeTimerMin, EnemyInstance.stateChangeTimerMax);
        EnemyCombatState newState = GetRandomCombatState();
        SetCombatState(newState);

        if (EnemyInstance.enemyCombatStates.Contains<EnemyCombatState>(EnemyCombatState.Patrolling))
        {
            // Determine patrol bounds
            float patrolCenter = UnityEngine.Random.Range(patrolAbsoluteMinPositionX + patrolSizeMinimum / 2,
                                                          patrolAbsoluteMaxPositionX - patrolSizeMinimum / 2);
            patrolMinPositionX = UnityEngine.Random.Range(patrolAbsoluteMinPositionX, patrolCenter);
            patrolMaxPositionX = UnityEngine.Random.Range(patrolAbsoluteMaxPositionX, patrolCenter);

            // Set player veloicty
            int randWalkDirection = UnityEngine.Random.Range(0, 2) * 2 - 1;
            currentVelocity = new Vector2(randWalkDirection * EnemyInstance.walkSpeed, rb2d.velocity.y);
            transform.localScale = new Vector3(randWalkDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void Update()
    {
        UpdateCombatState();
    }

    private void UpdateCombatState()
    {
        // Idling state logic
        if (EnemyInstance.CurrentCombatState == EnemyCombatState.Idling)
        {
            currentVelocity = new Vector2(0f, currentVelocity.y);
        }

        // Patrolling state logic
        if (EnemyInstance.CurrentCombatState == EnemyCombatState.Patrolling)
        {
            if ((rb2d.velocity.x < 0f && rb2d.position.x < patrolMinPositionX) ||
                (rb2d.velocity.x > 0f && rb2d.position.x > patrolMaxPositionX))
            {
                float scaleSign = (float)Mathf.Sign(transform.localScale.x);
                currentVelocity = new Vector2(-scaleSign * EnemyInstance.walkSpeed, currentVelocity.y);
                transform.localScale = new Vector3(-scaleSign * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb2d.velocity.x == 0f)
            {
                float scaleSign = (float)Mathf.Sign(transform.localScale.x);
                currentVelocity = new Vector2(scaleSign * EnemyInstance.walkSpeed, currentVelocity.y);
            }
        }

        playerInThrowRange = IsPlayerInThrowRange();
        if (EnemyInstance.CurrentCombatState == EnemyCombatState.Throwing)
        {
            if (!playerInThrowRange)
            {
                SetCombatState(EnemyCombatState.Idling);
            }
        }

        if (EnemyInstance.enemyCombatStates.Contains(EnemyCombatState.Throwing) && playerInThrowRange)
        {
            SetCombatState(EnemyCombatState.Throwing);
            currentVelocity = new Vector2(0f, currentVelocity.y);
        }

        // Update current combat state values
        timeSinceLastCombatStateChange += Time.deltaTime;
        if (timeSinceLastCombatStateChange >= timeUntilNextCombatStateChange)
        {
            EnemyCombatState newState = GetRandomCombatState();
            SetCombatState(newState);
        }
    }

    private bool IsPlayerInThrowRange()
    {
        // Out of range
        if ((player.transform.position.y - transform.position.y > upperThrowPositionCutoff) ||
            (player.transform.position.y - transform.position.y < lowerThrowPositionCutoff))
        {
            return false;
        }

        return true;
    }


    private EnemyCombatState GetRandomCombatState()
    {
        int randomCombatStateIndex = UnityEngine.Random.Range(0, EnemyInstance.enemyCombatStates.Length);
        EnemyCombatState newState = EnemyInstance.enemyCombatStates[randomCombatStateIndex];

        return newState;
    }

    private void SetCombatState(EnemyCombatState newState)
    {
        EnemyInstance.CurrentCombatState = newState;
        timeSinceLastCombatStateChange = 0f;
        timeUntilNextCombatStateChange = UnityEngine.Random.Range(EnemyInstance.stateChangeTimerMin, EnemyInstance.stateChangeTimerMax);
    }

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(currentVelocity.x, rb2d.velocity.y);
    }

    private void CheckIfAlive(float currentHealth)
    {
        if (currentHealth <= 0)
        {
            DestroyEnemy();
        }
    }

    public void SetEnemyKeyType(Key newKey)
    {
        keySO = newKey;
    }

    public void DamageEnemy(float damageValue)
    {
        EnemyInstance.HealthCurrent -= damageValue;
        float healthRatio = EnemyInstance.health / enemySO.health;
    }

    public void EnemyPunch()
    {

    }

    public void EnemyThrow()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 randomizedPosition = new Vector3(playerPosition.x, playerPosition.y + UnityEngine.Random.Range(-0.1f, 0.1f), playerPosition.z);
        Vector3 direction = (randomizedPosition - throwPositionTransform.transform.position).normalized;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(direction.x), transform.localScale.y, transform.localScale.z);

        float throwSpeed = UnityEngine.Random.Range(EnemyInstance.throwMinSpeed, EnemyInstance.throwMaxSpeed);
        GameObject spawnedThrowable = Instantiate(EnemyInstance.beerPrefab, throwPositionTransform.position, Quaternion.identity);
        spawnedThrowable.GetComponent<Rigidbody2D>().velocity = direction * throwSpeed;
        spawnedThrowable.GetComponent<Rigidbody2D>().angularVelocity = UnityEngine.Random.Range(0f, 500f);
        Destroy(spawnedThrowable, 3f);
    }

    public void DestroyEnemy()
    {
        if (keySO != null)
        {
            GameObject spawnedKey = Instantiate(keySO.keyPrefab, GameManager.Instance.KeyContainer.transform);
            spawnedKey.transform.position = transform.position;
        }

        List<AudioLibrarySounds> bruhEnums = new List<AudioLibrarySounds>();
        foreach (AudioLibrarySounds value in Enum.GetValues(typeof(AudioLibrarySounds)))
        {
            if (value.ToString().StartsWith("Bruh", StringComparison.OrdinalIgnoreCase))
            {
                bruhEnums.Add(value);
            }
        }

        int randIdx = UnityEngine.Random.Range(0, bruhEnums.Count);
        AudioManager.PlaySound(bruhEnums[randIdx]);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the other Rigidbody2D involved in the collision
        Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (otherRb != null)
        {
            // Calculate the force by multiplying the relative velocity with the mass of the other Rigidbody2D
            Vector2 relativeVelocity = collision.relativeVelocity;
            float forceMagnitude = relativeVelocity.magnitude * otherRb.mass;

            if (forceMagnitude >= EnemyInstance.damageForceThreshold)
            {
                DamageEnemy(forceMagnitude);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class SpawnedEnemy : MonoBehaviour
{
    // Serialized fields
    [SerializeField] Enemy enemySO;
    [ReadOnly] public Enemy EnemyInstance;
    [ReadOnly] public Key? keySO = null;

    // Components
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;

    // Private member fields
    private float patrolAbsoluteMinPositionX = -12f;
    private float patrolAbsoluteMaxPositionX = 12f;
    private float patrolSizeMinimum = 6f;
    private float patrolMinPositionX = 0f;
    private float patrolMaxPositionX = 0f;
    private float timeSinceLastCombatStateChange = 0f;
    private float timeUntilNextCombatStateChange = 0f;

    private Vector2 currentVelocity = Vector2.zero;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        gameObject.layer = LayerMask.NameToLayer("Enemy");

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
        timeUntilNextCombatStateChange = Random.Range(EnemyInstance.stateChangeTimerMin, EnemyInstance.stateChangeTimerMax);
        EnemyInstance.CurrentCombatState = SetRandomCombatState();

        if (EnemyInstance.enemyCombatStates.Contains<EnemyCombatState>(EnemyCombatState.Patrolling))
        {
            // Determine patrol bounds
            float patrolCenter = Random.Range(patrolAbsoluteMinPositionX + patrolSizeMinimum,
                                              patrolAbsoluteMaxPositionX - patrolSizeMinimum);
            patrolMinPositionX = Random.Range(patrolAbsoluteMinPositionX, patrolCenter);
            patrolMaxPositionX = Random.Range(patrolAbsoluteMaxPositionX, patrolCenter);

            // Set player veloicty
            int randWalkDirection = Random.Range(0, 2) * 2 - 1;
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

        // Update current combat state values
        timeSinceLastCombatStateChange += Time.deltaTime;
        if (timeSinceLastCombatStateChange >= timeUntilNextCombatStateChange)
        {
            EnemyInstance.CurrentCombatState = SetRandomCombatState();
            timeSinceLastCombatStateChange = 0f;
            timeUntilNextCombatStateChange = Random.Range(EnemyInstance.stateChangeTimerMin, EnemyInstance.stateChangeTimerMax);
        }
    }

    private EnemyCombatState SetRandomCombatState()
    {
        int randomCombatStateIndex = Random.Range(0, EnemyInstance.enemyCombatStates.Length);
        EnemyCombatState newState = EnemyInstance.enemyCombatStates[randomCombatStateIndex];

        return newState;
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

    public void DestroyEnemy()
    {
        if (keySO != null)
        {
            GameObject spawnedKey = Instantiate(keySO.keyPrefab, GameManager.Instance.KeyContainer.transform);
            spawnedKey.transform.position = transform.position;
        }

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

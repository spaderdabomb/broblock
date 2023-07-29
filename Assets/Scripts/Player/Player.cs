using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, PlayerInput.IUtilityMapActions
{
    [SerializeField] GameObject dirtBlockPrefab;
    [SerializeField] GameObject blockContainer;

    [ReadOnly] public PlayerData playerData;
    public BoxCollider2D boxCheckCol;

    private float blockSpawnOffsetX;
    private float blockSpawnOffsetY;

    private PlayerMovement playerMovement;
    private Rigidbody2D rb2d;
    private List<GameObject> dirtBlocks = new();
    private PlayerWeapons playerWeapons;
    private PlayerInput playerinput;
    private Vector3 startScale;

    private void Awake()
    {
        playerData = GameManager.Instance.playerData;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerinput = new PlayerInput();
        playerinput.Enable();
        playerinput.UtilityMap.SetCallbacks(this);

        playerData.HealthChanged += CheckIfAlive;
    }

    private void OnDisable()
    {
        playerinput.UtilityMap.RemoveCallbacks(this);
        playerinput.Disable();

        playerData.HealthChanged -= CheckIfAlive;
    }

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerWeapons = GetComponent<PlayerWeapons>();
        startScale = transform.localScale;

        blockSpawnOffsetX = boxCheckCol.size.x / 2;
        blockSpawnOffsetY = boxCheckCol.size.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        float scaleSignX = Mathf.Sign(playerMovement.lookDirection.x);
        SwitchedLookDirection(scaleSignX);
        UpdateInput();
    }

    private void UpdateInput()
    {
        if (playerinput.UtilityMap.PlaceBlock.IsPressed())
            OnPlaceBlockHeld();
    }

    private void SwitchedLookDirection(float scaleSignX)
    {
        transform.localScale = new Vector3(scaleSignX * startScale.x, transform.localScale.y, transform.localScale.z);
        playerWeapons.weaponContainer.transform.localScale = new Vector3(startScale.x, 1f, 1f);
    }

    public void DamagePlayer(float damageValue)
    {
        playerData.HealthCurrent -= damageValue;
    }

    public void CheckIfAlive(float newHealthValue)
    {
        if (newHealthValue <= 0f)
        {
            GameManager.Instance.LoseGame();
        }
    }

    public void OnCollideWithKey(SpawnedKey spawnedKey)
    {
        bool keyAdded = GameManager.Instance.AddKeyToDict(spawnedKey.keySO.keyType);
        if (keyAdded)
        {
            UIManager.Instance.uiGameSceneMain.AddKeyToUI(spawnedKey);
            Destroy(spawnedKey.gameObject);
        }
    }

    public void OnPlaceBlock(InputAction.CallbackContext context)
    {
        if (!context.canceled)
            return;
        
        // Get direction from player to mouse click
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Get block offset in x and y
        bool lookingMostlyInX = (Mathf.Abs(direction.x) > Mathf.Abs(direction.y));
        float offsetX;
        float offsetY;
        if (lookingMostlyInX)
        {
            offsetX = blockSpawnOffsetX;
            offsetY = blockSpawnOffsetY * direction.y;
        }
        else
        {
            offsetX = blockSpawnOffsetX * direction.x;
            offsetY = blockSpawnOffsetY;
        }

        // Get final position
        Vector3 roundedDirection = new Vector3(Mathf.Sign(direction.x), Mathf.Sign(direction.y), 0f);
        Vector3 newPosition = transform.position + new Vector3(offsetX * roundedDirection.x, offsetY * roundedDirection.y, 0f);


        // Check if block is placeable, then instantiate
        bool canPlaceBlock = GridManager.Instance.IsBlockPlaceable(newPosition);
        if (canPlaceBlock)
        {
            GameObject spawnedDirtBlock = Instantiate(dirtBlockPrefab, blockContainer.transform);
            spawnedDirtBlock.transform.position = newPosition;
            dirtBlocks.Add(spawnedDirtBlock);

            Rigidbody2D blockRb2d = spawnedDirtBlock.GetComponent<Rigidbody2D>();
            blockRb2d.velocity = direction.normalized * playerData.ThrowPowerCurrent;
        }

        playerData.ThrowPowerCurrent = 0f;
    }

    public void OnPlaceBlockHeld()
    {
        playerData.ThrowPowerCurrent += playerData.throwPowerIncreaseRate * Time.deltaTime;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Throwable>() != null)
        {
            // Get the other Rigidbody2D involved in the collision
            Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 relativeVelocity = collision.relativeVelocity;
            float forceMagnitude = relativeVelocity.magnitude * otherRb.mass;

            if (forceMagnitude >= playerData.playerForceThres)
            {
                DamagePlayer(forceMagnitude);
                Destroy(collision.gameObject);
            }
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Lava"))
        {
            DamagePlayer(1f);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

    }
}

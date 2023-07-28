using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapons : MonoBehaviour, PlayerInput.IWeaponsMapActions
{

    [SerializeField] float destroyRange = 2f;

    public GameObject weaponContainer;
    public PlayerInput playerInput { get; private set; }

    public LayerMask ignoreMask;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();
        playerInput.WeaponsMap.SetCallbacks(this);
    }

    private void OnDisable()
    {
        playerInput.WeaponsMap.RemoveCallbacks(this);
        playerInput.Disable();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnUseWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Debug.DrawRay(transform.position, direction);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, destroyRange, ~ignoreMask);

        hit.collider?.GetComponent<SpawnedBlock>()?.DamageBlock(100f);
        hit.collider?.GetComponent<SpawnedEnemy>()?.DamageEnemy(100f);


    }
}

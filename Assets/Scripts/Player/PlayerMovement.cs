using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, PlayerInput.IMovementActions
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float idlingSpeedThresh = 0.01f;

    public LayerMask physicsLayerMask;
    private PlayerData playerData;

    private Rigidbody2D rb2d;
    private CapsuleCollider2D capseulCol;
    private PlayerInput playerInput;

    public Vector3 lookDirection = new();
    public Vector2 moveInputDirection = Vector3.zero;

    private bool jumpPressed = false;
    private bool canJump = false;
    private bool isGrounded = false;
    private bool isMoving = false;

    private void OnEnable()
    {
        playerInput = new();
        playerInput.Enable();
        playerInput.Movement.SetCallbacks(this);
    }

    private void OnDisable()
    {
        playerInput.Movement.RemoveCallbacks(this);
        playerInput.Disable();
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        capseulCol = GetComponent<CapsuleCollider2D>();
        lookDirection = Vector3.right;
        playerData = GetComponent<Player>().playerData;
    }

    void Update()
    {
        if (moveInputDirection.x != 0f)
        {
            lookDirection = Vector3.Normalize(new Vector3(moveInputDirection.x, 0f, 0f));
        }

        isGrounded = IsGrounded();
        canJump = CanJump();
        isMoving = IsMoving();

        CheckMoveStates();
    }

    private void FixedUpdate()
    {
        Jump();
        Move();

        ResetFixedUpdateChecks();
    }

    private void ResetFixedUpdateChecks()
    {
        jumpPressed = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void Move()
    {
        rb2d.velocity = new Vector2(moveInputDirection.x * moveSpeed, rb2d.velocity.y);
    }

    private void Jump()
    {
        if (jumpPressed && canJump && !playerData.CurrentMoveState.HasFlag(PlayerMoveStates.Jumping))
        {
            rb2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            canJump = false;
        }
    }

    private bool CanJump()
    {
        return isGrounded ? true : false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started) return;

        moveInputDirection = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        jumpPressed = true;
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.down, capseulCol.size.y / 2 + 0.01f, physicsLayerMask);

        return hit2D.collider != null ? true : false;
    }

    private bool IsMoving()
    {
        return (Mathf.Abs(rb2d.velocity.x) > idlingSpeedThresh) ? true : false;
    }

    private void CheckMoveStates()
    {
        PlayerMoveStates newMoveState = isMoving ? PlayerMoveStates.Running : PlayerMoveStates.Idling;

        if (!isGrounded)
        {
            newMoveState = PlayerMoveStates.Jumping;
        }

        playerData.CurrentMoveState = newMoveState;
    }

    [Flags]
    public enum PlayerMoveStates
    {
        Idling = 0,
        Walking = 1,
        Running = 2,
        Jumping = 4
    }
}

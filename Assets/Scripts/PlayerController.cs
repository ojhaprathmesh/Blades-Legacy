using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour {
    // Movement settings
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float airRunSpeed = 4f;
    public float jumpImpulse = 10f;

    // State variables
    private Vector2 moveInput;
    private bool _isFacingRight = true;
    private bool _isMoving = false;
    private bool _isRunning = false;

    // Cached components
    private Rigidbody2D rb;
    private Animator animator;
    private TouchingDirections touchingDirections;

    // Properties for state management
    private float CurrentMoveSpeed => CanMove 
    ? (IsMoving && !touchingDirections.IsOnWall 
        ? (touchingDirections.IsGrounded 
            ? (IsRunning ? runSpeed : walkSpeed)  // Running while grounded
            : (IsRunning ? airRunSpeed : airWalkSpeed)) // Running in the air
        : 0)
    : 0;

    private bool IsMoving {
        get => _isMoving;
        set {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    private bool IsRunning {
        get => _isRunning;
        set {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    private bool IsFacingRight {
        get => _isFacingRight;
        set {
            if (_isFacingRight != value) {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
            _isFacingRight = value;
        }
    }

    private bool CanMove => animator.GetBool(AnimationStrings.canMove);

    private void Awake() {
        // Cache necessary components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate() {
        // Update player's horizontal velocity
        rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);
        animator.SetFloat(AnimationStrings.velocityY, rb.linearVelocityY);
    }

    public void OnMove(InputAction.CallbackContext context) {
        // Handle movement input
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }

    public void OnRun(InputAction.CallbackContext context) {
        // Handle run toggle
        IsRunning = context.performed;
    }

    public void OnJump(InputAction.CallbackContext context) {
        // Handle jump input
        if (context.started && touchingDirections.IsGrounded && CanMove) {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context) {
        // Trigger attack animation
        if (context.started) {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    private void SetFacingDirection(Vector2 moveInput) {
        // Check direction of the player
        if (moveInput.x != 0) {
            IsFacingRight = moveInput.x > 0;
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    Vector2 moveInput;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    private bool _isMoving = false;
    private bool _isRunning = false;

    public bool _isFacingRight = true;

    public float CurrentMoveSpeed {
        get {
            if (IsMoving) {
                if (IsRunning) {
                    return runSpeed;
                } else {
                    return walkSpeed;
                }
            } else {
                return 0;
            }
        }
    }

    public bool IsMoving {
        get {
            return _isMoving;
        } 
        private set {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool IsRunning {
        get {
            return _isRunning;
        } 
        private set {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool IsFacingRight { 
        get {
            return _isFacingRight;
        } 
        private set{
            if(_isFacingRight != value) {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    Rigidbody2D rb;
    Animator animator;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    public void OnRun(InputAction.CallbackContext context) {
        if (context.started) {
            IsRunning = true;
        } else if (context.canceled) {
            IsRunning = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput) {
        if (moveInput.x > 0 && !IsFacingRight) {
            IsFacingRight = true;
        } else if (moveInput.x < 0 && IsFacingRight) {
            IsFacingRight = false;
        }
    }
}
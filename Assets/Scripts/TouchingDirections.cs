using UnityEngine;

public class TouchingDirections : MonoBehaviour {
    // Settings for raycast detection
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;

    // Cached components
    private CapsuleCollider2D touchingCollider;
    private Animator animator;

    // State variables
    private bool _isGrounded;
    private bool _isOnWall;
    private bool _isOnCeiling;

    // Properties for state access
    public bool IsGrounded {
        get => _isGrounded;
        private set {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    public bool IsOnWall {
        get => _isOnWall;
        private set {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    public bool IsOnCeiling {
        get => _isOnCeiling;
        private set {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private Vector2 WallCheckDirection => transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    private void Awake() {
        // Cache necessary components
        touchingCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        // Check for ground, wall, and ceiling collisions
        IsGrounded = DetectCollision(Vector2.down, groundDistance);
        IsOnWall = DetectCollision(WallCheckDirection, wallDistance);
        IsOnCeiling = DetectCollision(Vector2.up, ceilingDistance);
    }

    private bool DetectCollision(Vector2 direction, float distance) {
        // Perform raycast in the specified direction
        return touchingCollider.Cast(direction, castFilter, new RaycastHit2D[5], distance) > 0;
    }
}

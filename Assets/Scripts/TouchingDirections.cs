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
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isOnWall;
    [SerializeField] private bool _isOnCeiling;

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
        // Check for ceiling first
        IsOnCeiling = DetectCollision(Vector2.up, ceilingDistance);

        // If not on the ceiling, check for ground
        IsGrounded = !IsOnCeiling && DetectCollision(Vector2.down, groundDistance);

        // Check for wall collision
        IsOnWall = DetectCollision(WallCheckDirection, wallDistance);
    }

    private bool DetectCollision(Vector2 direction, float distance) {
        // Perform raycast in the specified direction
        return touchingCollider.Cast(direction, castFilter, new RaycastHit2D[5], distance) > 0;
    }
}

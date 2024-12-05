using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    CapsuleCollider2D touchingCollider;
    Animator animator;
    RaycastHit2D[] groundHits = new RaycastHit2D[10];
    [SerializeField] private bool _isGrounded;

    public bool IsGrounded { 
        get {
            return _isGrounded;
        } 
        private set {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    private void Awake () {
        touchingCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        IsGrounded = touchingCollider.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
    }
}

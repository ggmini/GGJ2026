using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    CircleCollider2D upperCollider;
    BoxCollider2D lowerCollider;
    [SerializeField]
    PlayerAnimator animator;
    [SerializeField]
    LayerMask environmentLayer;
    [SerializeField]
    float moveSpeed = 500f;
    bool airborne = false;
    public bool Airborne { get => airborne; }
    bool canDoubleJump = false;
    public bool CanDoubleJump { get => canDoubleJump; }
    float maxJumpVelocity = 10f;
    float maxJumpTime = 0.75f;
    public float MaxJumpTime { get => maxJumpTime; }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        upperCollider = GetComponent<CircleCollider2D>();
        lowerCollider = GetComponent<BoxCollider2D>();
    }

    public void CheckFloor() {
        //TODO: Fix: Raycast is only in center, so might miss ground when on edges
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, environmentLayer);
        if (hit.collider != null) {
            airborne = false;
            canDoubleJump = true;
        } else
            airborne = true;
    }

    public void Move(float xDir) {
        rb.linearVelocity = new Vector2(xDir * moveSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
        if (xDir != 0) {
            int lookDir = xDir > 0 ? 1 : -1;
            transform.localScale = new Vector3(lookDir, 1, 1);
        }
        if (airborne)
            animator.Jump();
        else if (xDir != 0)
            animator.Run();            
        else
            animator.Idle();     
    }

    public void Jump(float timeJumpPressed) {
        float jumpVelocity = maxJumpVelocity;
        jumpVelocity *= maxJumpTime - (timeJumpPressed / maxJumpTime);
        rb.linearVelocityY = jumpVelocity;
    }

    public void Crouch() {
        upperCollider.enabled = false;
        lowerCollider.size = new Vector2(1, 0.25f);
        lowerCollider.offset = new Vector2(0, -0.375f);
        //animator.Crouch();
    }

    public void Uncrouch() {
        //TOODO: Check for ceiling
        upperCollider.enabled = true;
        lowerCollider.size = new Vector2(1, 0.5f);
        lowerCollider.offset = new Vector2(0, -0.25f);
        animator.Idle();
    }

    public bool TryJump() {
        if (!airborne)
            return true;
        else if (canDoubleJump) {
            canDoubleJump = false;
            return true;
        }
        return false;
    }

}

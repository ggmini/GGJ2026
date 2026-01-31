using System.Collections;
using UnityEngine;

sealed class Player : MonoBehaviour
{
    Rigidbody2D rb;
    CircleCollider2D upperCollider;
    BoxCollider2D lowerCollider;
    [SerializeField]
    PlayerAnimator animator;
    [SerializeField]
    GameManager GM;
    [SerializeField]
    LayerMask environmentLayer;
    [SerializeField]
    float moveSpeed = 500f;
    float sprintModifier = 1.5f;
    bool isSprinting = false;
    bool airborne = false;
    public bool Airborne { get => airborne; }
    bool canDoubleJump = false;
    public bool CanDoubleJump { get => canDoubleJump; }
    float maxJumpVelocity = 10f;
    float maxJumpTime = 0.75f;
    public float MaxJumpTime { get => maxJumpTime; }
    float playerWidth = 0.5f;

    int health = 10;
    bool dead = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        upperCollider = GetComponent<CircleCollider2D>();
        lowerCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate() {
        if (!dead) {
            if (rb.position.y < -10f)
                Die();
        }
    }

    public void CheckFloor() {
        //TODO: Fix: Raycast is only in center, so might miss ground when on edges
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + new Vector3(-playerWidth, 0, 0), Vector2.down, 0.1f, environmentLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + new Vector3(playerWidth, 0, 0), Vector2.down, 0.1f, environmentLayer);
        Debug.DrawRay(transform.position + new Vector3(-playerWidth, 0, 0), Vector2.down * 0.1f, Color.red);
        Debug.DrawRay(transform.position + new Vector3(playerWidth, 0, 0), Vector2.down * 0.1f, Color.red);
        if (hitLeft.collider != null || hitRight.collider != null) {
            airborne = false;
            canDoubleJump = true;
        } else
            airborne = true;
    }

    public void Move(float xDir) {
        float velocity = isSprinting ? moveSpeed * sprintModifier : moveSpeed;
        rb.linearVelocity = new Vector2(xDir * velocity * Time.fixedDeltaTime, rb.linearVelocity.y);
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

    public void SetSprinting(bool sprinting) {
        isSprinting = sprinting;
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
        animator.isUpright = false;
    }

    public void Uncrouch() {
        //TODO: Check for ceiling
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

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0)
            Die();
    }

    void Die() {
        dead = true;
        //animator.Die();
        StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene() {
        yield return new WaitForSeconds(1.6f);
        GM.ReloadScene();
    }

}

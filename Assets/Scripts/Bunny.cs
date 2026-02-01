using UnityEngine;

sealed class Bunny : MonoBehaviour {

    Rigidbody2D rb;
    [SerializeField]
    LayerMask environmentLayer;
    float timeOnGround = 0f;
    [SerializeField]
    Vector2 jumpForce = new(200, 300);
    [SerializeField]
    int dir = 1;
    [SerializeField]
    float modelWidth = 0.5f;
    bool justTurned;
    [SerializeField]
    EnemyAnimator animator;

    void Start() {
        animator.Idle();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        CheckInFront();
        bool grounded = isGrounded();
        if (grounded) {
            timeOnGround += Time.deltaTime;
            if (timeOnGround > 0.5f) {
                Jump();
            } else {
                animator.Idle();
            }
        } else {
            if (rb.linearVelocityY < 0) {
                animator.Fall();
            }

            timeOnGround = 0f;
        }
    }

    bool isGrounded() {
        //TODO: Can get stuck on edges
        var origin = transform.position;
        var offset = new Vector3(modelWidth, -0.5f, 0);
        var hitLeft = Physics2D.Raycast(origin + offset, Vector2.down, 0.1f, environmentLayer);
        //Debug.DrawRay(origin + offset, Vector2.down * 0.1f, Color.red);
        offset.x *= -1;
        var hitRight = Physics2D.Raycast(origin + offset, Vector2.down, 0.1f, environmentLayer);
        //Debug.DrawRay(origin + offset, Vector2.down * 0.1f, Color.red);
        if (hitLeft.collider != hitRight.collider) {
            TurnAround();
            return true;
        } else if (hitLeft.collider != null) {
            return true;
        }

        return false;
    }

    void Jump() {
        rb.AddForce(new Vector2(jumpForce.x * dir, jumpForce.y));
        timeOnGround = 0f;
        justTurned = false;
        animator.Jump();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            var player = other.gameObject.GetComponent<Player>();
            //TODO: Check if player is wearing bunny mask
            player.TakeDamage(2, transform.position.x);
        }
    }

    void CheckInFront() {
        //TODO: Adjust raycast positions based on bunny size
        var origin = transform.position + new Vector3(dir * 0.75f, 0, 0);
        var hitFront = Physics2D.Raycast(origin, Vector2.right * dir, 0.1f, LayerMask.GetMask("Environment"));
        //Debug.DrawRay(origin, Vector2.right * dir * 0.1f, Color.red);
        if (hitFront.collider != null) {
            TurnAround();
        }
    }

    void TurnAround() {
        if (justTurned) {
            return;
        }

        dir *= -1;
        rb.linearVelocityX *= -1f;
        justTurned = true;
        //TODO: Flip sprite in animator
    }
}

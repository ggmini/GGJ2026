using UnityEngine;

sealed class Rat : MonoBehaviour
{
    [SerializeField]
    float dir = 1f;
    [SerializeField]
    float moveSpeed = 10f;
    [SerializeField]
    EnemyAnimator animator;

    Rigidbody2D rb;
    bool isJumping = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if (isJumping) {
            if(isGrounded())
                isJumping = false;
            return;
        }

        CheckInFront();
        Move();
    }

    void CheckInFront() {
        var origin = transform.position + new Vector3(dir * 0.85f, -0.25f, 0);
        var hitFrontDistant = Physics2D.Raycast(origin, Vector2.right * dir, 3f, LayerMask.GetMask("Environment"));
        var hitFront = Physics2D.Raycast(origin, Vector2.right * dir, 0.1f, LayerMask.GetMask("Environment"));
        //Debug.DrawRay(origin, Vector2.right * dir * 0.1f, Color.red);
        origin.y -= 0.25f;
        bool grounded = isGrounded();
        //Debug.DrawRay(origin, Vector2.down * 0.1f, Color.red);
        if (!grounded && checkDropDown()) {
            return;
        }
        if (hitFrontDistant.collider != null) {
            if (checkJumpUp()) {
                rb.linearVelocity = new(rb.linearVelocity.x/2, 10f);
                isJumping = true;
                return;
            }
        }
        if (hitFront.collider != null || !grounded) {
            Debug.Log("Turn Around");
            dir *= -1f;
            animator.TurnAround();
        }
    }

    bool isGrounded() {
        var origin = transform.position + new Vector3(0, -1.5f, 0);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, LayerMask.GetMask("Environment"));
        //Debug.DrawRay(origin, Vector2.down * 0.1f, Color.purple);
        if (hit.collider != null)
            return true;
        return false;
    }

    bool checkJumpUp() {
        var origin = transform.position + new Vector3(dir * 3f, 3f, 0);
        var hit = Physics2D.Raycast(origin, Vector2.down, 3.1f, LayerMask.GetMask("Environment"));
        //Debug.DrawRay(origin, Vector2.down * 1.1f, Color.green);
        if (hit.collider != null && hit.distance > 0)
            return true;
        return false;
    }

    bool checkDropDown() {
        var origin = transform.position + new Vector3(dir * 0.85f, -1.5f, 0);
        var hit = Physics2D.Raycast(origin, Vector2.down, 3.1f, LayerMask.GetMask("Environment"));
        //Debug.DrawRay(origin, Vector2.down * 3.1f, Color.blue);
        if (hit.collider != null)
            return true;
        return false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            var player = other.gameObject.GetComponent<Player>();
            //TODO: Check if player is wearing rat mask
            player.TakeDamage(1);
        }
    }

    void Move() {
        rb.linearVelocityX = moveSpeed * dir;
    }

}

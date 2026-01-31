using System;
using System.Collections;
using UnityEngine;

sealed class Player : MonoBehaviour {
    Rigidbody2D rb;
    [SerializeField]
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

    [field: SerializeField]
    public bool Airborne { get; private set; } = false;
    public bool CanDoubleJump { get; private set; } = false;
    float maxJumpVelocity = 10f;

    public float MaxJumpTime { get; } = 0.75f;
    float playerWidth = 0.5f;

    int health = 10;
    bool dead = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        upperCollider = GetComponent<CircleCollider2D>();
        lowerCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate() {
        PerformMove();

        CheckFloor();

        rb.gravityScale = (isJumping, Airborne) switch {
            (true, _) => 0.75f,
            (_, true) => 1,
            _ => 1,
        };

        if (!dead) {
            if (rb.position.y < -10f) {
                Die();
            }
        }
    }

    public void CheckFloor() {
        //TODO: Adjust playerWidth based on player size
        var hitLeft = Physics2D.Raycast(transform.position + new Vector3(-playerWidth, 0, 0), Vector2.down, 0.1f, environmentLayer);
        var hitRight = Physics2D.Raycast(transform.position + new Vector3(playerWidth, 0, 0), Vector2.down, 0.1f, environmentLayer);
        //Debug.DrawRay(transform.position + new Vector3(-playerWidth, 0, 0), Vector2.down * 0.1f, Color.red);
        //Debug.DrawRay(transform.position + new Vector3(playerWidth, 0, 0), Vector2.down * 0.1f, Color.red);
        if (hitLeft.collider != null || hitRight.collider != null) {
            Airborne = false;
            CanDoubleJump = true;
        } else {
            Airborne = true;
        }
    }

    [SerializeField]
    float runAccelerationTime = 0.1f;
    [SerializeField]
    float jumpAccelerationTime = 0.2f;
    [SerializeField]
    float fallAccelerationTime = 0.3f;

    float moveIntention;
    Vector2 acceleration;

    public void Move(float xDir) {
        moveIntention = xDir * (isSprinting ? moveSpeed * sprintModifier : moveSpeed);
    }

    void PerformMove() {
        var targetVelocity = new Vector2(moveIntention * Time.fixedDeltaTime, rb.linearVelocity.y);
        float smoothTime = (isJumping, Airborne) switch {
            (true, _) => jumpAccelerationTime,
            (_, true) => fallAccelerationTime,
            _ => runAccelerationTime,
        };
        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref acceleration, smoothTime);

        float moveSign = Math.Sign(moveIntention);
        switch (moveSign) {
            case > 0:
                animator.isFacingLeft = false;
                break;
            case < 0:
                animator.isFacingLeft = true;
                break;
        }

        switch ((isJumping, Airborne, moveSign)) {
            case (true, _, _):
                animator.Jump();
                break;
            case (_, true, _):
                animator.Fall();
                break;
            case (_, _, 0):
                animator.Idle();
                break;
            default:
                animator.Run();
                break;
        }
    }

    public void SetSprinting(bool sprinting) {
        isSprinting = sprinting;
    }

    public void Jump() {
        rb.linearVelocityY = maxJumpVelocity;
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

    public bool isJumping => rb.linearVelocity.y > 0;

    public void StartJump() {
        if (!Airborne) {
            Jump();
            return;
        }

        if (CanDoubleJump) {
            CanDoubleJump = false;
        }
    }

    public void CancelJump() {
        if (!isJumping) {
            return;
        }

        rb.linearVelocityY *= 0.25f;
    }

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            Die();
        }
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

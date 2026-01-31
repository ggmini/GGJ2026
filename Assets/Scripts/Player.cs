using System;
using System.Collections;
using UnityEngine;

sealed class Player : MonoBehaviour {
    public static Player instance;
    Rigidbody2D rb;
    [SerializeField]
    CircleCollider2D upperCollider;
    BoxCollider2D lowerCollider;
    [SerializeField]
    PlayerAnimator animator;
    [SerializeField]
    LayerMask environmentLayer;
    [SerializeField]
    float moveSpeed = 500f;
    [SerializeField]
    float crouchModifier = 0.5f;
    [SerializeField]
    float sprintModifier = 1.5f;
    [SerializeField]
    bool isSprinting = false;

    [field: SerializeField]
    public bool Airborne { get; private set; } = false;
    public bool CanDoubleJump { get; private set; } = false;
    float maxJumpVelocity = 10f;

    public float MaxJumpTime { get; } = 0.75f;
    float playerWidth = 0.5f;

    int health = 10;
    bool dead = false;

    [Header("Mask Percentages")]
    public float DefaultMaskPercentage { get; private set; } = 1f;
    public float RatMaskPercentage { get; private set; } = 0f;
    public float BunnyMaskPercentage { get; private set; } = 0f;

    void Awake() {
        instance = this;
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        lowerCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate() {
        PerformMove();

        CheckFloor();

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
        Debug.DrawRay(transform.position + new Vector3(-playerWidth, 0, 0), Vector2.down * 0.1f, Color.red);
        Debug.DrawRay(transform.position + new Vector3(playerWidth, 0, 0), Vector2.down * 0.1f, Color.red);
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
    float acceleration;

    public void Move(float xDir) {
        moveIntention = xDir * moveSpeed;
    }

    void PerformMove() {
        float targetVelocity = moveIntention;
        if (isSprinting) {
            targetVelocity *= sprintModifier;
        }

        if (isCrouching) {
            targetVelocity *= crouchModifier;
        }

        float smoothTime = (isJumping, Airborne) switch {
            (true, _) => jumpAccelerationTime,
            (_, true) => fallAccelerationTime,
            _ => runAccelerationTime,
        };
        rb.linearVelocityX = Mathf.SmoothDamp(rb.linearVelocityX, targetVelocity, ref acceleration, smoothTime);

        float gravityScale = (isJumping, Airborne) switch {
            (true, _) => 0.75f,
            (_, true) => 1,
            _ => 1,
        };
        rb.linearVelocity += gravityScale * Time.deltaTime * Physics2D.gravity;

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

    public void Jump() {
        rb.linearVelocityY = maxJumpVelocity;
    }

    bool isCrouching = false;

    public void Crouch() {
        isCrouching = true;
        upperCollider.enabled = false;
        lowerCollider.size = new Vector2(1, 0.25f);
        lowerCollider.offset = new Vector2(0, 0.125f);
        animator.isUpright = false;
    }

    public void Uncrouch() {
        isCrouching = false;
        //TODO: Check for ceiling
        upperCollider.enabled = true;
        lowerCollider.size = new Vector2(1, 0.5f);
        lowerCollider.offset = new Vector2(0, 0.25f);
        animator.isUpright = true;
    }

    public bool isJumping => rb.linearVelocityY > 0;

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

    public void StartSprint() {
        isSprinting = true;
    }

    public void StopSprint() {
        isSprinting = false;
    }

    public void TakeDamage(int damage) {
        animator.Flash();

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
        GameManager.ReloadScene();
    }
}

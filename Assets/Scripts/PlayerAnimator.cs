using UnityEngine;

sealed class PlayerAnimator : MonoBehaviour {
    [SerializeField]
    Animator animator;
    [SerializeField]
    SpriteRenderer sprite;

    [SerializeField]
    public bool isUpright = true;

    public bool isFacingLeft {
        get => sprite.flipX;
        set => sprite.flipX = value;
    }

    [ContextMenu(nameof(TurnAround))]
    public void TurnAround() {
        isFacingLeft = !isFacingLeft;
    }

    void Play(string animation) {
        animator.Play(isUpright ? $"{STATE_UPRIGHT}_{animation}" : $"{STATE_DUCKED}_{animation}", 0);
    }

    const string STATE_UPRIGHT = "upright";
    const string STATE_DUCKED = "ducked";

    const string ANIM_IDLING = "idling";
    const string ANIM_JUMPING = "jumping";
    const string ANIM_RUNNING = "running";
    const string ANIM_FALLING = "falling";

    [ContextMenu(nameof(Idle))]
    public void Idle() {
        Play(ANIM_IDLING);
    }

    [ContextMenu(nameof(Jump))]
    public void Jump() {
        Play(ANIM_JUMPING);
    }

    [ContextMenu(nameof(Run))]
    public void Run() {
        Play(ANIM_RUNNING);
    }

    [ContextMenu(nameof(Fall))]
    public void Fall() {
        Play(ANIM_FALLING);
    }
}

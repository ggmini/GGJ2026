using Slothsoft.Aseprite;
using Slothsoft.UnityExtensions;
using UnityEngine;

sealed class PlayerAnimator : MonoBehaviour {
    [SerializeField]
    Animator animator;

    [SerializeField]
    SpriteRenderer sprite;

    [SerializeField]
    SpriteRenderer mask;

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
        string anim = isUpright ? $"{STATE_UPRIGHT}_{animation}" : $"{STATE_DUCKED}_{animation}";
        animator.Play(anim, 0);
    }

    const string STATE_UPRIGHT = "upright";
    const string STATE_DUCKED = "ducked";

    const string ANIM_IDLING = "idling";
    const string ANIM_JUMPING = "jumping";
    const string ANIM_RUNNING = "running";
    const string ANIM_FALLING = "falling";

    void Start() {
        ShowDefaultMask();
    }

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

    [ContextMenu(nameof(ShowDefaultMask))]
    public void ShowDefaultMask() {
        SetMaskRatios(1, 0, 0);
    }

    [ContextMenu(nameof(ShowMouseMask))]
    public void ShowMouseMask() {
        SetMaskRatios(0, 1, 0);
    }

    [ContextMenu(nameof(ShowBunnyMask))]
    public void ShowBunnyMask() {
        SetMaskRatios(0, 0, 1);
    }

    public void SetMaskRatios(float defaultVisibility, float mouseVisibility, float bunnyVisibility) {
        mask.material.SetFloat("_Default", defaultVisibility);
        mask.material.SetFloat("_Mouse", mouseVisibility);
        mask.material.SetFloat("_Bunny", bunnyVisibility);
    }

    [SerializeField, Expandable]
    ColorAsset eyeColor;

    void LateUpdate() {

    }
}

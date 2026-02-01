using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Slothsoft.Aseprite;
using Slothsoft.Effects;
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
        set {
            sprite.flipX = value;
            mask.flipX = value;
        }
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
        Idle();
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
    [SerializeField]
    Vector2 eyeOffset;

    readonly Dictionary<Sprite, Vector2> eyePositions = new();

    void LateUpdate() {
        if (!eyePositions.TryGetValue(sprite.sprite, out var position)) {
            position = Vector2.zero;
            int count = 0;
            var pixels = sprite
                .sprite
                .GetPixelsWithPosition()
                .Where(pixel => pixel.color.IsEqualTo(eyeColor.color32));
            foreach (var pixel in pixels) {
                position += pixel.position;
                count++;
            }

            if (count == 0) {
                return;
            }

            position /= count;

            eyePositions[sprite.sprite] = position;
        }

        position += eyeOffset;

        if (isFacingLeft) {
            position.x *= -1;
        }

        mask.transform.localPosition = position;
    }

    internal void Flash() {
        if (flashRoutine is not null) {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(Flash_Co());
    }

    Coroutine flashRoutine;
    [SerializeField]
    float flashDuration = 1;
    [SerializeField]
    AnimationCurve flashAnimation = AnimationCurve.Linear(0, 1, 1, 0);

    IEnumerator Flash_Co() {
        for (float flashTimer = 0; flashTimer < flashDuration; flashTimer += Time.deltaTime) {
            sprite.material.SetFloat("_FlashIntensity", flashAnimation.Evaluate(flashTimer));
            yield return null;
        }

        flashRoutine = null;
    }

    [SerializeField]
    EffectEvent onDeath = new();

    internal void Die() {
        sprite.enabled = false;
        mask.enabled = false;
        onDeath.Invoke(gameObject);
    }
}

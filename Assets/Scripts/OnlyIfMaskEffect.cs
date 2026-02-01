using System;
using Slothsoft.Effects;
using Slothsoft.UnityExtensions;
using UnityEngine;

[ImplementationFor(typeof(IEffect), nameof(OnlyIfMaskEffect))]
[Serializable]
sealed class OnlyIfMaskEffect : IEffect {
    [SerializeField]
    MaskType requiredMask = MaskType.Default;
    [SerializeField]
    EffectEvent effect = new();

    public void Invoke() {
        throw new Exception("can't");
    }

    public void Invoke(GameObject context) {
        if (context.TryGetComponent<Player>(out var player)) {
            if (player.activeMask == requiredMask) {
                effect.Invoke(context);
            }
        }
    }

    public void Invoke(CollisionInfo collision) {
        if (collision.gameObject.TryGetComponent<Player>(out var player)) {
            if (player.activeMask == requiredMask) {
                effect.Invoke(collision);
            }
        }
    }
}
using System;
using Slothsoft.Effects;
using Slothsoft.UnityExtensions;
using UnityEngine;

[ImplementationFor(typeof(IEffect), nameof(DamagePlayerEffect))]
[Serializable]
sealed class DamagePlayerEffect : IEffect {
    [SerializeField]
    int amount = 1;
    [SerializeField]
    MaskType immuneMask = MaskType.Unknown;

    public void Invoke() { }
    public void Invoke(GameObject context) {
        if (context.TryGetComponent<Player>(out var player)) {
            if (player.activeMask != immuneMask) {
                player.TakeDamage(amount);
            }
        }
    }
    public void Invoke(CollisionInfo collision) {
        if (collision.gameObject.TryGetComponent<Player>(out var player)) {
            if (player.activeMask != immuneMask) {
                player.TakeDamage(amount);
            }
        }
    }
}

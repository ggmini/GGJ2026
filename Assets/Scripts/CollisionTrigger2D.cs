using Slothsoft.Effects;
using UnityEngine;

sealed class CollisionTrigger2D : MonoBehaviour {
    [SerializeField]
    EffectEvent onTriggerEnter = new();
    [SerializeField]
    bool onlyCollideWithPlayer = true;

    void OnTriggerEnter2D(Collider2D collider) {
        if (!onlyCollideWithPlayer || collider.TryGetComponent<Player>(out _)) {
            onTriggerEnter.Invoke(collider.gameObject);
        }
    }
}
using Slothsoft.Effects;
using UnityEngine;

sealed class CollisionTrigger2D : MonoBehaviour {
    [SerializeField]
    EffectEvent onTriggerEnter = new();

    void OnTriggerEnter2D(Collider2D collider) {
        onTriggerEnter.Invoke(collider.gameObject);
    }
}
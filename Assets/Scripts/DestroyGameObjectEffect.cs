using System;
using Slothsoft.Effects;
using Slothsoft.UnityExtensions;
using UnityEngine;

[ImplementationFor(typeof(IEffect), nameof(DestroyGameObjectEffect))]
[Serializable]
sealed class DestroyGameObjectEffect : IEffect {

    public void Invoke() { }
    public void Invoke(GameObject context) {
        GameObject.Destroy(context);
    }
    public void Invoke(CollisionInfo collision) {
        GameObject.Destroy(collision.gameObject);
    }
}

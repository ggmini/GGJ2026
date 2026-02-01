using System;
using System.Collections;
using Slothsoft.Effects;
using Slothsoft.UnityExtensions;
using UnityEngine;

[ImplementationFor(typeof(IEffect), nameof(WaitThenDoEffect))]
[Serializable]
sealed class WaitThenDoEffect : IEffect {
    sealed class WaitThenDoComponent : MonoBehaviour {
    }

    [SerializeField]
    float waitDuration = 1;
    [SerializeField]
    EffectEvent effect = new();

    public void InvokeLater(Action action) {
        var context = new GameObject();
        var behaviour = context.AddComponent<WaitThenDoComponent>();
        behaviour.StartCoroutine(Invoke_Co(context, action));
    }

    public void Invoke() {
        InvokeLater(effect.Invoke);
    }

    public void Invoke(GameObject context) {
        InvokeLater(() => effect.Invoke(context));
    }

    public void Invoke(CollisionInfo collision) {
        InvokeLater(() => effect.Invoke(collision));
    }

    IEnumerator Invoke_Co(GameObject context, Action action) {
        yield return new WaitForSeconds(waitDuration);
        action();
        GameObject.Destroy(context);
    }
}

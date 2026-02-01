using System;
using MyBox;
using Slothsoft.Effects;
using Slothsoft.UnityExtensions;
using UnityEngine;

[ImplementationFor(typeof(IEffect), nameof(LoadSceneEffect))]
[Serializable]
sealed class LoadSceneEffect : IEffect {
    [SerializeField]
    SceneReference sceneToLoad = new();

    public void Invoke() {
        sceneToLoad.LoadSceneAsync();
    }
    public void Invoke(GameObject context) {
        sceneToLoad.LoadSceneAsync();
    }
    public void Invoke(CollisionInfo collision) {
        sceneToLoad.LoadSceneAsync();
    }
}

using System;
using MyBox;
using UnityEngine;

[CreateAssetMenu]
sealed class MainMenuAsset : ScriptableObject {
    [SerializeField]
    SceneReference[] levels = Array.Empty<SceneReference>();

    public void StartLevel1() {
        levels[0].LoadSceneAsync();
    }

    public void StartLevel2() {
        levels[1].LoadSceneAsync();
    }

    public void StartLevel3() {
        levels[2].LoadSceneAsync();
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
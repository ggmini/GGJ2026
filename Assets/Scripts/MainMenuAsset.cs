using MyBox;
using UnityEngine;

[CreateAssetMenu]
sealed class MainMenuAsset : ScriptableObject {
    [SerializeField]
    SceneReference gameScene = new();

    public void StartGame() {
        gameScene.LoadSceneAsync();
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
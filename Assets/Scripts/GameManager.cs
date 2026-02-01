using UnityEngine;
using UnityEngine.SceneManagement;

sealed class GameManager : MonoBehaviour
{
    public static void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LoadNextLevel() {
        if (SceneManager.GetActiveScene().name == "Level_1") {
            SceneManager.LoadScene("MainMenu"); //TODO: End Screen
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //play sound

    }

}

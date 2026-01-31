using UnityEngine;
using UnityEngine.SceneManagement;

sealed class GameManager : MonoBehaviour
{
    public static void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

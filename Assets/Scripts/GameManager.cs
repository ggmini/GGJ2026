using UnityEngine;
using UnityEngine.SceneManagement;

sealed class GameManager : MonoBehaviour
{
    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

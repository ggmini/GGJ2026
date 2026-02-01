using System.Collections;
using UnityEngine;

sealed class LevelExit : MonoBehaviour {

    bool triggered = false;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (triggered) return;
            triggered = true;
            StartCoroutine(WaitAndLoadNext());
        }
    }

    IEnumerator WaitAndLoadNext() {
        Debug.Log("Level Complete!");
        //play sound
        yield return new WaitForSeconds(2f);
        GameManager.LoadNextLevel();
    }

}

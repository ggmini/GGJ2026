using UnityEngine;

sealed class FollowPlayer : MonoBehaviour {
    [SerializeField]
    PlayerController player;

    void FixedUpdate() {
        var oldPosition = transform.position;
        var newPosition = player.transform.position;

        if (player.airborne && newPosition.y > oldPosition.y) {
            newPosition.y = oldPosition.y;
        }

        transform.position = newPosition;
    }
}

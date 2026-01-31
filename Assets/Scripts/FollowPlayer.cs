using UnityEngine;

sealed class FollowPlayer : MonoBehaviour {
    [SerializeField]
    Player player;

    void FixedUpdate() {
        var oldPosition = transform.position;
        var newPosition = player.transform.position;

        if (player.Airborne && newPosition.y > oldPosition.y) {
            newPosition.y = oldPosition.y;
        }

        transform.position = newPosition;
    }
}

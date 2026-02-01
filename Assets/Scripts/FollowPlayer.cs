using UnityEngine;

sealed class FollowPlayer : MonoBehaviour {
    void FixedUpdate() {
        if (Player.instance) {
            Apply(Player.instance);
        }
    }

    void Apply(Player player) {
        var oldPosition = transform.position;
        var newPosition = player.transform.position;

        if (player.Airborne && newPosition.y > oldPosition.y && player.activeMask != MaskType.Bunny) {
            newPosition.y = oldPosition.y;
        }

        transform.position = newPosition;
    }
}

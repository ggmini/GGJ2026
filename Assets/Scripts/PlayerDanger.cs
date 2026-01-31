using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

sealed class PlayerDanger : MonoBehaviour {
    [SerializeField]
    Tilemap tilemap;
    [SerializeField]
    TileBase dangerTile;
    [SerializeField]
    TileBase normalTile;

    [SerializeField]
    MaskType immuneMask = default;
    [SerializeField]
    bool isDangerous = false;

    void Start() {
        UpdateTiles();
    }

    void FixedUpdate() {
        if (Player.instance) {
            bool isDangerousNow = Player.instance.activeMask != immuneMask;
            if (isDangerous != isDangerousNow) {
                isDangerous = isDangerousNow;
                UpdateTiles();
            }
        }
    }

    void UpdateTiles() {
        var (source, target) = isDangerous ? (dangerTile, normalTile) : (normalTile, dangerTile);
        foreach (var (position, tile) in tilemap.GetUsedTiles()) {
            if (tile == source) {
                tilemap.SetTile(position, target);
            }
        }
    }
}

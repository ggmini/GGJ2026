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

    void Start() {
        UpdateTiles();
    }

    [SerializeField]
    bool isDangerous = false;

    void UpdateTiles() {
        var (source, target) = isDangerous ? (dangerTile, normalTile) : (normalTile, dangerTile);
        foreach (var (position, tile) in tilemap.GetUsedTiles()) {
            if (tile == source) {
                tilemap.SetTile(position, target);
            }
        }
    }
}
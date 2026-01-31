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
    bool isDangerous = false;

    void Start() {
        UpdateTiles();
    }

    void Update() {
        if (Player.instance) {
            //isDangerous = !isDangerous;
            //UpdateTiles();
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

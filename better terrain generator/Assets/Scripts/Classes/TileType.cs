using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileType
{
    [SerializeField] private Tile tile;

    [Range(0,1)]
    [SerializeField] private float height;

    public Tile getTile() {
        return tile;
    }

    public float getHeight() {
        return height;
    }
}
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    private Tilemap map;

    [SerializeField] private TileType[] tileTypes;
    [SerializeField] private TileType[] debugTiles;

    [SerializeField] private uint minIslands;
    [SerializeField] private uint maxIslands;

    void Awake()
    {
        map = gameObject.GetComponent<Tilemap>();

        Continent continent = new Continent(map, new Vector2Int(0,0), 10, 12, 100, debugTiles);
        continent.ChangeTerrain();
    }
}
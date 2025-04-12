using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    private Tilemap map;

    [SerializeField] private TileType[] tileTypes;
    [SerializeField] private TileType[] debugTiles;

    [SerializeField] private int minIslands;
    [SerializeField] private int maxIslands;

    void Awake()
    {
        map = gameObject.GetComponent<Tilemap>();

        Continent continent = new(map, new Vector2Int(0,0), minIslands, maxIslands, 100, tileTypes);

    }
}
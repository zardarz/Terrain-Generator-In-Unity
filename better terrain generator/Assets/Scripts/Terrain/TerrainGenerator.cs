using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    private Tilemap map;

    [SerializeField] private TileType[] tileTypes;
    [SerializeField] private TileType[] debugTiles;

    void Awake()
    {
        map = gameObject.GetComponent<Tilemap>();

        Continent continent = new Continent(map, new Vector2Int(50,0), 100, tileTypes);
        continent.ChangeTerrain();

        Continent continent1 = new(map, new(-50,0), 100, tileTypes);
        continent1.ChangeTerrain();
    }
}
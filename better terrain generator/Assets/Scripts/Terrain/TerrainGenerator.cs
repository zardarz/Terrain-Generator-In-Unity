using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    private Tilemap map;

    [SerializeField] private TileType[] tileTypes;
    [SerializeField] private TileType[] debugTiles;

    [SerializeField] private int minIslands;
    [SerializeField] private int maxIslands;

    private Continent continent;

    void Awake()
    {
        map = gameObject.GetComponent<Tilemap>();

        continent = new Continent(map, new Vector2Int(0,0), minIslands, maxIslands, 100, tileTypes);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            changeTerrain();
        }
    }

    private void changeTerrain() {
        continent.ChangeTerrain();
    }
}
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
        tileTypes = sort(tileTypes);

        Continent continent = new Continent(map, new Vector2Int(50,0), 100, tileTypes);
        continent.ChangeTerrain();

        Continent continent1 = new(map, new(-50,0), 100, tileTypes);
        continent1.ChangeTerrain();
    }

    private TileType[] sort(TileType[] tileTypes) {
        TileType[] sortedArray = new TileType[tileTypes.Length];

        TileType lowestTileType = tileTypes[0];

        while(sortedArray[tileTypes.Length-1] == null) {
            for(int i = 0; i < tileTypes.Length; i++) {
                if(tileTypes[i].getHeight() < lowestTileType.getHeight()) {
                    lowestTileType = tileTypes[i];
                    tileTypes[i].setHeight(float.PositiveInfinity);
                }
            }
        }

        return sortedArray;
    }
}
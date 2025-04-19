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
        bool[] used = new bool[tileTypes.Length];

        for (int i = 0; i < tileTypes.Length; i++) {
            int minIndex = -1;
            float minHeight = float.PositiveInfinity;

            for (int j = 0; j < tileTypes.Length; j++) {
                if (!used[j] && tileTypes[j].getHeight() < minHeight) {
                    minHeight = tileTypes[j].getHeight();
                    minIndex = j;
                }
            }

            if (minIndex != -1) {
                sortedArray[i] = tileTypes[minIndex];
                used[minIndex] = true;
            }
        }

        return sortedArray;
    }

}
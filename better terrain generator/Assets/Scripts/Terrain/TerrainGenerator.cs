using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    private Tilemap map;

    [SerializeField] private TileType[] tileTypes;
    [SerializeField] private TileType[] debugTiles;

    [SerializeField] private int minIslands;
    [SerializeField] private int maxIslands;

    public AnimationCurve islandCurve;

    private Continent continent;

    void Awake()
    {
        map = gameObject.GetComponent<Tilemap>();

        continent = new Continent(map, new Vector2Int(0,0), minIslands, maxIslands, 100, tileTypes);
        continent.ChangeTerrain();

        makeWave();
    }

    private void makeWave() {
        islandCurve.AddKey(0, 1);
        islandCurve.AddKey(1, 1);

        int amountOfKeys = Random.Range(1, 10);
        float keyJump = 1f / amountOfKeys;

        for (int i = 1; i < amountOfKeys; i++) {
            float keyTime = i * keyJump;
            float keyHeight = Random.Range(30, 100) / 100f;
            
            Debug.Log(keyHeight);

            islandCurve.AddKey(keyTime, keyHeight);
        }
    }
}
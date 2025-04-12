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
        int amountOfKeys = Random.Range(1, 5);
        float keyJump = 1f / amountOfKeys;

        for (int i = 0; i < amountOfKeys + 1; i++) {
            float keyTime = i * keyJump;
            float keyHeight = Random.Range(25, 75) / 100f;

            //Debug.Log(keyHeight);

            islandCurve.AddKey(keyTime, keyHeight);
        }
    }
}
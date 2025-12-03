using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using System.Collections;

public class TerrainGenerator : MonoBehaviour
{
    private Tilemap map;

    [SerializeField] private TileType[] tileTypes;
    [SerializeField] private TileType[] debugTiles;

    [SerializeField] private GameObject cameraGO;

    private Vector2 cameraPos;

    [SerializeField] private int continentChunkSize;

    private List<Vector2Int> continentChunks = new List<Vector2Int>();

    void Awake()
    {
        map = gameObject.GetComponent<Tilemap>();
        tileTypes = sort(tileTypes);
    }

    void Update()
    {
        cameraPos = (Vector2) cameraGO.transform.position;
        makeContinents();
    }

    private void makeContinents() {
        Vector2Int cameraChunk = new(roundNum(cameraPos.x, continentChunkSize), roundNum(cameraPos.y, continentChunkSize));
        
        // Generate chunks in a radius around the camera
        int chunkRadius = 2; // Adjust this to control how far ahead to generate (2 = 5x5 grid of chunks)
        
        for(int x = -chunkRadius; x <= chunkRadius; x++) {
            for(int y = -chunkRadius; y <= chunkRadius; y++) {
                Vector2Int chunkPos = new(cameraChunk.x + x, cameraChunk.y + y);
                
                if(continentChunks.Contains(chunkPos) == false) {
                    Debug.Log($"Generating continent at chunk {chunkPos}");
                    makeContinent(chunkPos);
                    continentChunks.Add(chunkPos);
                }
            }
        }
    }

    private void makeContinent(Vector2Int roundedPos) {
        // Use chunk position to create a consistent seed for this chunk
        int seed = roundedPos.x * 73856093 ^ roundedPos.y * 19349663;
        System.Random chunkRng = new System.Random(seed);
        
        // Add random offset within the chunk (but not too close to edges to avoid overlap)
        float offsetRange = continentChunkSize * 0.3f;
        float xOffset = (float)(chunkRng.NextDouble() * 2 - 1) * offsetRange;
        float yOffset = (float)(chunkRng.NextDouble() * 2 - 1) * offsetRange;
        
        int xPos = roundedPos.x * continentChunkSize + (continentChunkSize / 2) + (int)xOffset;
        int yPos = roundedPos.y * continentChunkSize + (continentChunkSize / 2) + (int)yOffset;
        
        float radius = (float)(chunkRng.NextDouble() * (continentChunkSize / 2.5f - 20) + 20);
        
        Debug.Log($"Creating continent at ({xPos}, {yPos}) with radius {radius}");

        // Start coroutine instead of thread
        StartCoroutine(GenerateContinentAsync(xPos, yPos, radius, seed));
    }


    private IEnumerator GenerateContinentAsync(int xPos, int yPos, float radius, int seed) {
        Continent newContinent = new(new(xPos, yPos), radius, tileTypes, seed);
        
        // This might take a frame or two depending on continent size
        Tile[][] terrain = newContinent.GetTerrainData();
        
        yield return null; // Wait one frame before starting to place tiles
        
        // Now place the tiles (this is already spread across frames)
        yield return StartCoroutine(changeTerrain(terrain, new(xPos - (int)radius, yPos - (int)radius)));
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


    private int roundNum(float numToRound, float numToRoundTo) {
        return Mathf.FloorToInt(numToRound / numToRoundTo);
    }

    IEnumerator changeTerrain(Tile[][] tiles, Vector2Int pos) {
        for(int x = 0; x < tiles.Length; x++) {
            for(int y = 0; y < tiles[x].Length; y++) {
                if(tiles[x][y] != null) {
                    map.SetTile(new(pos.x + x, pos.y + y, 0), tiles[x][y]);
                }
            }
            
            if (x % 10 == 0) yield return null; // Spread across frames to avoid lag
        }
    }
}
using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

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
        Vector2Int roundedPos = new(roundNum(cameraPos.x,continentChunkSize), roundNum(cameraPos.y,continentChunkSize));

        if(continentChunks.Contains(roundedPos) == false) {
            int amountOfConinents = Random.Range(1,2);

            for(int i = 0; i < amountOfConinents; i++) {
                makeContinent(roundedPos);
            }

            continentChunks.Add(roundedPos);
        }
    }

    private void makeContinent(Vector2Int roundedPos) {
        float radius = Random.Range(20, 400);
        int xPos = Random.Range(roundedPos.x * continentChunkSize, (roundedPos.x + 1) * continentChunkSize);
        int yPos = Random.Range(roundedPos.y * continentChunkSize, (roundedPos.y + 1) * continentChunkSize);

        Continent newContinent = new(new(xPos, yPos), radius, tileTypes);

        Thread thread = new(() => {
            Tile[][] terrain = newContinent.GetTerrainData();

            // ✅ Schedule both logging and tilemap updates on the main thread
            MainThreadDispatcher.Enqueue(() => {

                // ✅ Now safe to update the Tilemap here
                changeTerrain(terrain, new(xPos - (int)radius, yPos - (int)radius));
            });
        });

        thread.Start();
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
        return (int) Math.Round(numToRound/numToRoundTo);
    }

    private void changeTerrain(Tile[][] tileData, Vector2Int pos) {
        Vector3Int[] positions = new Vector3Int[tileData.Length * tileData[0].Length];
        Tile[] tiles = new Tile[tileData.Length * tileData[0].Length];

        int i = 0;
        for(int x = 0; x < tileData.Length; x++) {
            for(int y = 0; y < tileData[0].Length; y++) {
                positions[i] = new(x + pos.x, y+pos.y, 0);
                tiles[i] = tileData[x][y];
                i++;
            }
        }

        map.SetTiles(positions, tiles);
    }
}
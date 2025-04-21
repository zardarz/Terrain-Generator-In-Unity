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
        Vector2Int roundedPos = new(roundNum(cameraPos.x,continentChunkSize), roundNum(cameraPos.y,continentChunkSize));

        if(continentChunks.Contains(roundedPos) == false) {
            int amountOfConinents = Random.Range(5,10);

            for(int i = 0; i<amountOfConinents;i++) {
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

    IEnumerator changeTerrain(Tile[][] tiles, Vector2Int pos) {
        for(int x = 0; x < tiles.Length; x++) {
            for(int y = 0; y < tiles[0].Length; y++) {
                map.SetTile(new(pos.x + x, pos.y + y, 0), tiles[x][y]);
            }

            if (x % 10 == 0) yield return null;
        }
    }
}
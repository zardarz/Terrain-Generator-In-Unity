using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;
using Random = UnityEngine.Random;

public class Continent
{
    private Tilemap map;
    private Vector2Int position;

    private Island[] islands;
    private readonly int amountOfIslands;

    private readonly double radious;

    private TileType[] tileTypes;

    public Continent(Tilemap map, Vector2Int pos, int minIslands, int maxIslands, double radious, TileType[] tileTypes) {
        this.map = map;
        this.radious = radious;
        this.tileTypes = tileTypes;
        position = pos;

        islands = new Island[Random.Range(minIslands, maxIslands + 1)];
        amountOfIslands = islands.Length;
        GenerateIslands();
    }

    private void GenerateIslands() {
        for(int i = 0; i < amountOfIslands; i++) {
            Vector2 newPos = Random.insideUnitCircle;

            Vector2Int newPosInt = new((int) (newPos.x * radious), (int) (newPos.y * radious));

            Island newIsland = new(newPosInt);

            islands[i] = newIsland;
        }
    }

    public void ChangeTerrain() {
        for(int x = 0; x < radious*2; x++) {
            for(int y = 0; y < radious*2; y++) {
                double[] distances = new double[amountOfIslands];
                double[] values = new double[amountOfIslands];

                Vector2Int currentTile = new Vector2Int((int) (x - radious) , (int) (y - radious));

                for(int distanceIndex = 0; distanceIndex < amountOfIslands; distanceIndex++) {
                    distances[distanceIndex] = Vector2.Distance(currentTile, islands[distanceIndex].getPos());
                }

                for(int valueIndex = 0; valueIndex < amountOfIslands; valueIndex++) {
                    values[valueIndex] = islands[valueIndex].getWave(distances[valueIndex]);
                }

                double[] weightedValues = new double[amountOfIslands];

                for(int i = 0; i < amountOfIslands; i++) {
                    weightedValues[i] = values[i] / Math.Max(1, distances[i]);
                }

                double finalHeight = 1;

                for(int i = 0; i < amountOfIslands; i++) {
                    finalHeight *= weightedValues[i];
                }

                //finalHeight *= 10;

                Debug.Log(finalHeight);

                map.SetTile(new Vector3Int(currentTile.x + position.x, currentTile.y + position.y, 0), GetTileByHeight(finalHeight));
            }
        }
    }

    private Tile GetTileByHeight(double height) {

        for(int i = tileTypes.Length - 1; i >= 0; i--) {
            if(height >= tileTypes[i].getHeight()) {
                return tileTypes[i].getTile();
            }
        }

        return tileTypes[0].getTile();
    }
}
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

    private readonly float radious;

    private readonly TileType[] tileTypes;
    

    public Continent(Tilemap map, Vector2Int pos, int minIslands, int maxIslands, float radious, TileType[] tileTypes) {
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

                Vector2Int currentTile = new((int) (x - radious) ,(int) (y - radious));

                if(Vector2.Distance(currentTile, position) < radious) {
                    float[] distances = GetAllDistances(currentTile);
                    float[] values = GetAllWaveValues(distances);

                    float[] weightedValues = GetWeightedValues(values, distances);

                    float finalHeight = GetProduct(weightedValues);

                    //Debug.Log(finalHeight);

                    map.SetTile(new Vector3Int(currentTile.x + position.x, currentTile.y + position.y, 0), GetTileByHeight(finalHeight));
                }
            }
        }
    }

    private float[] GetAllDistances(Vector2Int currentTile) {
        float[] distances = new float[amountOfIslands];

        for(int distanceIndex = 0; distanceIndex < amountOfIslands; distanceIndex++) {
            float distance = Vector2.Distance(currentTile, islands[distanceIndex].getPos());

            if(distance == 0) {
                distance = 1;
            }

            distances[distanceIndex] = distance;
        }

        return distances;
    }

    private float[] GetAllWaveValues(float[] distances) {
        float[] values = new float[amountOfIslands];

        for(int valueIndex = 0; valueIndex < amountOfIslands; valueIndex++) {
            values[valueIndex] = islands[valueIndex].getWave(distances[valueIndex]);
        }

        return values;
    }

    private float[] GetWeightedValues(float[] values, float[] distances) {
        float[] weightedValues = new float[amountOfIslands];

        for(int i = 0; i < amountOfIslands; i++) {
            if(distances[i] != 0) {
                weightedValues[i] = values[i] / distances[i];
            }
        }

        return weightedValues;
    }

    private float GetProduct(float[] weightedValues) {
        float finalProduct = 1;

        for(int i = 0; i < amountOfIslands; i++) {
            finalProduct *= weightedValues[i];
        }

        return finalProduct;
    }

    private Tile GetTileByHeight(float height) {

        for(int i = tileTypes.Length - 1; i >= 0; i--) {
            if(height > tileTypes[i].getHeight()) {
                return tileTypes[i].getTile();
            }
        }

        return tileTypes[0].getTile();
    }
}
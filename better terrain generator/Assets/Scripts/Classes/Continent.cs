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
        for (int i = 0; i < amountOfIslands; i++) {
            Vector2 newPos = Random.insideUnitCircle;
            Vector2Int newPosInt = new((int)(newPos.x * radious), (int)(newPos.y * radious));
            Island newIsland = new(newPosInt);
            islands[i] = newIsland;
        }
    }

    public void ChangeTerrain() {
        for (int x = 0; x < radious * 2; x++) {
            for (int y = 0; y < radious * 2; y++) {
                Vector2Int currentTile = new((int)(x - radious), (int)(y - radious));

                if (Vector2.Distance(currentTile, position) < radious) {
                    float[] distances = GetAllDistances(currentTile);
                    float[] values = GetAllWaveValues(distances);

                    float finalHeight = GetWeightedAverage(values, distances);

                    map.SetTile(new Vector3Int(currentTile.x + position.x, currentTile.y + position.y, 0), GetTileByHeight(finalHeight));
                }
            }
        }
    }

    private float[] GetAllDistances(Vector2Int currentTile) {
        float[] distances = new float[amountOfIslands];

        for (int i = 0; i < amountOfIslands; i++) {
            float distance = Vector2.Distance(currentTile, islands[i].getPos());
            distances[i] = distance / (radious * 2); // Normalize to [0,1]
        }

        return distances;
    }

    private float[] GetAllWaveValues(float[] distances) {
        float[] values = new float[amountOfIslands];

        for (int i = 0; i < amountOfIslands; i++) {
            values[i] = islands[i].getWave(distances[i]);
        }

        return values;
    }

    private float GetWeightedAverage(float[] values, float[] distances) {
        float sum = 0f;
        float totalWeight = 0f;

        for (int i = 0; i < values.Length; i++) {
            float weight = Mathf.Pow(Mathf.Clamp01(1f - distances[i]), 2f); // Squared falloff
            sum += values[i] * weight;
            totalWeight += weight;
        }

        if (totalWeight == 0f) return 0f;

        return sum / totalWeight;
    }

    private Tile GetTileByHeight(float height) {
        for (int i = tileTypes.Length - 1; i >= 0; i--) {
            if (height > tileTypes[i].getHeight()) {
                return tileTypes[i].getTile();
            }
        }

        return tileTypes[0].getTile();
    }
}

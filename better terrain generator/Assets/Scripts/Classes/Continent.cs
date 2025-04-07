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

    private readonly TileType[] tileTypes;
    

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

                Vector2Int currentTile = new((int) (x - radious) ,(int) (y - radious));

                if(Vector2.Distance(currentTile, position) < radious) {
                    double[] distances = GetAllDistances(currentTile);
                    double[] values = GetAllWaveValues(distances);

                    double[] weightedValues = GetWeightedValues(values, distances);

                    double finalHeight = GetProduct(weightedValues) * 10;

                    Debug.Log(finalHeight);

                    map.SetTile(new Vector3Int(currentTile.x + position.x, currentTile.y + position.y, 0), GetTileByHeight(finalHeight));
                }
            }
        }
    }

    private double[] GetAllDistances(Vector2Int currentTile) {
        double[] distances = new double[amountOfIslands];

        for(int distanceIndex = 0; distanceIndex < amountOfIslands; distanceIndex++) {
            distances[distanceIndex] = Vector2.Distance(currentTile, islands[distanceIndex].getPos());
        }

        return distances;
    }

    private double[] GetAllWaveValues(double[] distances) {
        double[] values = new double[amountOfIslands];

        for(int valueIndex = 0; valueIndex < amountOfIslands; valueIndex++) {
            values[valueIndex] = islands[valueIndex].getWave(distances[valueIndex]);
        }

        return values;
    }

    private double[] GetWeightedValues(double[] values, double[] distances) {
        double[] weightedValues = new double[amountOfIslands];

        for(int i = 0; i < amountOfIslands; i++) {
            weightedValues[i] = values[i] / Math.Min(1, distances[i]);
        }

        return weightedValues;
    }

    private double GetProduct(double[] weightedValues) {
        double finalProduct = 1;

        for(int i = 0; i < amountOfIslands; i++) {
            finalProduct *= weightedValues[i];
        }

        return finalProduct;
    }

    private Tile GetTileByHeight(double height) {

        for(int i = tileTypes.Length - 1; i >= 0; i--) {
            if(height > tileTypes[i].getHeight()) {
                return tileTypes[i].getTile();
            }
        }

        return tileTypes[0].getTile();
    }
}
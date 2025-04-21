using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Random = UnityEngine.Random;
using System;
using UnityEngine.Tilemaps;

public class Continent
{
    private Vector2Int position;
    private Island[] islands;
    private IslandComponent[] islandComponents;
    private TileType[] tileTypes;
    private readonly int amountOfIslands;
    private readonly float radius;

    private System.Random rng;

    public Continent(Vector2Int position, float radius, TileType[] tileTypes, int seed = -1) {
        this.radius = radius;
        this.position = position;
        this.tileTypes = tileTypes;

        rng = seed == -1 ? new System.Random() : new System.Random(seed);

        amountOfIslands = rng.Next((int)radius / 10, (int)radius / 10 + 5);
        islands = new Island[amountOfIslands];
        islandComponents = new IslandComponent[amountOfIslands];
        GenerateIslands();
    }


    private void GenerateIslands() {
        GameObject islandParent = new("Islands");

        for(int i = 0; i < amountOfIslands; i++) {
            Vector2 newPos = Random.insideUnitCircle;

            Vector2Int newPosInt = new((int) (newPos.x * radius) + position.x, (int) (newPos.y * radius) + position.y);

            Island newIsland = new(newPosInt, randomNum(0,radius));
            
            MakeIslandComponent(islandParent.transform, newIsland, newPosInt, i);

            islands[i] = newIsland;
        }

        GameObject continentParent = MakeContinentComponent(this);
        islandParent.transform.parent = continentParent.transform;
    }

    private void MakeIslandComponent(Transform parent, Island island, Vector2Int pos, int index) {

        GameObject islandGO = new(pos.ToString());
        islandGO.transform.parent = parent.transform;

        islandGO.transform.position = new(pos.x, pos.y, 0);

        IslandComponent islandComponent = islandGO.AddComponent<IslandComponent>();
        islandComponent.setIsland(island);

        islandComponents[index] = islandComponent;
    }

    private GameObject MakeContinentComponent(Continent continent) {
        GameObject continentGO = new("Continent: " + position.ToString());
        continentGO.transform.position = new(continent.position.x, continent.position.y, 0);
        ContinentComponent continentComponent = continentGO.AddComponent<ContinentComponent>();

        continentComponent.setContinent(continent);

        return continentGO;
    }

    public Tile[][] GetTerrainData() {

        Tile[][] heights = new Tile[(int) Math.Round(radius*2)][];

        for(int x = 0; x < radius*2; x++) {
            heights[x] = new Tile[(int) Math.Round(radius*2)];

            for(int y = 0; y < radius*2; y++) {

                Vector2Int currentTile = new((int) (x - radius + position.x) ,(int) (y - radius + position.y));

                if(Vector2.Distance(currentTile, position) < radius) {
                    float finalHeight = getHeight(currentTile) * 10f;

                    heights[x][y] = GetTileByHeight(finalHeight);
                }
            }
        }

        return heights;
    }

    private float getHeight(Vector2Int currentTile) {
        float sum = 0;

        for(int i = 0; i < amountOfIslands; i++) {
            Island island = islands[i];

            float distance = GetDistance(currentTile, island);
            float waveValue = GetWaveValue(island, distance);

            float weightedWaveValue = GetWeightedWaveValue(waveValue, distance, island);

            if(distance != 0) {
                sum += weightedWaveValue;
            }
        }

        float distanceFromCenterMultiplyer = getDistanceFromCenterMultiplyer(currentTile);

        return sum / amountOfIslands * (randomNum(90,100) / 100f) * distanceFromCenterMultiplyer;
    }
    
    private float GetDistance(Vector2Int currentTile, Island island) {
        float distance = Vector2.Distance(currentTile, island.getPos());

        if(distance == 0) {
            distance = 1;
        }

        return distance;
    }

    private float GetWaveValue(Island island, float distance) {
        float islandWaveValue = island.getWave(distance);

        return islandWaveValue;
    }

    private float GetWeightedWaveValue(float waveValue, float distance, Island island) {
        float islandRadius = island.getIslandRadius();
        
        if (distance >= islandRadius) return 0f;

        float weight = 1f - (distance / islandRadius);
        float weightedWaveValue = waveValue * weight;

        return weightedWaveValue;
    }

    private float getDistanceFromCenterMultiplyer(Vector2Int currentTile) {
        float distanceFromCenterMultiplyer;
        float distanceFromCenter = Vector2.Distance(currentTile,position) / radius;

        if(distanceFromCenter < 0.8f) {
            distanceFromCenterMultiplyer = 1f;
        } else {
            distanceFromCenterMultiplyer = -25 * Mathf.Pow(distanceFromCenter - .8f , 2) + 1;
        }

        return distanceFromCenterMultiplyer;
    }

    private Tile GetTileByHeight(float height) {

        for(int i = tileTypes.Length - 1; i >= 0; i--) {
            if(height > tileTypes[i].getHeight()) {
                return tileTypes[i].getTile();
            }
        }

        return tileTypes[0].getTile();
    }

    private float randomNum(float min, float max) {
        return (float) rng.NextDouble() * (max-min) + min;
    }

    public float getContinentRadius() {
        return radius;
    }

    public Vector2Int getContinentPosition() {
        return position;
    }

    public IslandComponent[] getIslandComponents() {
        return islandComponents;
    }
}
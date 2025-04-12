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

            Vector2Int newPosInt = new(0,0);

            Island newIsland = new(newPosInt, radious, Random.Range(0,radious), Random.Range(3,15));

            islands[i] = newIsland;
        }
    }

    public void ChangeTerrain() {
        for(int x = 0; x < radious*2; x++) {
            for(int y = 0; y < radious*2; y++) {

                Vector2Int currentTile = new((int) (x - radious) ,(int) (y - radious));

                if(Vector2.Distance(currentTile, position) < radious) {
                    float finalHeight = getHeight(currentTile);

                    map.SetTile(new Vector3Int(currentTile.x + position.x, currentTile.y + position.y, 0), GetTileByHeight(finalHeight));
                }
            }
        }
    }

    private float getHeight(Vector2Int currentTile) {
        float sum = 0;

        for(int i = 0; i < amountOfIslands; i++) {
            Island island = islands[i];

            float distance = GetDistance(currentTile, island);
            float waveValue = GetWaveValue(island, distance);


            if(distance != 0) {
                sum += waveValue;
            }
        }

        float distanceFromCenterMultiplyer;
        float distanceFromCenter = Vector2.Distance(currentTile,new(0,0)) / radious;

        if(distanceFromCenter < 0.8f) {
            distanceFromCenterMultiplyer = 1f;
        } else {
            distanceFromCenterMultiplyer = -25 * Mathf.Pow(distanceFromCenter - .8f , 2) + 1;
        }

        return sum / amountOfIslands * 10 * (Random.Range(95,100) / 100f) * distanceFromCenterMultiplyer;
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
        float islandRadious = island.getIslandRadious();

        islandWaveValue *= islandRadious / radious * island.getIslandScale();

        return islandWaveValue;
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
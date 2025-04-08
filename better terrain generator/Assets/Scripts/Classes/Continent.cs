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

            Island newIsland = new(newPosInt, radious, Random.Range(0,radious));

            islands[i] = newIsland;
        }
    }

    public void ChangeTerrain() {
        for(int x = 0; x < radious*2; x++) {
            for(int y = 0; y < radious*2; y++) {

                Vector2Int currentTile = new((int) (x - radious) ,(int) (y - radious));

                if(Vector2.Distance(currentTile, position) < radious) {
                    float finalHeight = getHeight(currentTile);

                    Debug.Log(finalHeight);

                    map.SetTile(new Vector3Int(currentTile.x + position.x, currentTile.y + position.y, 0), GetTileByHeight(finalHeight));
                }
            }
        }
    }

    private float getHeight(Vector2Int currentTile) {
        float[][] distancesAndValues = new float[2][];

        float sum = 0;

        for (int i = 0; i < 2; i++)
        {
            distancesAndValues[i] = new float[amountOfIslands];
        }

        for(int i = 0; i < amountOfIslands; i++) {
            Island island = islands[i];


            float distance = Vector2.Distance(currentTile, island.getPos());

            if(distance == 0) {
                distance = 1;
            }

            distancesAndValues[0][i] = distance;


            float islandWaveValue = island.getWave(distance);
            float islandRadious = island.getIslandRadious();

            Debug.Log(islandRadious);

            distancesAndValues[1][i] = islandWaveValue * ((islandRadious / radious) + 1);


            if(distancesAndValues[0][i] != 0) {
                sum += distancesAndValues[1][i] / distancesAndValues[0][i];
            }

        }

        return sum / amountOfIslands;
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
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;
using Random = UnityEngine.Random;

public class Continent
{
    private Tilemap map;
    private Vector2Int position;
    private Island[] islands;
    private IslandComponent[] islandComponents;
    private readonly int amountOfIslands;

    private readonly float radius;

    private readonly TileType[] tileTypes;
    

    public Continent(Tilemap map, Vector2Int position, float radius, TileType[] tileTypes) {
        this.map = map;
        this.radius = radius;
        this.tileTypes = tileTypes;
        this.position = position;

        amountOfIslands = Random.Range((int) radius/10, (int) radius/10 + 5);
        islands = new Island[amountOfIslands];
        islandComponents = new IslandComponent[amountOfIslands];
        GenerateIslands();
    }

    private void GenerateIslands() {
        GameObject islandParent = new("Islands");

        for(int i = 0; i < amountOfIslands; i++) {
            Vector2 newPos = Random.insideUnitCircle;

            Vector2Int newPosInt = new((int) (newPos.x * radius) + position.x, (int) (newPos.y * radius) + position.y);

            Island newIsland = new(newPosInt, Random.Range(0,radius));
            
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
        ContinentComponent continentComponent = continentGO.AddComponent<ContinentComponent>();

        continentComponent.setContinent(continent);

        return continentGO;
    }

    public void ChangeTerrain() {
        for(int x = 0; x < radius*2; x++) {
            for(int y = 0; y < radius*2; y++) {

                Vector2Int currentTile = new((int) (x - radius + position.x) ,(int) (y - radius + position.y));

                if(Vector2.Distance(currentTile, position) < radius) {
                    float finalHeight = getHeight(currentTile) * 10f;

                    map.SetTile(new Vector3Int(currentTile.x, currentTile.y, 0), GetTileByHeight(finalHeight));
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

            float weightedWaveValue = GetWeightedWaveValue(waveValue, distance, island);

            if(distance != 0) {
                sum += weightedWaveValue;
            }
        }

        float distanceFromCenterMultiplyer = getDistanceFromCenterMultiplyer(currentTile);

        return sum / amountOfIslands * (Random.Range(90,100) / 100f) * distanceFromCenterMultiplyer;
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
        float radius = island.getIslandRadius();
        
        if (distance >= radius) return 0f;

        float weight = 1f - (distance / radius);
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
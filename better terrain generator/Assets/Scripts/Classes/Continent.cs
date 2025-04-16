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

    private readonly float radious;

    private readonly TileType[] tileTypes;
    

    public Continent(Tilemap map, Vector2Int pos, int minIslands, int maxIslands, float radious, TileType[] tileTypes) {
        this.map = map;
        this.radious = radious;
        this.tileTypes = tileTypes;
        position = pos;

        islands = new Island[Random.Range(minIslands, maxIslands + 1)];
        amountOfIslands = islands.Length;
        islandComponents = new IslandComponent[amountOfIslands];
        GenerateIslands();
    }

    private void GenerateIslands() {
        GameObject islandParent = new("Islands");

        for(int i = 0; i < amountOfIslands; i++) {
            Vector2 newPos = Random.insideUnitCircle;

            Vector2Int newPosInt = new((int) (newPos.x * radious), (int) (newPos.y * radious));

            Island newIsland = new(newPosInt, Random.Range(0,radious));
            
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
        for(int x = 0; x < radious*2; x++) {
            for(int y = 0; y < radious*2; y++) {

                Vector2Int currentTile = new((int) (x - radious) ,(int) (y - radious));

                if(Vector2.Distance(currentTile, position) < radious) {
                    float finalHeight = getHeight(currentTile) * 10f;

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

            float weightedWaveValue = GetWeightedWaveValue(waveValue, distance, island);

            if(distance != 0) {
                sum += weightedWaveValue;
            }
        }

        float distanceFromCenterMultiplyer;
        float distanceFromCenter = Vector2.Distance(currentTile,new(0,0)) / radious;

        if(distanceFromCenter < 0.8f) {
            distanceFromCenterMultiplyer = 1f;
        } else {
            distanceFromCenterMultiplyer = -25 * Mathf.Pow(distanceFromCenter - .8f , 2) + 1;
        }

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
        float radius = island.getIslandRadious();
        
        if (distance >= radius) return 0f;

        float weight = 1f - (distance / radius);
        float weightedWaveValue = waveValue * weight;

        if(distance <= 10f) {
            Debug.Log(distance + " : " + weightedWaveValue + " : " + weight + " : " + waveValue);
        }

        return weightedWaveValue;
    }


    private Tile GetTileByHeight(float height) {

        for(int i = tileTypes.Length - 1; i >= 0; i--) {
            if(height > tileTypes[i].getHeight()) {
                return tileTypes[i].getTile();
            }
        }

        return tileTypes[0].getTile();
    }


    public float getContinentRadious() {
        return radious;
    }

    public Vector2Int getContinentPosition() {
        return position;
    }

    public IslandComponent[] getIslandComponents() {
        return islandComponents;
    }
}
using UnityEngine;

public class ContinentComponent : MonoBehaviour
{
    public float continentRadius;
    public IslandComponent[] islandComponents;
    public Vector2Int position;

    public void setContinent(Continent continent) {
        continentRadius = continent.getContinentRadius();
        islandComponents = continent.getIslandComponents();
        position = continent.getContinentPosition();
    }
}
using UnityEngine;

public class ContinentComponent : MonoBehaviour
{
    public float continentRadious;
    public IslandComponent[] islandComponents;
    public Vector2Int position;

    public void setContinent(Continent continent) {
        continentRadious = continent.getContinentRadious();
        islandComponents = continent.getIslandComponents();
        position = continent.getContinentPosition();
    }
}
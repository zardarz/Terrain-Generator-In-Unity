using UnityEngine;

[System.Serializable]
public class IslandComponent : MonoBehaviour
{

    public AnimationCurve islandCurve;

    public float islandRadious;

    public Vector2Int islandPosition;

    public void setIsland(Island island) {
        islandCurve = island.getIslandCurve();
        islandRadious = island.getIslandRadious();
        islandPosition = island.getPos();
    }
}
using UnityEngine;

[System.Serializable]
public class IslandComponent : MonoBehaviour
{

    public AnimationCurve islandCurve;

    public float islandRadius;

    public Vector2Int islandPosition;

    public void setIsland(Island island) {
        islandCurve = island.getIslandCurve();
        islandRadius = island.getIslandRadius();
        islandPosition = island.getPos();
    }
}
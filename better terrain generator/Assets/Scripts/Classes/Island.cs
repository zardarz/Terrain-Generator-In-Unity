using UnityEngine;

public class Island
{
    private AnimationCurve graphCurve;

    private Vector2Int position;

    private readonly float continentRadious;

    private readonly float islandRadious;

    private readonly int islandScale;

    public Island(Vector2Int pos, float radious, float islandRadious, int islandScale) {
        position = pos;
        continentRadious = radious; 
        this.islandRadious = islandRadious;
        this.islandScale = islandScale;
    }
    public float getWave(float x) {
        return 1;
    }

    public Vector2Int getPos() {
        return position;
    }

    public float getIslandRadious() {
        return islandRadious;
    }

    public int getIslandScale() {
        return islandScale;
    }
}

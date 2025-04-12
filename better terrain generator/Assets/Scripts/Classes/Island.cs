using UnityEngine;

[System.Serializable]
public class Island
{
    private AnimationCurve graphCurve;

    private Vector2Int position;

    private readonly float continentRadious;

    private readonly float islandRadious;

    private readonly int islandScale;

    public Island(Vector2Int pos, float radious, float islandRadious) {
        position = pos;
        continentRadious = radious;
        this.islandRadious = islandRadious;
        islandScale = Random.Range(1,10);
    }
    public float getWave(float x) {
        return Mathf.Cos(2/continentRadious);
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

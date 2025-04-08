using UnityEngine;

public class Island
{
    private AnimationCurve graphCurve;

    private Vector2Int position;

    private readonly float continentRadious;

    public Island(Vector2Int pos, float radious) {
        position = pos;
        continentRadious = radious;
    }
    public float getWave(float x) {
        return Mathf.Cos(continentRadious * x*(1/100));
    }

    public Vector2Int getPos() {
        return position;
    }
}

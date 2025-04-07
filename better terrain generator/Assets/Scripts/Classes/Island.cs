using UnityEngine;

public class Island
{
    private AnimationCurve graphCurve;

    private Vector2Int position;

    public Island(Vector2Int pos) {
        position = pos;
    }
    public float getWave(float x) {
        return Mathf.Sin(x);
    }

    public Vector2Int getPos() {
        return position;
    }
}

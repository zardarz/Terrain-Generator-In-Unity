using UnityEngine;

public class Island
{
    private AnimationCurve graphCurve;

    private Vector2Int position;

    public Island(Vector2Int pos) {
        position = pos;
    }
    public double getWave(double x) {
        return Mathf.Sin((float) x);
    }

    public Vector2Int getPos() {
        return position;
    }
}

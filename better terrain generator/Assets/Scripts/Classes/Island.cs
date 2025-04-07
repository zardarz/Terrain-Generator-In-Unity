using UnityEngine;

public class Island
{
    private AnimationCurve graphCurve;

    private Vector2Int position;

    public Island(Vector2Int pos) {
        position = pos;
    }
    public float getWave(float x) {
        return Mathf.Sin((float) (x * 0.1f))*0.5f + 0.5f;
    }

    public Vector2Int getPos() {
        return position;
    }
}

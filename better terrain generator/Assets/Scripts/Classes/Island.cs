using UnityEngine;

public class Island
{
    private AnimationCurve islandCurve;

    private Vector2Int position;

    private readonly float islandRadius;

    public Island(Vector2Int pos, float islandRadius) {
        islandCurve = new();
        position = pos;
        this.islandRadius = islandRadius;

        makeWave();
    }

    private void makeWave() {
        int amountOfKeys = Random.Range(1, 5);
        float keyJump = 1f / amountOfKeys;

        for (int i = 0; i < amountOfKeys + 1; i++) {
            float keyTime = i * keyJump;
            float keyHeight = Random.Range(0, 100) / 100f;

            islandCurve.AddKey(keyTime, keyHeight);
        }
    }

    public float getWave(float x) {
        return islandCurve.Evaluate(x/islandRadius);
    }

    public Vector2Int getPos() {
        return position;
    }

    public float getIslandRadius() {
        return islandRadius;
    }

    public AnimationCurve getIslandCurve() {
        return islandCurve;
    }
}

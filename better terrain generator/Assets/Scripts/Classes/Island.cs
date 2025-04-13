using UnityEngine;

public class Island
{
    private AnimationCurve islandCurve;

    private Vector2Int position;

    private readonly float islandRadious;

    public Island(Vector2Int pos, float islandRadious) {
        islandCurve = new();
        position = pos;
        this.islandRadious = islandRadious;

        makeWave();
    }

    private void makeWave() {
        int amountOfKeys = Random.Range(1, 10);
        float keyJump = 1f / amountOfKeys;

        for (int i = 0; i < amountOfKeys + 1; i++) {
            float keyTime = i * keyJump;
            float keyHeight = Random.Range(80, 100) / 100f;

            //Debug.Log(keyHeight);

            islandCurve.AddKey(keyTime, keyHeight);
        }
    }

    public float getWave(float x) {
        return islandCurve.Evaluate(x/islandRadious);
    }

    public Vector2Int getPos() {
        return position;
    }

    public float getIslandRadious() {
        return islandRadious;
    }

    public AnimationCurve getIslandCurve() {
        return islandCurve;
    }
}

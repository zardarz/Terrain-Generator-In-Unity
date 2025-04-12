using UnityEngine;

public class Island
{
    private AnimationCurve islandCurve;

    private Vector2Int position;

    private readonly float continentRadious;

    private readonly float islandRadious;

    private readonly int islandScale;

    public Island(Vector2Int pos, float radious, float islandRadious, int islandScale) {
        islandCurve = new();
        position = pos;
        continentRadious = radious; 
        this.islandRadious = islandRadious;
        this.islandScale = islandScale;

        makeWave();
    }

    private void makeWave() {
        islandCurve.AddKey(0, Random.Range(0, 100) / 100f);
        islandCurve.AddKey(1, Random.Range(0, 100) / 100f);

        int amountOfKeys = Random.Range(1, 100);
        float keyJump = 1f / amountOfKeys;

        for (int i = 1; i < amountOfKeys; i++) {
            float keyTime = i * keyJump;
            float keyHeight = Random.Range(25, 75) / 100f;

            //Debug.Log(keyHeight);

            islandCurve.AddKey(keyTime, keyHeight);
        }
    }

    public float getWave(float x) {
        return islandCurve.Evaluate(x/continentRadious);
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

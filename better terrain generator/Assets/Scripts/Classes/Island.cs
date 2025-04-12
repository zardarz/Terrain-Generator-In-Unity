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
        islandCurve.AddKey(0,1);
        islandCurve.AddKey(1,1);

        int amountOfKeys = Random.Range(1,10);
        float keyJump = 1/amountOfKeys;

        for(float i = keyJump; i < amountOfKeys * keyJump; i += keyJump) {
            float keyHeight = Random.Range(90, 100) / 100f;
            Debug.Log(keyHeight);

            islandCurve.AddKey(i, keyHeight);
        }
    }
    public float getWave(float x) {
        return islandCurve.Evaluate(x);
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

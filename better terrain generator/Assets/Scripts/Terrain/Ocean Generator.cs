using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OceanGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cameraGO;

    private float cameraSize;

    private Vector2 cameraPos;

    private Tilemap map;

    [SerializeField] private Tile oceanTile;

    [SerializeField] private int chunckSize;

    [Range(0,5)]
    [SerializeField] private int renderScale;

    void Update()
    {
        map = gameObject.GetComponent<Tilemap>();
        cameraSize = cameraGO.GetComponent<Camera>().orthographicSize;
        cameraPos = (Vector2) cameraGO.transform.position;
        makeNewChunks();
    }

    private void makeNewChunks() {
        Vector2Int[] chunks = getChunksInSquare(cameraPos, chunckSize, cameraSize);

        for(int i = 0; i < chunks.Length; i++) {
            map.SetTile((Vector3Int)chunks[i], oceanTile);
        }
    }

    private Vector2Int[] getChunksInSquare(Vector2 pos, int chunkSize, float cameraSize) {
        Vector2Int roundedPos = new(roundNum(pos.x, chunkSize), roundNum(pos.y, chunkSize));
        List<Vector2Int> chunks = new List<Vector2Int>();

        float sideSize = cameraSize * renderScale;

        Vector2Int startPos = new(roundedPos.x - roundNum(sideSize/2, chunkSize)-2, roundedPos.y - roundNum(sideSize/2, chunkSize)-2);

        for(int x = 0; x < roundNum(sideSize, chunkSize)+2; x++) {
            for(int y = 0; y < roundNum(sideSize, chunkSize)+2; y++) {
                Vector2Int newPos = new(startPos.x + x, startPos.y + y);
                chunks.Add(newPos);
            }
        }

        return chunks.ToArray();
    }

    private int roundNum(float numToRound, float numToRoundTo) {
        return (int) Math.Round(numToRound/numToRoundTo);
    }
}
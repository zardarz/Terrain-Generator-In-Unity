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

    private Dictionary<Vector2Int, bool> chunks;

    void Update()
    {
        map = gameObject.GetComponent<Tilemap>();
        cameraSize = cameraGO.GetComponent<Camera>().orthographicSize;
        cameraPos = (Vector2) cameraGO.transform.position;
        makeNewChunks();
    }

    private void makeNewChunks() {
        Vector2Int unmadeChunks = getUnmadeChunks();

        Debug.Log(unmadeChunks);

        map.SetTile((Vector3Int) unmadeChunks, oceanTile);
    }

    private Vector2Int getUnmadeChunks() {
        List<Vector2Int> unmadeChunks = new List<Vector2Int>();
        Vector2Int chunksInSquare = getChunksInSquare(cameraPos, chunckSize);

        return chunksInSquare;
    }

    private Vector2Int getChunksInSquare(Vector2 pos, int squareSize) {
        Vector2Int roundedPos = new((int) Math.Round(pos.x/squareSize),(int) Math.Round(pos.y/squareSize));
        Vector2Int chunk = roundedPos;

        return roundedPos;
    }
}
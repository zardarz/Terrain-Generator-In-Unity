using UnityEngine;

public class OceanGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cameraGO;

    private float cameraSize;

    private Vector2 cameraPos;

    

    void Update()
    {
        cameraSize = cameraGO.GetComponent<Camera>().orthographicSize;
        cameraPos = (Vector2) cameraGO.transform.position;
    }
}
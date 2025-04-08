using UnityEngine;

public class CameraPaning : MonoBehaviour
{
    private Camera cam;

    private bool isPanning = false;
    private Vector3 lastMouseWorldPosition;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 20f;

    [Header("Panning")]
    public float panSpeed = 1f;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        HandleZoom();
        HandlePanning();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    private void HandlePanning()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPanning = true;
            lastMouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPanning = false;
        }

        if (isPanning)
        {
            Vector3 currentMouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 delta = lastMouseWorldPosition - currentMouseWorldPos;
            transform.position += delta * panSpeed;
        }
    }
}

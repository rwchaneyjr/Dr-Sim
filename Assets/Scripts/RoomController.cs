using UnityEngine;

public class RoomController : MonoBehaviour
{
    public int row;
    public int col;
    public GameObject target;
    public Patient patient;
    public CubeGridSpawner gridSpawner;

    private Renderer roomRenderer;

    public void Initialize(CubeGridSpawner spawner, int roomRow, int roomCol)
    {
        gridSpawner = spawner;
        row = roomRow;
        col = roomCol;
        roomRenderer = GetComponentInChildren<Renderer>();
    }

    public void ShowTarget()
    {
        if (target != null)
            target.SetActive(true);
    }

    public void HideTarget()
    {
        if (target != null)
            target.SetActive(false);
    }

    public Vector3 GetCenter()
    {
        if (roomRenderer == null)
            roomRenderer = GetComponentInChildren<Renderer>();

        return roomRenderer != null ? roomRenderer.bounds.center : transform.position;
    }

    public float GetFloorY()
    {
        if (roomRenderer == null)
            roomRenderer = GetComponentInChildren<Renderer>();

        return roomRenderer != null ? roomRenderer.bounds.min.y : transform.position.y;
    }
}

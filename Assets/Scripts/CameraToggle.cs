using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    [Header("Drag your 2 cameras here")]
    public Camera overheadCamera;
    public Camera closeUpCamera;

    private bool usingOverhead = true;

    void Start()
    {
        // Safety check
        if (overheadCamera == null || closeUpCamera == null)
        {
            Debug.LogError("Please assign both cameras in the Inspector.");
            return;
        }

        // Start with overhead camera on
        overheadCamera.enabled = true;
        closeUpCamera.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            usingOverhead = !usingOverhead;

            overheadCamera.enabled = usingOverhead;
            closeUpCamera.enabled = !usingOverhead;
        }
    }
}
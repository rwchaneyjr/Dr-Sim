using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private const string CameraOffsetObjectName = "CameraOffsetcube";

    [Header("Start Camera")]
    public Vector3 startOffset = new Vector3(0f, 7f, -8f);
    public float startMoveSpeed = 1.5f;
    public float startDuration = 3f;

    [Header("Normal Camera")]
    public Vector3 normalOffset = new Vector3(0f, 2.75f, -1f);
    public float normalMoveSpeed = 2.5f;
    public float turnSpeed = 5f;

    [Tooltip("Assign the cube/empty object the camera should follow.")]
    public Transform target;
    float timer = 0f;

    void LateUpdate()
    {
        if (!TryResolveTarget())
        {
            return;
        }

        timer += Time.deltaTime;

        Vector3 activeOffset;
        float activeSpeed;

        if (timer < startDuration)
        {
            activeOffset = startOffset;
            activeSpeed = startMoveSpeed;
        }
        else
        {
            activeOffset = normalOffset;
            activeSpeed = normalMoveSpeed;
        }

        Vector3 wantedPosition =
            target.position + activeOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            wantedPosition,
            activeSpeed * Time.deltaTime
        );

        Vector3 lookDirection = target.position - transform.position;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion wantedRotation = Quaternion.LookRotation(lookDirection);
            if (timer >= startDuration)
            {
                wantedRotation *= Quaternion.Euler(-20f, 0f, 0f);
            }

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                wantedRotation,
                turnSpeed * Time.deltaTime
            );
        }
    }

    bool TryResolveTarget()
    {
        if (target != null)
        {
            return true;
        }

        GameObject cameraOffset = GameObject.Find(CameraOffsetObjectName);
        if (cameraOffset != null)
        {
            target = cameraOffset.transform;
            return true;
        }

        Patient patient = FindObjectOfType<Patient>();
        if (patient != null)
        {
            target = patient.transform;
            return true;
        }

        return false;
    }
}
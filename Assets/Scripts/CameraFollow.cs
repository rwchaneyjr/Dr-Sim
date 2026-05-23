using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Start Camera")]
    public Vector3 startOffset = new Vector3(0f, 7f, -8f);
    public float startMoveSpeed = 1.5f;
    public float startDuration = 3f;

    [Header("Normal Camera")]
    public Vector3 normalOffset = new Vector3(0f, 2.75f, -1f);
    public float normalMoveSpeed = 2.5f;
    public float turnSpeed = 5f;

    public Transform target;
    float timer = 0f;
    Renderer[] targetRenderers;
    bool[] targetRendererStartStates;
    Transform renderersTarget;
    bool hasAppliedPatientVisibility;
    bool currentPatientVisibility;

    void LateUpdate()
    {
        if (target == null)
        {
            Patient patient = FindObjectOfType<Patient>();

            if (patient != null)
            {
                target = patient.transform;
                CacheTargetRenderers();
                Debug.Log("CAMERA FOUND PATIENT");
            }
            else
            {
                return;
            }
        }

        timer += Time.deltaTime;
        CacheTargetRenderers();

        Vector3 activeOffset;
        float activeSpeed;
        bool shouldShowPatient;

        if (timer < startDuration)
        {
            activeOffset = startOffset;
            activeSpeed = startMoveSpeed;
            shouldShowPatient = false;
        }
        else
        {
            activeOffset = normalOffset;
            activeSpeed = normalMoveSpeed;
            shouldShowPatient = true;
        }

        SetPatientVisible(shouldShowPatient);

        Vector3 wantedPosition =
            target.position + activeOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            wantedPosition,
            activeSpeed * Time.deltaTime
        );

        Quaternion wantedRotation =
            Quaternion.LookRotation(target.position - transform.position);
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

    void CacheTargetRenderers()
    {
        if (target == null || renderersTarget == target)
            return;

        renderersTarget = target;
        targetRenderers = target.GetComponentsInChildren<Renderer>(true);
        targetRendererStartStates = new bool[targetRenderers.Length];

        for (int i = 0; i < targetRenderers.Length; i++)
        {
            targetRendererStartStates[i] = targetRenderers[i].enabled;
        }

        hasAppliedPatientVisibility = false;
    }

    void SetPatientVisible(bool visible)
    {
        if (targetRenderers == null || targetRendererStartStates == null)
            return;

        if (hasAppliedPatientVisibility && currentPatientVisibility == visible)
            return;

        for (int i = 0; i < targetRenderers.Length; i++)
        {
            if (targetRenderers[i] != null)
                targetRenderers[i].enabled = visible && targetRendererStartStates[i];
        }

        currentPatientVisibility = visible;
        hasAppliedPatientVisibility = true;
    }

    void OnDisable()
    {
        SetPatientVisible(true);
    }
}
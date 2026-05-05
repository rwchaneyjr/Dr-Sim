using UnityEngine;

public class DoctorHealTile : MonoBehaviour
{
    public GameObject doctorCanvas;   // drag your Canvas here
    public DoctorTool doctorTool;     // drag your DoctorTool object here
    private Patient activePatient;

    private void Awake()
    {
        if (doctorTool == null)
        {
            doctorTool = FindObjectOfType<DoctorTool>();
        }

        if (doctorCanvas == null && doctorTool != null)
        {
            doctorCanvas = doctorTool.doctorCanvas;
        }
    }

    private void Start()
    {
        SetCanvasVisible(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Patient patient = FindTriggeredPatient(other);

        if (patient != null)
        {
            Debug.Log("PATIENT FOUND: " + other.name);

            activePatient = patient;

            if (doctorTool != null)
            {
                doctorTool.SelectPatient(patient);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Patient patient = FindTriggeredPatient(other);

        if (patient != null && (activePatient == null || patient == activePatient))
        {
            activePatient = null;

            if (doctorTool != null)
            {
                doctorTool.ClearPatient(patient);
            }
        }
    }

    private Patient FindTriggeredPatient(Collider other)
    {
        Patient patient = other.GetComponent<Patient>();

        if (patient == null)
        {
            patient = other.GetComponentInParent<Patient>();
        }

        if (patient == null)
        {
            patient = other.GetComponentInChildren<Patient>();
        }

        if (patient == null)
        {
            patient = GetComponent<Patient>();
        }

        if (patient == null)
        {
            patient = GetComponentInParent<Patient>();
        }

        if (patient == null)
        {
            patient = GetComponentInChildren<Patient>();
        }

        return patient;
    }

    private void SetCanvasVisible(bool visible)
    {
        if (doctorCanvas != null)
        {
            doctorCanvas.SetActive(visible);
        }
    }
}

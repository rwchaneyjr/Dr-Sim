using UnityEngine;

public class DoctorHealTile : MonoBehaviour
{
    private DoctorTool doctorTool;

    void Start()
    {
        doctorTool = FindObjectOfType<DoctorTool>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTER TRIGGER: " + other.name);

        Patient patient = other.GetComponent<Patient>();

        if (patient == null)
        {
            patient = other.GetComponentInParent<Patient>();
        }

        if (patient != null && doctorTool != null)
        {
            Debug.Log("PATIENT FOUND");
            doctorTool.SelectPatient(patient);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Patient patient = other.GetComponent<Patient>();

        if (patient == null)
        {
            patient = other.GetComponentInParent<Patient>();
        }

        if (patient != null && doctorTool != null)
        {
            doctorTool.ClearUI();
        }
    }
}
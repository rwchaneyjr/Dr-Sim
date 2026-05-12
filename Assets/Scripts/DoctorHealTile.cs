using UnityEngine;

public class DoctorHealTile : MonoBehaviour
{
    public GameObject doctorCanvas;   // drag your Canvas here
    public DoctorTool doctorTool;     // drag your DoctorTool object here

    private void Start()
    {
        if (doctorTool == null)
            doctorTool = FindObjectOfType<DoctorTool>();

        if (doctorCanvas != null)
        {
            doctorCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Patient patient = other.GetComponentInParent<Patient>();

        if (patient != null)
        {
            Debug.Log("PATIENT FOUND: " + other.name);

            if (doctorCanvas != null)
            {
                doctorCanvas.SetActive(true);
            }

            if (doctorTool != null)
            {
                doctorTool.SelectPatient(patient);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Patient patient = other.GetComponentInParent<Patient>();

        if (patient != null)
        {
            if (doctorCanvas != null)
            {
                doctorCanvas.SetActive(false);
            }

            if (doctorTool != null)
            {
                doctorTool.ClearUI();
            }
        }
    }
}
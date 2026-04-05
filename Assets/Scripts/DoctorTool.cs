using UnityEngine;
using TMPro;

public class DoctorTool : MonoBehaviour
{
    public TMP_Text resultText;

    private Patient selectedPatient;

    // Called when you click a cube
    public void SelectPatient(Patient patient)
    {
        selectedPatient = patient;

        resultText.text = "Selected: " + patient.currentCondition.ToString();

        Debug.Log("Patient selected: " + patient.currentCondition);
    }

    public void PrescribeFlu()
    {
        if (selectedPatient == null)
        {
            resultText.text = "Select a patient first!";
            return;
        }

        if (selectedPatient.currentCondition == Patient.Condition.Flu)
        {
            selectedPatient.Heal(30f);
            resultText.text = "✅ Flu treated";
        }
        else
        {
            selectedPatient.AdverseReaction();
            resultText.text = "❌ Wrong medicine";
        }
    }

    public void PrescribeArm()
    {
        if (selectedPatient == null)
        {
            resultText.text = "Select a patient first!";
            return;
        }

        if (selectedPatient.currentCondition == Patient.Condition.BrokenArm)
        {
            selectedPatient.Heal(30f);
            resultText.text = "✅ Arm treated";
        }
        else
        {
            selectedPatient.AdverseReaction();
            resultText.text = "❌ Wrong medicine";
        }
    }
}
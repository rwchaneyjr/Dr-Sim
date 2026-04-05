using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DoctorTool : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text diagnosisText;
    public TMP_Text healthText;
    public TMP_Text resultText;
    public GameObject panel;

    private Patient selectedPatient;

    // Called when cube is clicked
    public void SelectPatient(Patient patient)
    {
        selectedPatient = patient;

        if (panel != null)
        {
            panel.SetActive(true);
        }

        UpdateUI();

        Debug.Log("Selected: " + patient.currentCondition);
    }

    void Update()
    {
        if (selectedPatient != null)
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (diagnosisText != null)
        {
            diagnosisText.text = "Diagnosis: " + selectedPatient.currentCondition.ToString();
        }

        if (healthText != null)
        {
            healthText.text = "Health: " + selectedPatient.health.ToString("F0");
        }
    }

    public void TreatFlu()
    {
        if (selectedPatient == null)
        {
            if (resultText != null)
            {
                resultText.text = "Select a patient first!";
            }
            return;
        }

        if (selectedPatient.currentCondition == Patient.Condition.Flu)
        {
            selectedPatient.Heal(30f);

            if (resultText != null)
            {
                resultText.text = "Flu treated";
            }
        }
        else
        {
            selectedPatient.AdverseReaction();

            if (resultText != null)
            {
                resultText.text = "Wrong medicine";
            }
        }
    }

    public void TreatArm()
    {
        if (selectedPatient == null)
        {
            if (resultText != null)
            {
                resultText.text = "Select a patient first!";
            }
            return;
        }

        if (selectedPatient.currentCondition == Patient.Condition.BrokenArm)
        {
            selectedPatient.Heal(30f);

            if (resultText != null)
            {
                resultText.text = "Arm treated";
            }
        }
        else
        {
            selectedPatient.AdverseReaction();

            if (resultText != null)
            {
                resultText.text = "Wrong medicine";
            }
        }
    }

    public void TreatHeart()
    {
        if (selectedPatient == null)
        {
            if (resultText != null)
            {
                resultText.text = "Select a patient first!";
            }
            return;
        }

        if (selectedPatient.currentCondition == Patient.Condition.HeartPalpitation)
        {
            selectedPatient.Heal(30f);

            if (resultText != null)
            {
                resultText.text = "Heart treated";
            }
        }
        else
        {
            selectedPatient.AdverseReaction();

            if (resultText != null)
            {
                resultText.text = "Wrong medicine";
            }
        }
    }
}
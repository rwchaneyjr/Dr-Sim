using UnityEngine;
using TMPro;
using System.Collections;

public class DoctorTool : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text diagnosisText;
    public TMP_Text healthText;
    public TMP_Text resultText;

    [Header("Treatment UI")]
    public GameObject treatmentPanel;

    // STATIC result message shared between scripts
    public static string currentResultMessage = "";

    private Patient selectedPatient;
    private Coroutine diagnosisCoroutine;

    void Start()
    {
        if (treatmentPanel != null)
        {
            treatmentPanel.SetActive(false);
        }

        diagnosisText.text = "";
        healthText.text = "";
        resultText.text = "";
        currentResultMessage = "";
    }

    public void SelectPatient(Patient patient)
    {
        selectedPatient = patient;

        if (treatmentPanel != null)
        {
            treatmentPanel.SetActive(true);
        }

        if (diagnosisCoroutine != null)
        {
            StopCoroutine(diagnosisCoroutine);
            diagnosisCoroutine = null;
        }

        diagnosisText.text = "Symptoms: " + selectedPatient.GetSymptoms();
        healthText.text = "Health: " + selectedPatient.health.ToString("F0");

        currentResultMessage = "";
        resultText.text = currentResultMessage;

        diagnosisCoroutine = StartCoroutine(ShowDiagnosisAfterDelay(3f));
    }

    IEnumerator ShowDiagnosisAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (selectedPatient != null)
        {
            diagnosisText.text = "Diagnosis: " + selectedPatient.currentCondition.ToString();
        }
    }

    void Update()
    {
        if (selectedPatient != null)
        {
            healthText.text = "Health: " + selectedPatient.health.ToString("F0");
        }

        // Always show the shared static message
        if (resultText != null)
        {
            resultText.text = currentResultMessage;
        }
    }

    public void ClearUI()
    {
        selectedPatient = null;

        if (diagnosisCoroutine != null)
        {
            StopCoroutine(diagnosisCoroutine);
            diagnosisCoroutine = null;
        }

        diagnosisText.text = "";
        healthText.text = "";
        currentResultMessage = "";
        resultText.text = "";

        if (treatmentPanel != null)
        {
            treatmentPanel.SetActive(false);
        }
    }

    public void Cure(Patient.Condition cureType)
    {
        if (selectedPatient == null)
        {
            currentResultMessage = "Select a patient first!";
            return;
        }

        if (selectedPatient.currentCondition == cureType)
        {
            selectedPatient.Heal(100f);
            currentResultMessage = "Correct cure!";
        }
        else
        {
            selectedPatient.AdverseReaction();
            currentResultMessage = "Wrong cure!";
        }
    }

    public void CureDehydration() { Cure(Patient.Condition.Dehydration); }
    public void CureInfection() { Cure(Patient.Condition.Infection); }
    public void CureFever() { Cure(Patient.Condition.Fever); }
    public void CureBurn() { Cure(Patient.Condition.Burn); }
    public void CureSprain() { Cure(Patient.Condition.Sprain); }
    public void CureHeartPalpitation() { Cure(Patient.Condition.HeartPalpitation); }
    public void CureHeadache() { Cure(Patient.Condition.Headache); }
    public void CureFoodPoisoning() { Cure(Patient.Condition.FoodPoisoning); }
    public void CureCold() { Cure(Patient.Condition.Cold); }
    public void CureBrokenArm() { Cure(Patient.Condition.BrokenArm); }
    public void CureFlu() { Cure(Patient.Condition.Flu); }
}
using UnityEngine;
using TMPro;
using System.Collections;

public class DoctorTool : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text diagnosisText;
    public TMP_Text healthText;
    public TMP_Text resultText;

    private Patient selectedPatient;
    private Coroutine diagnosisCoroutine;

    public void SelectPatient(Patient patient)
    {
        selectedPatient = patient;

        if (diagnosisCoroutine != null)
        {
            StopCoroutine(diagnosisCoroutine);
        }

        // 🔥 SHOW SYMPTOMS FIRST
        diagnosisText.text = "Symptoms: " + selectedPatient.GetSymptoms();
        healthText.text = "Health: " + selectedPatient.health.ToString("F0");

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
    }

    public void Cure(Patient.Condition cureType)
    {
        if (selectedPatient == null)
        {
            resultText.text = "Select a patient first!";
            return;
        }

        if (selectedPatient.currentCondition == cureType)
        {
            selectedPatient.Heal(30f);
            resultText.text = "Correct cure!";
        }
        else
        {
            selectedPatient.AdverseReaction();
            resultText.text = "Wrong cure!";
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
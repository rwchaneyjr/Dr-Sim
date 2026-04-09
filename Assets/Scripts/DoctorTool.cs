using UnityEngine;
using TMPro;
using System.Collections;

public class DoctorTool : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text diagnosisText;
    public TMP_Text healthText;
    public TMP_Text resultText;

    [Header("Start Instructions")]
    public GameObject doctorCanvas;
    public TMP_Text instructionText;
    public float instructionDuration = 4f;

    private Patient selectedPatient;
    private Coroutine diagnosisCoroutine;

    void Start()
    {
        if (doctorCanvas != null)
            doctorCanvas.SetActive(true);

        if (instructionText != null)
            instructionText.text = "Use W A S D  - C Key to Toggle Camera";

        if (diagnosisText != null)
            diagnosisText.text = "";

        if (healthText != null)
            healthText.text = "";

        if (resultText != null)
        {
            resultText.text = "";
            resultText.color = Color.white;
        }

        StartCoroutine(HideInstruction());
    }

    IEnumerator HideInstruction()
    {
        yield return new WaitForSeconds(instructionDuration);

        if (instructionText != null)
            instructionText.text = "";
    }

    public void SelectPatient(Patient patient)
    {
        selectedPatient = patient;

        if (diagnosisCoroutine != null)
        {
            StopCoroutine(diagnosisCoroutine);
            diagnosisCoroutine = null;
        }

        if (diagnosisText != null)
            diagnosisText.text = "Symptoms: " + selectedPatient.GetSymptoms();

        if (healthText != null)
            healthText.text = "Health: " + selectedPatient.health.ToString("F0");

        if (resultText != null)
        {
            resultText.text = "";
            resultText.color = Color.white;
        }

        diagnosisCoroutine = StartCoroutine(ShowDiagnosisAfterDelay(3f));
    }

    IEnumerator ShowDiagnosisAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (selectedPatient != null && diagnosisText != null)
        {
            diagnosisText.text = "Diagnosis: " + selectedPatient.currentCondition.ToString();
        }
    }

    void Update()
    {
        if (selectedPatient != null && healthText != null)
        {
            healthText.text = "Health: " + selectedPatient.health.ToString("F0");
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

        if (diagnosisText != null) diagnosisText.text = "";
        if (healthText != null) healthText.text = "";
        if (resultText != null)
        {
            resultText.text = "";
            resultText.color = Color.white;
        }
    }

    public void Cure(Patient.Condition cureType)
    {
        if (selectedPatient == null)
        {
            if (resultText != null)
            {
                resultText.text = "Select a cure!";
                resultText.color = Color.white;
            }
            return;
        }

        Renderer patientRenderer = selectedPatient.GetComponent<Renderer>();

        if (selectedPatient.currentCondition == cureType)
        {
            selectedPatient.Heal(30f);

            if (resultText != null)
            {
                resultText.text = "Correct cure!";
                resultText.color = Color.green;
            }

            if (patientRenderer != null)
            {
                patientRenderer.material.color = Color.green;
            }
        }
        else
        {
            selectedPatient.AdverseReaction();

            if (resultText != null)
            {
                resultText.text = "Wrong cure!";
                resultText.color = Color.red;
            }

            if (patientRenderer != null)
            {
                patientRenderer.material.color = Color.black;
            }
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
    public void CureToothAche() { Cure(Patient.Condition.ToothAche); }
    public void CureFlu() { Cure(Patient.Condition.Flu); }
}
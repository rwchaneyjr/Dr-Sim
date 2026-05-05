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

    [Header("Result Typewriter")]
    public float resultCharacterDelay = 0.04f;

    private Patient selectedPatient;
    private Coroutine diagnosisCoroutine;
    private Coroutine resultCoroutine;

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
            ClearResultText();
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

        if (doctorCanvas != null)
            doctorCanvas.SetActive(true);

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
            ClearResultText();
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
            ClearResultText();
            resultText.color = Color.white;
        }

        if (doctorCanvas != null)
            doctorCanvas.SetActive(false);
    }

    public void ClearPatient(Patient patient)
    {
        if (selectedPatient == patient)
        {
            ClearUI();
        }
    }

    public void Cure(Patient.Condition cureType)
    {
        if (selectedPatient == null)
        {
            if (resultText != null)
            {
                ShowResult("Select a patient first!", Color.white);
            }
            return;
        }

        Renderer patientRenderer = selectedPatient.GetComponentInChildren<Renderer>();

        if (selectedPatient.currentCondition == cureType)
        {
            selectedPatient.Heal(30f);

            if (resultText != null)
            {
                ShowResult("Correct cure!", Color.green);
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
                ShowResult("Wrong cure!", Color.red);
            }

            if (patientRenderer != null)
            {
                patientRenderer.material.color = Color.black;
            }
        }
    }

    private void ShowResult(string message, Color color)
    {
        if (resultText == null)
        {
            return;
        }

        if (resultCoroutine != null)
        {
            StopCoroutine(resultCoroutine);
        }

        resultText.color = color;
        resultCoroutine = StartCoroutine(TypeResult(message));
    }

    private IEnumerator TypeResult(string message)
    {
        resultText.text = "";

        foreach (char letter in message)
        {
            resultText.text += letter;
            yield return new WaitForSeconds(resultCharacterDelay);
        }

        resultCoroutine = null;
    }

    private void ClearResultText()
    {
        if (resultCoroutine != null)
        {
            StopCoroutine(resultCoroutine);
            resultCoroutine = null;
        }

        if (resultText != null)
        {
            resultText.text = "";
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
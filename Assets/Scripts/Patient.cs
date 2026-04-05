using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Patient : MonoBehaviour
{
    public enum Condition
    {
        Healthy,
        Flu,
        BrokenArm,
        HeartPalpitation
    }

    public Condition currentCondition;

    [Header("Stats")]
    public float health = 100f;

    [Header("Visual References")]
    public Renderer bodyRenderer;
    public GameObject redNose;
    public GameObject cast;
    public GameObject heartIcon;
    public Slider healthBar;

    [Header("UI")]
    public TMP_Text healthText;
    public TMP_Text diagnosisText;
    public TMP_Text resultText;

    void Start()
    {
        UpdateVisuals();
        UpdateHealthUI();
        UpdateDiagnosisUI();
    }

    void Update()
    {
        void OnMouseDown()
        {
            Debug.Log("CLICKED PATIENT");   // ← test
            Diagnose();
        }
        UpdateHealthUI();
    }

    void OnMouseDown()
    {
        Diagnose();
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.value = health / 100f;

        if (healthText != null)
            healthText.text = "Health: " + health.ToString("F0");
    }

    void UpdateDiagnosisUI()
    {
        if (diagnosisText != null)
            diagnosisText.text = "Diagnosis: " + currentCondition.ToString();
    }

    public void Diagnose()
    {
        UpdateDiagnosisUI();
        Debug.Log("Diagnosis: " + currentCondition);
    }

    public void SetCondition(Condition newCondition)
    {
        currentCondition = newCondition;
        UpdateVisuals();
        UpdateDiagnosisUI();
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > 100f)
            health = 100f;

        if (resultText != null)
            resultText.text = "✅ Correct Treatment";

        StartCoroutine(FlashGreen());

        if (health >= 100f)
        {
            currentCondition = Condition.Healthy;
            UpdateVisuals();
            UpdateDiagnosisUI();
        }
    }

    public void AdverseReaction()
    {
        health -= 30f;
        if (health < 0f)
            health = 0f;

        if (resultText != null)
            resultText.text = "❌ Adverse Reaction!";

        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        if (bodyRenderer == null)
            yield break;

        Color originalColor = bodyRenderer.material.color;

        for (int i = 0; i < 3; i++)
        {
            bodyRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.2f);

            bodyRenderer.material.color = originalColor;
            yield return new WaitForSeconds(0.2f);
        }

        UpdateVisuals();
    }

    IEnumerator FlashGreen()
    {
        if (bodyRenderer == null)
            yield break;

        Color originalColor = bodyRenderer.material.color;

        for (int i = 0; i < 3; i++)
        {
            bodyRenderer.material.color = Color.green;
            yield return new WaitForSeconds(0.2f);

            bodyRenderer.material.color = originalColor;
            yield return new WaitForSeconds(0.2f);
        }

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        if (redNose != null) redNose.SetActive(false);
        if (cast != null) cast.SetActive(false);
        if (heartIcon != null) heartIcon.SetActive(false);

        if (bodyRenderer == null)
            return;

        if (currentCondition == Condition.Flu)
        {
            if (redNose != null) redNose.SetActive(true);
            bodyRenderer.material.color = Color.red;
        }
        else if (currentCondition == Condition.BrokenArm)
        {
            if (cast != null) cast.SetActive(true);
            bodyRenderer.material.color = Color.yellow;
        }
        else if (currentCondition == Condition.HeartPalpitation)
        {
            if (heartIcon != null) heartIcon.SetActive(true);
            bodyRenderer.material.color = new Color(1f, 0.5f, 0.5f);
        }
        else
        {
            bodyRenderer.material.color = Color.white;
        }
    }
}
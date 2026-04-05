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

    [Header("Condition")]
    public Condition currentCondition = Condition.Healthy;

    [Header("Stats")]
    public float health = 100f;

    [Header("Visual")]
    public Renderer bodyRenderer;
    public GameObject redNose;
    public GameObject cast;
    public GameObject heartIcon;

    private DoctorTool doctorTool;

    void Awake()
    {
        if (bodyRenderer == null)
        {
            bodyRenderer = GetComponent<Renderer>();
        }

        // Automatically find DoctorTool in the scene
        doctorTool = FindObjectOfType<DoctorTool>();
    }

    void Start()
    {
        UpdateVisuals();
    }

    void OnMouseDown()
    {
        Debug.Log("CLICKED PATIENT: " + gameObject.name);
        Debug.Log("Diagnosis: " + currentCondition);

        if (doctorTool != null)
        {
            doctorTool.SelectPatient(this);
        }
        else
        {
            Debug.LogWarning("No DoctorTool found in scene.");
        }
    }

    public void SetCondition(Condition newCondition)
    {
        currentCondition = newCondition;
        UpdateVisuals();
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > 100f)
        {
            health = 100f;
        }

        if (health >= 100f)
        {
            currentCondition = Condition.Healthy;
            UpdateVisuals();
        }
    }

    public void AdverseReaction()
    {
        health -= 30f;
        if (health < 0f)
        {
            health = 0f;
        }
    }

    public void UpdateVisuals()
    {
        if (redNose != null) redNose.SetActive(false);
        if (cast != null) cast.SetActive(false);
        if (heartIcon != null) heartIcon.SetActive(false);

        if (bodyRenderer == null)
        {
            Debug.LogError("No Renderer on " + gameObject.name);
            return;
        }

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
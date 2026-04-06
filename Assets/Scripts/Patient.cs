using UnityEngine;

public class Patient : MonoBehaviour
{
    public enum Condition
    {
        Flu,
        BrokenArm,
        HeartPalpitation,
        Fever,
        Cold,
        Headache,
        Infection,
        Burn,
        Fracture,
        Sprain,
        Dehydration,
        FoodPoisoning
    }

    [Header("Patient State")]
    public Condition currentCondition;
    public float health = 100f;

    [Header("Colors")]
    public Color fluColor = new Color(0.4f, 1f, 0.4f);          // green
    public Color brokenArmColor = Color.yellow;                 // yellow
    public Color heartColor = Color.red;                        // red
    public Color feverColor = new Color(1f, 0.5f, 0f);          // orange
    public Color coldColor = Color.cyan;                        // cyan
    public Color headacheColor = new Color(0.7f, 0.3f, 1f);     // purple
    public Color infectionColor = new Color(0.5f, 1f, 0.5f);    // pale green
    public Color burnColor = new Color(1f, 0.3f, 0.1f);         // hot orange/red
    public Color fractureColor = new Color(1f, 1f, 0.6f);       // light yellow
    public Color sprainColor = new Color(0.2f, 0.8f, 1f);       // blue-cyan
    public Color dehydrationColor = new Color(0.3f, 0.5f, 1f);  // blue
    public Color foodPoisoningColor = new Color(0.2f, 0.8f, 0.2f);

    private Renderer rend;
    private DoctorTool cachedDoctorTool;

    void Awake()
    {
        rend = GetComponent<Renderer>();

        if (rend == null)
        {
            Debug.LogError("Patient: No Renderer found on " + gameObject.name);
        }
    }

    void Start()
    {
        cachedDoctorTool = FindObjectOfType<DoctorTool>();
        UpdateVisuals();
    }

    public void SetCondition(Condition newCondition)
    {
        currentCondition = newCondition;
        health = 100f;
        UpdateVisuals();
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0f, 100f);

        if (health >= 100f && rend != null)
        {
            rend.material.color = Color.white;
        }
    }

    public void AdverseReaction()
    {
        health -= 25f;
        health = Mathf.Clamp(health, 0f, 100f);

        if (rend != null)
        {
            rend.material.color = Color.black;
        }
    }

    void UpdateVisuals()
    {
        if (rend == null) return;

        switch (currentCondition)
        {
            case Condition.Flu:
                rend.material.color = fluColor;
                break;

            case Condition.BrokenArm:
                rend.material.color = brokenArmColor;
                break;

            case Condition.HeartPalpitation:
                rend.material.color = heartColor;
                break;

            case Condition.Fever:
                rend.material.color = feverColor;
                break;

            case Condition.Cold:
                rend.material.color = coldColor;
                break;

            case Condition.Headache:
                rend.material.color = headacheColor;
                break;

            case Condition.Infection:
                rend.material.color = infectionColor;
                break;

            case Condition.Burn:
                rend.material.color = burnColor;
                break;

            case Condition.Fracture:
                rend.material.color = fractureColor;
                break;

            case Condition.Sprain:
                rend.material.color = sprainColor;
                break;

            case Condition.Dehydration:
                rend.material.color = dehydrationColor;
                break;

            case Condition.FoodPoisoning:
                rend.material.color = foodPoisoningColor;
                break;

            default:
                rend.material.color = Color.gray;
                break;
        }
    }

    void OnMouseDown()
    {
        if (cachedDoctorTool == null)
        {
            cachedDoctorTool = FindObjectOfType<DoctorTool>();
        }

        if (cachedDoctorTool != null)
        {
            cachedDoctorTool.SelectPatient(this);
        }
    }

    public string GetSymptoms()
    {
        switch (currentCondition)
        {
            case Condition.Flu:
                return "Coughing, chills";

            case Condition.BrokenArm:
                return "Pain, cannot move arm";

            case Condition.HeartPalpitation:
                return "Rapid heartbeat";

            case Condition.Fever:
                return "High temperature";

            case Condition.Cold:
                return "Sneezing, runny nose";

            case Condition.Headache:
                return "Head pain";

            case Condition.Infection:
                return "Redness, swelling";

            case Condition.Burn:
                return "Blisters, red skin";

            case Condition.Fracture:
                return "Severe pain, deformity";

            case Condition.Sprain:
                return "Swelling, pain";

            case Condition.Dehydration:
                return "Dry mouth, dizziness";

            case Condition.FoodPoisoning:
                return "Nausea, vomiting";

            default:
                return "Unknown";
        }
    }
}
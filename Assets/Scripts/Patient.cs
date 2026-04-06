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
    public Color fluColor = new Color(0.4f, 1f, 0.4f);
    public Color brokenArmColor = Color.yellow;
    public Color heartColor = Color.red;
    public Color feverColor = new Color(1f, 0.5f, 0f);
    public Color coldColor = Color.cyan;
    public Color headacheColor = new Color(0.7f, 0.3f, 1f);
    public Color infectionColor = new Color(0.5f, 1f, 0.5f);
    public Color burnColor = new Color(1f, 0.3f, 0.1f);
    public Color fractureColor = new Color(1f, 1f, 0.6f);
    public Color sprainColor = new Color(0.2f, 0.8f, 1f);
    public Color dehydrationColor = new Color(0.3f, 0.5f, 1f);
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

        DoctorTool.currentResultMessage = "New patient needs treatment.";

        UpdateVisuals();
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0f, 100f);

        if (health >= 100f)
        {
            health = 100f;

            if (rend != null)
            {
                Color healedColor = Color.white;
                healedColor.a = rend.material.color.a;
                rend.material.color = healedColor;
            }
        }
    }

    public void AdverseReaction()
    {
        health -= 25f;
        health = Mathf.Clamp(health, 0f, 100f);

        if (rend != null)
        {
            Color badColor = Color.black;
            badColor.a = rend.material.color.a;
            rend.material.color = badColor;
        }
    }

    void UpdateVisuals()
    {
        if (rend == null) return;

        Color c;

        switch (currentCondition)
        {
            case Condition.Flu: c = fluColor; break;
            case Condition.BrokenArm: c = brokenArmColor; break;
            case Condition.HeartPalpitation: c = heartColor; break;
            case Condition.Fever: c = feverColor; break;
            case Condition.Cold: c = coldColor; break;
            case Condition.Headache: c = headacheColor; break;
            case Condition.Infection: c = infectionColor; break;
            case Condition.Burn: c = burnColor; break;
            case Condition.Fracture: c = fractureColor; break;
            case Condition.Sprain: c = sprainColor; break;
            case Condition.Dehydration: c = dehydrationColor; break;
            case Condition.FoodPoisoning: c = foodPoisoningColor; break;
            default: c = Color.gray; break;
        }

        c.a = rend.material.color.a;
        rend.material.color = c;
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
            DoctorTool.currentResultMessage = ""; // ✅ FIXED
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
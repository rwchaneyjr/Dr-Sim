using UnityEngine;


public class Patient : MonoBehaviour
{
    public bool isSick = false;
    public Renderer bodyRenderer;

   // public float health = 100f;

    // the rest of your Patient code stays here

public enum Condition
    {
        Dehydration,
        Infection,
        Fever,
        Burn,
        Sprain,
        HeartPalpitation,
        Headache,
        FoodPoisoning,
        Cold,
        BrokenArm,
        Flu,
        ToothAche,
        StomachPain
    }
    private Color normalColor = Color.white;
    private bool hasNormalColor = false;

    void Awake()
    {
        CacheRenderer();
        CaptureNormalColor();
        ApplyConditionColor();
    }

    void Start()
    {
        ApplyConditionColor();
    }

    void OnMouseDown()
    {
        Debug.Log("PATIENT CLICKED");

        DoctorTool tool = FindObjectOfType<DoctorTool>();

        if (tool != null)
        {
            tool.SelectPatient(this);
        }
    }
    public Condition currentCondition;
    public float health = 100f;

    public void Heal(float amount)
    {
        health += amount;
        if (health > 100f)
            health = 100f;
    }
    // OPTIONAL: used by other scripts (keeps errors away)
    public void SetCondition(Condition newCondition)
    {
        currentCondition = newCondition;
        ApplyConditionColor();
    }

    // OPTIONAL: used by Medicine script
    public void AdverseReaction()
    {
        TakeDamage(20f);
        SetPatientColor(new Color(0.2f, 0.2f, 0.2f));
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health < 0f)
            health = 0f;
    }
    public void Recover()
    {
        ResetAppearance();
    }

    public void ResetAppearance()
    {
        SetPatientColor(hasNormalColor ? normalColor : Color.white);
    }

    void ApplyConditionColor()
    {
        SetPatientColor(GetConditionColor(currentCondition));
    }

    void CacheRenderer()
    {
        if (bodyRenderer == null)
            bodyRenderer = GetComponentInChildren<Renderer>();
    }

    void CaptureNormalColor()
    {
        CacheRenderer();

        if (bodyRenderer == null || bodyRenderer.material == null)
            return;

        normalColor = GetMaterialColor(bodyRenderer.material);
        hasNormalColor = true;
    }

    Color GetMaterialColor(Material material)
    {
        if (material.HasProperty("_BaseColor"))
            return material.GetColor("_BaseColor");

        if (material.HasProperty("_Color"))
            return material.GetColor("_Color");

        return Color.white;
    }

    void SetPatientColor(Color color)
    {
        CacheRenderer();

        if (bodyRenderer == null || bodyRenderer.material == null)
            return;

        Material material = bodyRenderer.material;

        if (material.HasProperty("_BaseColor"))
            material.SetColor("_BaseColor", color);

        if (material.HasProperty("_Color"))
            material.SetColor("_Color", color);

    }

    Color GetConditionColor(Condition condition)
    {
        switch (condition)
        {
            case Condition.Dehydration: return new Color(0.85f, 0.65f, 0.25f);
            case Condition.Infection: return new Color(0.15f, 0.95f, 0.15f);
            case Condition.Fever: return new Color(1f, 0.45f, 0.1f);
            case Condition.Burn: return new Color(1f, 0.15f, 0.15f);
            case Condition.Sprain: return new Color(0.95f, 0.55f, 0.15f);
            case Condition.HeartPalpitation: return new Color(1f, 0.1f, 0.45f);
            case Condition.Headache: return new Color(0.55f, 0.55f, 0.55f);
            case Condition.FoodPoisoning: return new Color(0.45f, 0.85f, 0.2f);
            case Condition.Cold: return new Color(0.35f, 0.7f, 1f);
            case Condition.BrokenArm: return new Color(0.75f, 0.75f, 1f);
            case Condition.Flu: return new Color(0.55f, 1f, 0.55f);
            case Condition.ToothAche: return new Color(1f, 0.8f, 0.8f);
            case Condition.StomachPain: return new Color(0.65f, 0.25f, 0.95f);
            default: return hasNormalColor ? normalColor : Color.white;
        }
    }
    public string GetSymptoms()
    {
        switch (currentCondition)
        {
            case Condition.Dehydration: return "Thirst, dizziness";
            case Condition.Infection: return "Weakness, chills";
            case Condition.Fever: return "Hot skin, sweating";
            case Condition.Burn: return "Red skin, pain";
            case Condition.Sprain: return "Swelling, pain";
            case Condition.HeartPalpitation: return "Fast heartbeat";
            case Condition.Headache: return "Head pain";
            case Condition.FoodPoisoning: return "Nausea, vomiting";
            case Condition.Cold: return "Sneezing, cough";
            case Condition.BrokenArm: return "Arm pain, cannot move";
            case Condition.Flu: return "Body aches, fever";
            case Condition.ToothAche: return "Tooth pain, jaw soreness";
            case Condition.StomachPain: return "Stomach pain, cramping";
            default: return "Unknown symptoms";
        }
    }
}
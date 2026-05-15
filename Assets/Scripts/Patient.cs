using UnityEngine;


public class Patient : MonoBehaviour
{
    public bool isSick = false;

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
        ToothAche// 👈 ADD THIS
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
    }
    // OPTIONAL: used by other scripts (keeps errors away)
    public void SetCondition(Condition newCondition)
    {
        currentCondition = newCondition;
    }

    // OPTIONAL: used by Medicine script
    public void AdverseReaction()
    {
        TakeDamage(20f);
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
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
            default: return "Unknown symptoms";
        }
    }
}
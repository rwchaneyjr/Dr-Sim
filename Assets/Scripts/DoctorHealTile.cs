using UnityEngine;

public class DoctorHealTile : MonoBehaviour
{
    [Header("If true, touched tiles turn green. If false, they turn blue.")]
    public bool turnTileGreen = true;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Doctor touched: " + other.gameObject.name);

        HealthTile tile = other.GetComponent<HealthTile>();

        if (tile != null)
        {
            if (tile.isSick)
            {
                if (turnTileGreen)
                {
                    tile.HealTile();
                }
                else
                {
                    tile.MakeRecoveryBlue();
                }
            }
        }
    }
}
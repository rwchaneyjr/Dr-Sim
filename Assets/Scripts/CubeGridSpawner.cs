using UnityEngine;

public class CubeGridSpawner : MonoBehaviour
{
    [Header("Drag your cube prefab here")]
    public GameObject cubePrefab;

    [Header("Grid Size")]
    public int width = 10;
    public int height = 10;

    [Header("Spacing Multiplier (1 = touching, >1 = gap)")]
    public float spacingMultiplier = 1.1f;

    [Header("Start Position Offset")]
    public Vector3 startPosition = Vector3.zero;

    private Vector3 cubeSize;

    void Start()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("No cube prefab assigned.");
            return;
        }

        Renderer rend = cubePrefab.GetComponent<Renderer>();

        if (rend == null)
        {
            Debug.LogError("Cube prefab has no Renderer.");
            return;
        }

        cubeSize = rend.bounds.size;

        SpawnGrid();
    }

    void SpawnGrid()
    {
        Quaternion spawnRotation = Quaternion.Euler(-90f, 180f, 0f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 offset = new Vector3(
                    x * cubeSize.x * spacingMultiplier,
                    0f,
                    z * cubeSize.z * spacingMultiplier
                );

                Vector3 spawnPosition = startPosition + offset;

                GameObject newCube = Instantiate(cubePrefab, spawnPosition, spawnRotation);

                Patient patient = newCube.GetComponent<Patient>();

                if (patient != null)
                {
                    Patient.Condition[] allConditions =
                        (Patient.Condition[])System.Enum.GetValues(typeof(Patient.Condition));

                    int randomIndex = Random.Range(0, allConditions.Length);
                    patient.SetCondition(allConditions[randomIndex]);
                }
            }
        }
    }
}
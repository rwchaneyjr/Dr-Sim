using UnityEngine;

public class CubeGridSpawner : MonoBehaviour
{
    [Header("Drag your room prefab here")]
    public GameObject cubePrefab;

    [Header("Patient Character Prefab")]
    public GameObject patientPrefab;

    [Header("Grid Size")]
    public int width = 10;
    public int height = 10;

    [Header("Spacing Multiplier")]
    public float spacingMultiplier = 1.1f;

    [Header("Start Position Offset")]
    public Vector3 startPosition = Vector3.zero;

    [Header("Patient Settings")]
    public Vector3 patientOffset = new Vector3(0f, -2f, 0f);

    [Tooltip("Base scale for the FBX")]
    public float patientScale = 0.001f;

    [Tooltip("Extra shrink amount. 0.4 = 2/5 of current size")]
    public float shrinkMultiplier = 0.4f;

    [Header("Patient Rotation")]
    public Vector3 patientRotation = new Vector3(-90f, 90f, 90f);

    private Vector3 cubeSize = Vector3.one;

    void Start()
    {
       
        if (cubePrefab == null || patientPrefab == null)
        {
            Debug.LogError("Missing prefab assignment.");
            return;
        }

        Renderer rend = cubePrefab.GetComponentInChildren<Renderer>();

        if (rend == null)
        {
            Debug.LogError("Room prefab has no Renderer.");
            return;
        }

        cubeSize = rend.bounds.size;

        SpawnGrid();
    }

    void SpawnGrid()
    {
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

                GameObject newCube = Instantiate(
                    cubePrefab,
                    spawnPosition,
                    cubePrefab.transform.rotation
                );

                GameObject newPatient = Instantiate(patientPrefab);
                newPatient.transform.SetParent(newCube.transform, false);

                newPatient.transform.localPosition = patientOffset;
                newPatient.transform.localRotation = Quaternion.Euler(patientRotation);

                float finalScale = patientScale * shrinkMultiplier;
                newPatient.transform.localScale = Vector3.one * finalScale;

                Renderer roomRenderer = newCube.GetComponentInChildren<Renderer>();
                Renderer patientRenderer = newPatient.GetComponentInChildren<Renderer>();

                if (roomRenderer != null && patientRenderer != null)
                {
                    // 1) snap feet to floor
                    float floorY = roomRenderer.bounds.min.y;
                    float patientBottom = patientRenderer.bounds.min.y;
                    float difference = floorY - patientBottom;
                    newPatient.transform.position += new Vector3(0f, difference, 0f);

                    // 2) move patient back by 1.5x patient height
                    float patientHeight = patientRenderer.bounds.size.y;
                   // float backAmount = patientHeight * 5f;

                  
                }
                newPatient.transform.position = new Vector3(newPatient.transform.position.x, newPatient.transform.position.y+1, newPatient.transform.position.z +9f);
                Patient patient = newPatient.GetComponent<Patient>();

                if (patient != null)
                {
                    Patient.Condition[] allConditions =
                        (Patient.Condition[])System.Enum.GetValues(typeof(Patient.Condition));

                    int randomIndex = Random.Range(0, allConditions.Length);
                    patient.SetCondition(allConditions[randomIndex]);
                }
                else
                {
                    Debug.LogWarning("Patient prefab is missing Patient script!");
                }
            }
        }
    }
}


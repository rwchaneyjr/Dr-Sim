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

    [Header("Transparency At Start")]
    [Range(0f, 1f)]
    public float startAlpha = 0f;

    private Vector3 cubeSize = Vector3.one;

    void Start()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("No cube prefab assigned.");
            return;
        }

        Renderer rend = cubePrefab.GetComponentInChildren<Renderer>();

        if (rend == null)
        {
            Debug.LogError("Cube prefab has no Renderer anywhere.");
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

                MakeTransparent(newCube);

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

    void MakeTransparent(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogWarning("Spawned object has no Renderers.");
            return;
        }

        foreach (Renderer rend in renderers)
        {
            Material mat = new Material(rend.material);
            rend.material = mat;

            mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;

            Color color = mat.color;
            color.a = startAlpha;
            mat.color = color;
        }
    }
}
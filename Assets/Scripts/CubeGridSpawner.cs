using UnityEngine;

public class CubeGridSpawner : MonoBehaviour
{
    public static CubeGridSpawner Instance { get; private set; }

    [Header("Room Prefab")]
    public GameObject cubePrefab;

    [Header("Target Prefab")]
    public GameObject targetPrefab;
    float targetHeight = 0 - .87f; // FIXED HEIGHT

    [Header("Roof Pad")]
    public bool spawnRoofPads = true;
    public GameObject roofPadPrefab;
    public Vector3 roofPadScale = new Vector3(2f, 0.02f, 2f);
    public float roofPadVerticalOffset = 0.03f;

    [Header("Room Scale")]
    public float roomScale = 425f;

    [Header("Grid Size")]
    public int rows = 3;
    public int columns = 3;

    [Header("Spacing")]
    public float spacingMultiplier = 2.5f;

    [Header("Start Position")]
    public Vector3 startPosition = new Vector3(20f, 0f, 0f);

    [Header("Room Rotation")]
    public Vector3 roomRotation = new Vector3(-90f, 90f, 0f);

    [Header("Patient Move")]
    public Vector3 patientTargetOffset = new Vector3(0f, 0.15f, 0f);
    public GameObject patientPrefab;
    [Header("Camera Move")]
    public Camera cameraToMove;
    public Vector3 cameraTargetOffset = new Vector3(0f, 8f, -8f);
    public bool moveCameraToTarget = true;

    // 🔥 GRID STORAGE
    public RoomController[,] grid;

    public int activeRow = 0;
    public int activeCol = 1;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (cubePrefab == null || targetPrefab == null)
        {
            Debug.LogError("Missing prefab assignments!");
            return;
        }

        if (cameraToMove == null)
            cameraToMove = Camera.main;

        grid = new RoomController[rows, columns];

        SpawnGrid();
    }

    // =========================
    // 🔥 GRID SPAWN
    // =========================
    void SpawnGrid()
    {
        Renderer rend = cubePrefab.GetComponentInChildren<Renderer>();

        Vector3 size = rend != null ? rend.bounds.size : new Vector3(13f, 13f, 13f);

        float spacingX = size.x * spacingMultiplier;
        float spacingZ = size.z * spacingMultiplier;

        Quaternion rotation = Quaternion.Euler(roomRotation);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 spawnPos = startPosition + new Vector3(
                    col * spacingX,
                    0f,
                    row * spacingZ
                );

                RoomController room = SpawnRoom(spawnPos, rotation);

                room.row = row;
                room.col = col;

                grid[row, col] = room;
            }
        }
    }

    // =========================
    // 🔥 SPAWN ROOM + TARGET
    // =========================
    RoomController SpawnRoom(Vector3 position, Quaternion rotation)
    {
        GameObject newCube = Instantiate(cubePrefab, position, rotation);

        newCube.transform.localScale = new Vector3(roomScale, roomScale, roomScale);

        RoomController room = newCube.AddComponent<RoomController>();

        Vector3 roomCenter = newCube.transform.position;
        Vector3 targetOffset = new Vector3(0f, 0.872f, 0f); // slightly above floor

        GameObject target = Instantiate(
            targetPrefab,
            roomCenter - targetOffset,
            Quaternion.identity
        );

        Patient patient = FindObjectOfType<Patient>();

        if (patient != null)
        {
            int conditionCount =
                System.Enum.GetValues(typeof(Patient.Condition)).Length;

            int rand = Random.Range(1, conditionCount);
            // 1 skips Healthy
            // conditionCount includes all enum values

            patient.SetCondition((Patient.Condition)rand);

            Debug.Log("Assigned condition: " + patient.currentCondition);
        }

        target.transform.localScale = new Vector3(2f, 0.02f, 2f);

        target.SetActive(false);

        room.target = target;
        room.roofPad = SpawnRoofPad(newCube);

        target.transform.SetParent(null);

        return room;
    }

    GameObject SpawnRoofPad(GameObject roomObject)
    {
        if (!spawnRoofPads)
            return null;

        GameObject padPrefab = roofPadPrefab != null ? roofPadPrefab : targetPrefab;

        if (padPrefab == null)
            return null;

        Bounds roomBounds = GetRoomBounds(roomObject);
        Vector3 roofPosition = new Vector3(
            roomBounds.center.x,
            roomBounds.max.y + roofPadVerticalOffset,
            roomBounds.center.z
        );

        GameObject roofPad = Instantiate(
            padPrefab,
            roofPosition,
            Quaternion.identity
        );

        roofPad.name = "Roof Pad";
        roofPad.transform.localScale = roofPadScale;
        roofPad.SetActive(true);
        roofPad.transform.SetParent(roomObject.transform, true);

        return roofPad;
    }

    Bounds GetRoomBounds(GameObject roomObject)
    {
        Renderer[] renderers = roomObject.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return new Bounds(roomObject.transform.position, Vector3.zero);

        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return bounds;
    }

    // =========================
    // 🔥 HIDE ALL TARGETS
    // =========================
    public void HideAllTargets()
    {
        foreach (RoomController room in grid)
        {
            if (room != null)
                room.HideTarget();
        }
    }

    // =========================
    // 🔥 CORRECT ANSWER → NEXT TARGET
    // =========================



    [Header("Forward Direction")]
    public int northRowStep = 1; // If it moves wrong direction, change this to -1

    public void ShowNextForwardTarget()
    {
        RoomController currentRoom = FindActiveTargetRoom();

        if (currentRoom == null)
        {
            Debug.LogWarning("No active target found. Showing first room target.");
            HideAllTargets();

            if (grid[0, 0] != null)
                grid[0, 0].ShowTarget();

            return;
        }

        int nextRow = currentRoom.row + northRowStep;
        int nextCol = currentRoom.col;

        if (nextRow < 0 || nextRow >= rows)
        {
            Debug.Log("All rooms completed.");
            return;
        }

        RoomController nextRoom = grid[nextRow, nextCol];

        if (nextRoom == null)
        {
            Debug.LogError("No room found at row " + nextRow + ", col " + nextCol);
            return;
        }

        HideAllTargets();
        nextRoom.ShowTarget();
     
         
         PutPatientOnTarget(nextRoom.target);
  
        Debug.Log("Moved target north from " + currentRoom.row + "," + currentRoom.col +
                  " to " + nextRow + "," + nextCol);
    }
    public void MovePlayerLeft()
    {
        ShowLeftTargetForWrongAnswer();
    }

    [Header("Wrong Answer Direction")]
    public int wrongAnswerLeftColStep = -1; // If left moves wrong direction, change this to 1

    public void ShowLeftTargetForWrongAnswer()
    {
        RoomController currentRoom = FindActiveTargetRoom();

        if (currentRoom == null)
        {
            Debug.LogWarning("No active target found. Cannot move left.");
            return;
        }

        int nextRow = currentRoom.row;
        int nextCol = currentRoom.col + wrongAnswerLeftColStep;

        if (nextCol < 0 || nextCol >= columns)
        {
            Debug.Log("No room to the left.");
            return;
        }

        RoomController nextRoom = grid[nextRow, nextCol];

        if (nextRoom == null)
        {
            Debug.LogError("No room found at row " + nextRow + ", col " + nextCol);
            return;
        }

        HideAllTargets();
        nextRoom.ShowTarget();

        PutPatientOnTarget(nextRoom.target);

        Debug.Log("Wrong answer moved patient left from " + currentRoom.row + "," + currentRoom.col +
                  " to " + nextRow + "," + nextCol);
    }

    RoomController FindActiveTargetRoom()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                RoomController room = grid[row, col];

                if (room != null && room.target != null && room.target.activeSelf)
                {
                    return room;
                }
            }
        }

        return null;
    }
    void PutPatientOnTarget(GameObject target)
    {
        if (target == null)
            return;

        Patient patient = FindObjectOfType<Patient>();

        if (patient == null)
        {
            Debug.LogWarning("No Patient found in scene.");
            return;
        }

        patient.transform.position = target.transform.position + patientTargetOffset;
        DoctorTool tool = FindObjectOfType<DoctorTool>();
        if (tool != null)
        {
            tool.SelectPatient(patient);
            Debug.Log("GAME LOOP RESTARTED IN NEXT ROOM");
        }

    }
    void MovePatientToTarget(GameObject target)
    {
        if (target == null)
            return;

        Patient patient = FindObjectOfType<Patient>();

        if (patient == null)
            return;

        patient.transform.position = target.transform.position + patientTargetOffset;
    }
  
    void MoveCameraToTarget(GameObject target)
    {
        if (!moveCameraToTarget || target == null)
            return;

        if (cameraToMove == null)
            cameraToMove = Camera.main;

        if (cameraToMove == null)
            return;

        cameraToMove.transform.position = target.transform.position + cameraTargetOffset;
        cameraToMove.transform.LookAt(target.transform);
    }
}
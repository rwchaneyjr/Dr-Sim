using UnityEngine;

public class CubeGridSpawner : MonoBehaviour
{
    [Header("Drag your room prefab here")]
    public GameObject cubePrefab;

    [Header("Patient Character Prefab")]
    public GameObject patientPrefab;

    [Header("Treatment Pad")]
    public GameObject padPrefab;
    public Vector3 padScale = new Vector3(2f, 0.15f, 2f);
    public Vector3 padOffset = new Vector3(0f, 0.05f, 0f);
    public Color generatedPadColor = Color.red;

    [Header("Grid Size")]
    public int width = 10;
    public int height = 10;

    [Header("Spacing Multiplier")]
    public float spacingMultiplier = 1.1f;

    [Header("Start Position Offset")]
    public Vector3 startPosition = Vector3.zero;

    [Header("Patient Settings")]
    public bool spawnPatientsOnStart = true;
    public Vector3 patientOffset = new Vector3(0f, -2f, 0f);
    public Vector3 patientOnPadOffset = new Vector3(0f, 0.05f, 0f);

    [Tooltip("Base scale for the FBX")]
    public float patientScale = 0.001f;

    [Tooltip("Extra shrink amount. 0.4 = 2/5 of current size")]
    public float shrinkMultiplier = 0.4f;

    [Header("Patient Rotation")]
    public Vector3 patientRotation = new Vector3(-90f, 90f, 90f);

    public RoomController[,] grid;

    private Vector3 cubeSize = Vector3.one;
    private RoomController activeRoom;
    private Patient activePatient;

    void Start()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("Missing room prefab assignment.");
            return;
        }

        if (patientPrefab == null)
            Debug.LogWarning("Patient prefab is missing. Correct cures can still move an existing patient.");

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
        grid = new RoomController[height, width];

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

                RoomController room = newCube.GetComponent<RoomController>();

                if (room == null)
                    room = newCube.AddComponent<RoomController>();

                room.Initialize(this, z, x);
                grid[z, x] = room;

                if (spawnPatientsOnStart)
                    SpawnPatientInRoom(room);
            }
        }
    }

    public int rows
    {
        get { return height; }
    }

    public int columns
    {
        get { return width; }
    }

    public RoomController SpawnNextRoomPadAndPatient(RoomController currentRoom, RoomDirection direction, Patient patientToMove)
    {
        if (currentRoom == null)
        {
            Debug.LogWarning("Cannot spawn next room target without a current room.");
            return null;
        }

        RoomController nextRoom = GetRoomInDirection(currentRoom, direction);

        if (nextRoom == null)
        {
            Debug.Log("No room " + direction + " of [" + currentRoom.row + "," + currentRoom.col + "].");
            return null;
        }

        ActivateRoom(nextRoom, patientToMove);
        return nextRoom;
    }

    public RoomController SpawnPadAndPatientInFrontOf(Patient patientToMove)
    {
        if (patientToMove == null)
        {
            Debug.LogWarning("Cannot spawn the next room pad without a patient.");
            return null;
        }

        RoomController currentRoom = patientToMove.GetComponentInParent<RoomController>();

        if (currentRoom == null)
            currentRoom = FindClosestRoom(patientToMove.transform.position);

        RoomDirection frontDirection = GetDirectionFromForward(patientToMove.transform.forward);
        return SpawnNextRoomPadAndPatient(currentRoom, frontDirection, patientToMove);
    }

    RoomDirection GetDirectionFromForward(Vector3 forward)
    {
        Vector3 flatForward = new Vector3(forward.x, 0f, forward.z);

        if (flatForward.sqrMagnitude <= 0.001f)
            return RoomDirection.North;

        flatForward.Normalize();

        if (Mathf.Abs(flatForward.z) >= Mathf.Abs(flatForward.x))
            return flatForward.z >= 0f ? RoomDirection.North : RoomDirection.South;

        return flatForward.x >= 0f ? RoomDirection.East : RoomDirection.West;
    }

    public RoomController GetRoomInDirection(RoomController currentRoom, RoomDirection direction)
    {
        if (currentRoom == null || grid == null)
            return null;

        int nextRow = currentRoom.row;
        int nextCol = currentRoom.col;

        switch (direction)
        {
            case RoomDirection.North:
                nextRow += 1;
                break;
            case RoomDirection.South:
                nextRow -= 1;
                break;
            case RoomDirection.East:
                nextCol += 1;
                break;
            case RoomDirection.West:
                nextCol -= 1;
                break;
        }

        if (nextRow < 0 || nextRow >= height || nextCol < 0 || nextCol >= width)
            return null;

        return grid[nextRow, nextCol];
    }

    public RoomController FindClosestRoom(Vector3 worldPosition)
    {
        if (grid == null)
            return null;

        RoomController closestRoom = null;
        float closestDistance = float.MaxValue;

        foreach (RoomController room in grid)
        {
            if (room == null)
                continue;

            float distance = Vector3.SqrMagnitude(room.GetCenter() - worldPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestRoom = room;
            }
        }

        return closestRoom;
    }

    public void HideAllTargets()
    {
        if (grid == null)
            return;

        foreach (RoomController room in grid)
        {
            if (room != null)
                room.HideTarget();
        }
    }

    void ActivateRoom(RoomController room, Patient patientToMove)
    {
        if (activeRoom != null && activeRoom != room)
            activeRoom.HideTarget();

        GameObject pad = EnsurePad(room);
        Patient patient = patientToMove != null ? patientToMove : GetPatientForRoom(room);

        if (patient != null)
            PlacePatientOnPad(room, patient, pad);

        room.ShowTarget();
        activeRoom = room;
        activePatient = patient;

        Debug.Log("Activated room [" + room.row + "," + room.col + "] for patient and pad.");
    }

    Patient GetPatientForRoom(RoomController room)
    {
        if (room.patient != null)
            return room.patient;

        if (activePatient != null)
            return activePatient;

        return SpawnPatientInRoom(room);
    }

    Patient SpawnPatientInRoom(RoomController room)
    {
        if (patientPrefab == null || room == null)
            return null;

        GameObject newPatient = Instantiate(patientPrefab);
        newPatient.transform.SetParent(room.transform, false);

        newPatient.transform.localPosition = patientOffset;
        newPatient.transform.localRotation = Quaternion.Euler(patientRotation);

        float finalScale = patientScale * shrinkMultiplier;
        newPatient.transform.localScale = Vector3.one * finalScale;

        Renderer patientRenderer = newPatient.GetComponentInChildren<Renderer>();

        if (patientRenderer != null)
        {
            float difference = room.GetFloorY() - patientRenderer.bounds.min.y;
            newPatient.transform.position += new Vector3(0f, difference, 0f);
        }

        Patient patient = newPatient.GetComponent<Patient>();

        if (patient != null)
        {
            Patient.Condition[] allConditions =
                (Patient.Condition[])System.Enum.GetValues(typeof(Patient.Condition));

            int randomIndex = Random.Range(0, allConditions.Length);
            patient.SetCondition(allConditions[randomIndex]);
            room.patient = patient;
        }
        else
        {
            Debug.LogWarning("Patient prefab is missing Patient script!");
        }

        return patient;
    }

    GameObject EnsurePad(RoomController room)
    {
        if (room.target != null)
        {
            PositionPad(room.target, room);
            room.target.SetActive(true);
            return room.target;
        }

        GameObject pad = padPrefab != null
            ? Instantiate(padPrefab)
            : GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        pad.name = "Treatment Pad [" + room.row + "," + room.col + "]";
        pad.transform.localScale = padScale;

        if (padPrefab == null)
        {
            Renderer padRenderer = pad.GetComponent<Renderer>();

            if (padRenderer != null)
                padRenderer.material.color = generatedPadColor;
        }

        Collider padCollider = pad.GetComponent<Collider>();

        if (padCollider != null)
            padCollider.isTrigger = true;

        if (pad.GetComponent<DoctorHealTile>() == null)
            pad.AddComponent<DoctorHealTile>();

        PositionPad(pad, room);
        room.target = pad;

        return pad;
    }

    void PositionPad(GameObject pad, RoomController room)
    {
        Vector3 center = room.GetCenter();
        pad.transform.position = new Vector3(center.x, room.GetFloorY(), center.z) + padOffset;

        Renderer padRenderer = pad.GetComponentInChildren<Renderer>();

        if (padRenderer != null)
        {
            float difference = (room.GetFloorY() + padOffset.y) - padRenderer.bounds.min.y;
            pad.transform.position += new Vector3(0f, difference, 0f);
        }
    }

    void PlacePatientOnPad(RoomController room, Patient patient, GameObject pad)
    {
        patient.transform.SetParent(room.transform, true);
        patient.transform.rotation = Quaternion.Euler(patientRotation);

        Vector3 padCenter = pad.transform.position;
        patient.transform.position = new Vector3(padCenter.x, patient.transform.position.y, padCenter.z) + patientOnPadOffset;

        Renderer padRenderer = pad.GetComponentInChildren<Renderer>();
        Renderer patientRenderer = patient.GetComponentInChildren<Renderer>();

        if (padRenderer != null && patientRenderer != null)
        {
            float difference = padRenderer.bounds.max.y - patientRenderer.bounds.min.y;
            patient.transform.position += new Vector3(0f, difference, 0f);
        }

        room.patient = patient;
    }
}

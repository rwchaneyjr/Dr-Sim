using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static int sickTiles = 0;

    private bool readyToCheck = false;
    private bool gameWon = false;

    void Start()
    {
        sickTiles = 0;
        gameWon = false;

        // Wait one frame so all HealthTiles/Patients can register themselves first
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return null; // wait one frame
        readyToCheck = true;
        Debug.Log("Game started. Sick tiles: " + sickTiles);
    }

    void Update()
    {
        if (readyToCheck && !gameWon && sickTiles <= 0)
        {
            gameWon = true;
            Debug.Log("YOU WIN");
        }
    }
}

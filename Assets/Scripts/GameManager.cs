using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int sickTiles = 0;
    private bool gameStarted = false;

    void Start()
    {
        sickTiles = 0;
        gameStarted = true;
    }

    void Update()
    {
        if (gameStarted && sickTiles <= 0)
        {
            Debug.Log("YOU WIN");
        }
    }
}
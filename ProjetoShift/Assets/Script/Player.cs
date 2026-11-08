using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    GameManager gameManager;
    GameObject[] Runners;
    int currentRunner = -1;
    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }
    void Start()
    {
        Runners = gameManager.GetRunners();
        if (Runners.Length > 0)
        {
            currentRunner = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            gameManager.MoveRunnerRight(Runners[currentRunner]);
        }
        
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            gameManager.MoveRunnerLeft(Runners[currentRunner]);
        }
        if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {
            Debug.Log("shiftKey key pressed");

            if (currentRunner < Runners.Length - 1)
            {
                currentRunner++;
            }
            else
            {
                currentRunner = 0;
            }   
        }
    }
}

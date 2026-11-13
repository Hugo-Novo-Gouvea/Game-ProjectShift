using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Referencia para o GameManager
    GameManager gameManager;

    // Variaveis para armazenar os dados dos runners
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
        // Verifica se o jogo não acabou antes de permitir o movimento dos runners
        if (!gameManager.IsGameOver)
        {
            // Verifica se as teclas de seta ou WASD foram pressionadas
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
            {
                gameManager.MoveRunnerRight(Runners[currentRunner]);
            }

            if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
            {
                gameManager.MoveRunnerLeft(Runners[currentRunner]);
            }
            if (Keyboard.current.shiftKey.wasPressedThisFrame)
            {

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
}

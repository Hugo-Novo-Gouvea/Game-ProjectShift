using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Estrutura para armazenar a referência do runner e a linha correspondente
    private struct LinesStruct
    {
        public GameObject RunnerObject;
        public Transform LineTransform;
    }
    private LinesStruct[] Lines;
    [SerializeField]
    // Array de runners a serem instanciados
    private GameObject[] Runners;

    // Variaveis para armazenar os dados dos inimigos
    [SerializeField]
    private GameObject[] Enemies;
    [SerializeField]
    private float spawnInterval = 2f; // Intervalo de spawn em segundos
    [SerializeField]
    private int maxEnemiesOnScreenOnStart; // Número máximo de inimigos na tela no inicio do jogo
    private int currentEnemiesOnScreen = 0; // Contador de inimigos na tela

    // Variável para controlar o tempo
    private float timeMaster = 0f;

    // Variavel para pontuação
    public float Score { get; private set; } = 0;

    [SerializeField]
    private TMP_Text hudScoreText;
    [SerializeField]
    private TMP_Text endScore;
    // Variável para controlar o estado do jogo
    public bool IsGameOver { get; private set; }

    // Variavel para canvas de game over

    [SerializeField]
    private GameObject gameOverCanvas;

    // Singleton instance
    public static GameManager Instance { get; private set; }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        int childCount = transform.childCount;
        if (childCount == 0 || Runners == null || Runners.Length == 0)
        { 
            Debug.LogWarning("Não há linhas ou runners definidos. Certifique-se de que o GameManager tenha filhos e que o array Runners esteja preenchido.");
            return; 
        }

        // 1. Preenche o array Lines com os Transforms dos filhos
        Lines = new LinesStruct[childCount];
        for (int c = 0; c < childCount; c++)
        {
            Lines[c] = new LinesStruct
            {
                RunnerObject = null,
                LineTransform = transform.GetChild(c),
            };
        }

        // 2. Calcula o espaçamento com segurança
        int runnerCount = Runners.Length;
        int aux = runnerCount > 1 ? (Lines.Length - 1) / (runnerCount - 1) : 0;

        // 3. Itera instanciando cada runner na sua respectiva linha e marcando a linha como ocupada
        for (int i = 0; i < runnerCount; i++)
        {
            int lineIndex = Mathf.Clamp(aux * i, 0, Lines.Length - 1);
            GameObject runner = Instantiate(Runners[i], Lines[lineIndex].LineTransform.position, Quaternion.identity);
            Lines[lineIndex].RunnerObject = runner;
        }

        currentEnemiesOnScreen = maxEnemiesOnScreenOnStart;

    }
    private void Start()
    {
        StartCoroutine(RoutineSpawn());

        if (gameOverCanvas == null)
        {
            gameOverCanvas = GameObject.Find("GameOverCanvas");
        }
    }

    private void FixedUpdate()
    {
        timeMaster += Time.fixedDeltaTime;

        if (!IsGameOver)
        {
            Score += 10 * Time.fixedDeltaTime;
            hudScoreText.SetText("Score: {0:0}", Score);
        }

        if (Score >= 1000)
        {
            spawnInterval = 1f; // Reduz o intervalo de spawn para 1 segundo
            currentEnemiesOnScreen = maxEnemiesOnScreenOnStart + 2; // Aumenta o número de inimigos na tela
        }
        else if (Score >= 300)
        {
            currentEnemiesOnScreen = maxEnemiesOnScreenOnStart + 1; // Aumenta o número de inimigos na tela
        }

        spawnInterval -= 0.0833f * Time.fixedDeltaTime; // Reduz o intervalo de spawn em 0.083 segundos a cada segundo
    }

    // Método para obter os runners instanciados
    public GameObject[] GetRunners()
    {
        GameObject[] tempRunner = new GameObject[Runners.Length];
        int aux = 0;
        for (int i = 0; i < Lines.Length; i++)
        {
            if (Lines[i].RunnerObject != null)
            {
                tempRunner[aux] = Lines[i].RunnerObject;
                aux++;
            }
        }
        return tempRunner;
    }

    // Métodos para mover os runners
    public void MoveRunnerRight(GameObject runner)
    {
        int currentLineIndex = -1;
        // checando onde o runner está atualmente
        for (int i = 0; i < Lines.Length; i++)
        {
            if (Lines[i].RunnerObject == runner)
            {
                currentLineIndex = i;
                break;
            }
        }

        // checando se o runner pode se mover para a direita
        if (currentLineIndex < Lines.Length - 1 && Lines[currentLineIndex + 1].RunnerObject == null)
        {
            // movendo o runner para a linha à direita
            runner.transform.position = Lines[currentLineIndex + 1].LineTransform.position;
            Lines[currentLineIndex + 1].RunnerObject = runner;
            Lines[currentLineIndex].RunnerObject = null;
        }
    }

    public void MoveRunnerLeft(GameObject runner)
    {
        int currentLineIndex = -1;
        // checando onde o runner está atualmente
        for (int i = 0; i < Lines.Length; i++)
        {
            if (Lines[i].RunnerObject == runner)
            {
                currentLineIndex = i;
                break;
            }
        }

        // checando se o runner pode se mover para a esquerda
        if (currentLineIndex > 0 && Lines[currentLineIndex - 1].RunnerObject == null)
        {
            // movendo o runner para a linha à esquerda
            runner.transform.position = Lines[currentLineIndex - 1].LineTransform.position;
            Lines[currentLineIndex - 1].RunnerObject = runner;
            Lines[currentLineIndex].RunnerObject = null;
        }
    }

    private IEnumerator RoutineSpawn()
    {
        while (!IsGameOver)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }

    }

    private void SpawnEnemy()
    {
        if (Enemies.Length == 0) return;
        // Escolhe um inimigo aleatório
        GameObject enemyPrefab = Enemies[Random.Range(0, Enemies.Length)];
        // Escolhe uma linha aleatória
        int lineIndex = Random.Range(0, Lines.Length);
        Vector3 spawnPoint = new Vector3 (Lines[lineIndex].LineTransform.position.x, Lines[lineIndex].LineTransform.position.y + 12f, 0f);
        // Instancia o inimigo na posição da linha escolhida
        if (FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length < currentEnemiesOnScreen)
        {
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        }
    }
    public void TriggerGameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;

        // Interrompe imediatamente o agendamento do SpawnEnemy
        StopCoroutine(RoutineSpawn());

        // Ativa o canvas de Game Over
        if (gameOverCanvas != null)
        {
            Debug.Log("Game Over! Ativando o canvas de Game Over.");
            gameOverCanvas.SetActive(true);
            endScore.SetText("Score: {0:0}", Score);
        }
    }

}

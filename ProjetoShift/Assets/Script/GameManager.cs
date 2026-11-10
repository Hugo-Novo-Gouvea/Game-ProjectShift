using UnityEngine;

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

    // Variável para controlar o tempo desde o último spawn
    private float timeSinceLastSpawn = 0f;

    void Awake()
    {
        int childCount = transform.childCount;
        if (childCount == 0 || Runners == null || Runners.Length == 0) return;

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

    }

    private void FixedUpdate()
    {
        timeSinceLastSpawn += Time.fixedDeltaTime;
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }
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

    private void SpawnEnemy()
    {
        if (Enemies.Length == 0) return;
        // Escolhe um inimigo aleatório
        GameObject enemyPrefab = Enemies[Random.Range(0, Enemies.Length)];
        // Escolhe uma linha aleatória
        int lineIndex = Random.Range(0, Lines.Length);
        Vector3 spawnPoint = new Vector3 (Lines[lineIndex].LineTransform.position.x, Lines[lineIndex].LineTransform.position.y + 12f, 0f);
        // Instancia o inimigo na posição da linha escolhida
        Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
    }

}

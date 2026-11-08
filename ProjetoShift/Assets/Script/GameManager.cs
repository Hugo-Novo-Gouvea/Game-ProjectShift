using UnityEngine;

public class GameManager : MonoBehaviour
{
    private struct LinesStruct
    {
        public GameObject RunnerObject;
        public Transform LineTransform;
    }
    private LinesStruct[] Lines;
    [SerializeField]
    private GameObject[] Runners;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

}

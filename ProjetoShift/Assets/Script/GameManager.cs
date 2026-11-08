using UnityEngine;

public class GameManager : MonoBehaviour
{
    private struct LinesStruct
    {
        public Transform Position;
        public bool IsOccupied;
    }
    private LinesStruct[] Lines;
    [SerializeField]
    private GameObject[] Runners;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int childCount = transform.childCount;
        if (childCount == 0 || Runners == null || Runners.Length == 0) return;

        // 1. Preenche o array Lines com os Transforms dos filhos
        Lines = new LinesStruct[childCount];
        for (int c = 0; c < childCount; c++)
        {
            Lines[c] = new LinesStruct
            {
                Position = transform.GetChild(c),
                IsOccupied = false
            };
        }

        // 2. Calcula o espaçamento com segurança
        int runnerCount = Runners.Length;
        int aux = runnerCount > 1 ? (Lines.Length - 1) / (runnerCount - 1) : 0;

        // 3. Itera instanciando cada runner na sua respectiva linha e marcando a linha como ocupada
        for (int i = 0; i < runnerCount; i++)
        {
            int lineIndex = Mathf.Clamp(aux * i, 0, Lines.Length - 1);
            Instantiate(Runners[i], Lines[lineIndex].Position.position, Quaternion.identity);
            Lines[lineIndex].IsOccupied = true;
        }

    }

    public void MoveRunner(GameObject runner, int newLineIndex)
    {
        if (newLineIndex < 0 || newLineIndex >= Lines.Length)
        {
            Debug.LogWarning("Invalid line index.");
            return;
        }
        // Encontra o índice da linha atual do runner
        int currentLineIndex = -1;
        for (int i = 0; i < Lines.Length; i++)
        {
            if (Lines[i].IsOccupied && Lines[i].Position == runner.transform.parent)
            {
                currentLineIndex = i;
                break;
            }
        }
        if (currentLineIndex == -1)
        {
            Debug.LogWarning("Runner is not on any line.");
            return;
        }
        // Checa se o índice da nova linha é visinho ao índice da linha atual
        if (Mathf.Abs(newLineIndex - currentLineIndex) != 1)
        {
            Debug.LogWarning("The new line index is not adjacent to the current line index.");
            return;
        }
        // Checa se a nova linha está ocupada
        if (Lines[newLineIndex].IsOccupied)
        {
            Debug.LogWarning("The target line is already occupied.");
            return;
        }
        // move the runner to the new line
        runner.transform.position = Lines[newLineIndex].Position.position;
        // Atualiza o estado de ocupação das linhas
        Lines[currentLineIndex].IsOccupied = false;
        Lines[newLineIndex].IsOccupied = true;
    }
}

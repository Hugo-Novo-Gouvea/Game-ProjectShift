using UnityEngine;

public class Runner : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            gameManager.TriggerGameOver();
            // Aqui você pode adicionar a lógica para quando o inimigo colidir com o runner
            Debug.Log("Inimigo colidiu com o Player!");
        }
    }
}

using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameManager gameManager;

    [SerializeField]
    private float speed = 5f; // Velocidade do inimigo
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void FixedUpdate()
    {
        if (!gameManager.IsGameOver)
        {
            Movement();
        }
        else
        {
            rb.linearVelocityY = 0f; // Para o movimento do inimigo quando o jogo termina
        }
    }

    private void Movement()
    {
        rb.linearVelocityY = -speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BottomBoundary"))
        {
            // Aqui você pode adicionar a lógica para quando o inimigo sair da tela
            Debug.Log("Inimigo saiu da tela!");
            Destroy(gameObject); // Destroi o inimigo quando ele sai da tela
        }
    }
}

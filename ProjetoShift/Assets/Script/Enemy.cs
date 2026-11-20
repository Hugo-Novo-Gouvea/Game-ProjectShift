using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameManager gameManager;

    float tempoDeVida = 0f; // Tempo de vida

    [SerializeField]
    private float speed; // Velocidade do inimigo
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();
        speed = speed + (gameManager.Score *0.01f) + (0.001f * speed * gameManager.Score); // Aumenta a velocidade com base na pontuação
    }

    void FixedUpdate()
    {
        tempoDeVida += Time.fixedDeltaTime;
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
        rb.linearVelocityY = -speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BottomBoundary"))
        {
            // Aqui você pode adicionar a lógica para quando o inimigo sair da tela
            Debug.Log("Inimigo saiu da tela! Tempo de vida: " + tempoDeVida);
            Destroy(gameObject); // Destroi o inimigo quando ele sai da tela
        }
    }
}

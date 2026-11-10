using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float speed = 5f; // Velocidade do inimigo
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        rb.linearVelocityY = -speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Aqui você pode adicionar a lógica para quando o inimigo colidir com o runner
            Debug.Log("Inimigo colidiu com o Player!");
        }
        if (collision.CompareTag("BottomBoundary"))
        {
            // Aqui você pode adicionar a lógica para quando o inimigo sair da tela
            Debug.Log("Inimigo saiu da tela!");
            Destroy(gameObject); // Destroi o inimigo quando ele sai da tela
        }
    }
}

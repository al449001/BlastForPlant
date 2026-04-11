using UnityEngine;

// Obligamos a Unity a que este objeto tenga siempre estos componentes
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class ControlEnemigo : MonoBehaviour
{
    [Header("Configuración de IA")]
    public float radioDeVision = 6f;
    public float radioDeAtaque = 1.5f;
    public float velocidad = 2f;

    [Header("Referencias")]
    public Transform jugador;

    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Si se te olvida poner al jugador en el Inspector, el script lo busca por ti
        if (jugador == null)
        {
            GameObject objJugador = GameObject.Find("Personaje");
            if (objJugador != null) jugador = objJugador.transform;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        // --- Lógica de Movimiento e IA ---
        if (distancia <= radioDeAtaque)
        {
            // FASE 1: ATACAR
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Frena
            animator.SetBool("Moviendose", false);
            animator.SetBool("Atacando", true);
            MirarAlJugador();
        }
        else if (distancia <= radioDeVision)
        {
            // FASE 2: PERSEGUIR
            animator.SetBool("Atacando", false);
            animator.SetBool("Moviendose", true);

            // Si el jugador está a la derecha, direccionX será 1. Si está a la izquierda, será -1.
            float direccionX = (jugador.position.x - transform.position.x) > 0 ? 1 : -1;
            rb.linearVelocity = new Vector2(direccionX * velocidad, rb.linearVelocity.y);
            MirarAlJugador();
        }
        else
        {
            // FASE 3: IDLE (Quieto)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetBool("Atacando", false);
            animator.SetBool("Moviendose", false);
        }
    }

    private void MirarAlJugador()
    {
        // ¡AQUÍ ESTABA EL ERROR! 
        // Si el jugador está a la izquierda (x es menor), la escala X debe ser NEGATIVA para voltearlo.
        if (jugador.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // Si el jugador está a la derecha, la escala X debe ser POSITIVA (su estado normal).
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ControlPersonaje personaje = collision.gameObject.GetComponent<ControlPersonaje>();
            if (personaje != null) personaje.RecibirDano();

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeVision);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeAtaque);
    }
}
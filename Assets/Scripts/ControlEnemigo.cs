using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class ControlEnemigo : MonoBehaviour
{
    [Header("Configuración de IA")]
    public float radioDeVision = 6f;
    public float velocidad = 3f; // Ajusta este valor para que no parezca una babosa

    [Header("Referencias")]
    public Transform jugador;

    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Autocargar al jugador
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

        // --- ESTADO 2: PERSECUCIÓN ---
        if (distancia <= radioDeVision)
        {
            // 1. Activamos la animación
            animator.SetBool("Atacando", true);

            // 2. Calculamos la dirección (1 para derecha, -1 para izquierda)
            float direccionX = (jugador.position.x - transform.position.x) > 0 ? 1 : -1;

            // 3. Aplicamos la velocidad constante hacia el jugador
            rb.linearVelocity = new Vector2(direccionX * velocidad, rb.linearVelocity.y);

            // 4. Volteamos el sprite
            MirarAlJugador();
        }
        // --- ESTADO 1: IDLE ---
        else
        {
            // 1. Apagamos la animación
            animator.SetBool("Atacando", false);

            // 2. Detenemos al enemigo en seco (eje X en 0)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void MirarAlJugador()
    {
        if (jugador.position.x < transform.position.x)
        {
            // Mirar izquierda
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // Mirar derecha
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
    }
}
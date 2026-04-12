using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class ControlEnemigo : MonoBehaviour
{
    [Header("Configuración de IA (Saltos)")]
    public float radioDeVision = 6f;
    public float fuerzaSaltoX = 3f;
    public float fuerzaSaltoY = 5f;
    public float tiempoEntreSaltos = 1.5f;

    [Header("Referencias")]
    public Transform jugador;

    private Animator animator;
    private Rigidbody2D rb;
    private float temporizadorSalto;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

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
        temporizadorSalto -= Time.deltaTime;

        // --- LÓGICA STRICTA DE 2 ESTADOS ---

        if (distancia <= radioDeVision)
        {
            // 1. Activa el estado de ataque constantemente mientras estés cerca
            animator.SetBool("Atacando", true);
            MirarAlJugador();

            // 2. Ejecuta el salto físico si toca el suelo y el tiempo ha pasado
            if (temporizadorSalto <= 0f && Mathf.Abs(rb.linearVelocity.y) < 0.1f)
            {
                SaltarHaciaJugador();
                temporizadorSalto = tiempoEntreSaltos;
            }
        }
        else // Si te alejas de su rango de visión...
        {
            // 1. Lo mandamos a descansar (Idle)
            animator.SetBool("Atacando", false);
        }
    }

    private void SaltarHaciaJugador()
    {
        float direccionX = (jugador.position.x - transform.position.x) > 0 ? 1 : -1;
        // Empujón diagonal
        rb.linearVelocity = new Vector2(direccionX * fuerzaSaltoX, fuerzaSaltoY);
    }

    private void MirarAlJugador()
    {
        if (jugador.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
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
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class ControlEnemigo : MonoBehaviour
{
    [Header("Configuración de IA (Saltos)")]
    public float radioDeVision = 6f;
    public float fuerzaSaltoX = 3f; // Lo lejos que salta hacia ti
    public float fuerzaSaltoY = 5f; // Lo alto que salta
    public float tiempoEntreSaltos = 1.5f; // Segundos que espera en el suelo antes de volver a saltar

    [Header("Referencias")]
    public Transform jugador;

    private Animator animator;
    private Rigidbody2D rb;
    private float temporizadorSalto;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Autocargar al jugador si se nos olvida en el Inspector
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

        // Si el jugador entra en el campo de visión...
        if (distancia <= radioDeVision)
        {
            // 1. Activa la animación de movimiento/ataque TODO EL RATO que te vea
            animator.SetBool("Atacando", true);
            MirarAlJugador();

            // 2. Lógica del salto (solo salta si está en el suelo y acabó el tiempo)
            if (temporizadorSalto <= 0f && Mathf.Abs(rb.linearVelocity.y) < 0.1f)
            {
                SaltarHaciaJugador();
                temporizadorSalto = tiempoEntreSaltos;
            }
        }
        else // Si estás lejos y no te ve
        {
            // Vuelve a su estado de reposo absoluto
            animator.SetBool("Atacando", false);
        }
    }

    private void SaltarHaciaJugador()
    {
        // Calculamos la dirección (1 para la derecha, -1 para la izquierda)
        float direccionX = (jugador.position.x - transform.position.x) > 0 ? 1 : -1;

        // Le damos un impulso seco hacia arriba y hacia adelante
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
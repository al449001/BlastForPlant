using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class ControlCorazon : MonoBehaviour
{
    [Header("Efecto de Parpadeo")]
    public float velocidadParpadeo = 0.2f; // Cambia esto para que parpadee más rápido o más lento

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Iniciamos el efecto de parpadeo infinito
        StartCoroutine(ParpadearInfinitamente());
    }

    private IEnumerator ParpadearInfinitamente()
    {
        while (true) // Bucle infinito mientras el corazón exista
        {
            // Alterna entre visible e invisible
            spriteRenderer.enabled = !spriteRenderer.enabled;
            // Espera el tiempo que tú le hayas configurado
            yield return new WaitForSeconds(velocidadParpadeo);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si lo toca el jugador...
        if (collision.CompareTag("Player"))
        {
            ControlPersonaje personaje = collision.GetComponent<ControlPersonaje>();
            if (personaje != null)
            {
                // Le sumamos la vida y destruimos el corazón
                personaje.Curar();
                Destroy(gameObject);
            }
        }
    }
}

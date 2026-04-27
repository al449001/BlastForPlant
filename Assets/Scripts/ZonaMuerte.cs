using UnityEngine;
using System.Collections; // ¡Añadido para poder usar las rutinas de tiempo!
using UnityEngine.SceneManagement;

public class ZonaMuerte : MonoBehaviour
{
    public Transform puntoDeRespawn;
    public string escenaGameOver = "Game Over";

    [Header("Espera de Audio")]
    [Tooltip("Arrastra aquí el MISMO sonido del grito de caída para que el código sepa cuánto dura")]
    public AudioClip gritoCaida;

    // Evita que el trigger se active varias veces seguidas mientras esperamos
    private bool esperandoGameOver = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !esperandoGameOver)
        {
            ControlPersonaje personaje = collision.GetComponent<ControlPersonaje>();

            if (personaje != null)
            {
                personaje.PerderVida();

                if (personaje.vidas > 0)
                {
                    // Si le quedan vidas, reaparece de inmediato como siempre
                    collision.transform.position = puntoDeRespawn.position;
                }
                else
                {
                    // ¡AQUÍ ESTÁ LA MAGIA! Si no le quedan vidas, iniciamos la espera
                    StartCoroutine(RutinaGameOver(personaje.gameObject));
                }
            }
        }
    }

    private IEnumerator RutinaGameOver(GameObject jugador)
    {
        esperandoGameOver = true;

        // 1. Congelamos y ocultamos al jugador para que no siga cayendo hacia el infinito
        jugador.GetComponent<SpriteRenderer>().enabled = false; // Lo hacemos invisible

        Rigidbody2D rb = jugador.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Frenamos su caída en seco
            rb.gravityScale = 0f; // Le quitamos la gravedad
        }

        // Desactivamos su control para que el jugador no pueda moverse ni saltar siendo invisible
        jugador.GetComponent<ControlPersonaje>().enabled = false;

        // 2. Calculamos cuánto tiempo hay que esperar
        float tiempoEspera = 1.5f; // Tiempo por defecto por si olvidas poner el audio en el Inspector
        if (gritoCaida != null)
        {
            tiempoEspera = gritoCaida.length; // Leemos EXACTAMENTE los segundos que dura el MP3
        }

        // 3. Esperamos a que termine de sonar el grito
        yield return new WaitForSeconds(tiempoEspera);

        // 4. Una vez terminado el audio en completo silencio... ¡Cambiamos de escena!
        SceneManager.LoadScene(escenaGameOver);
    }
}
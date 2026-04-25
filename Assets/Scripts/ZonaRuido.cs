using UnityEngine;

public class ZonaRuido : MonoBehaviour
{
    [Header("Grito de Caída")]
    public AudioClip gritoCaida;

    [Range(0f, 1f)]
    public float volumen = 1f; // 1 es el máximo, 0 es silencio

    // Esta variable evitará que el sonido se repita mil veces si el personaje roza el borde
    private bool yaHaGritado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !yaHaGritado)
        {
            yaHaGritado = true; // Marcamos que ya está gritando

            if (gritoCaida != null)
            {
                // Creamos un altavoz temporal que nos persigue "en la oreja" (Sonido 2D)
                GameObject altavozTemporal = new GameObject("Grito_Caida");
                AudioSource fuenteAudio = altavozTemporal.AddComponent<AudioSource>();

                fuenteAudio.clip = gritoCaida;
                fuenteAudio.volume = volumen;
                fuenteAudio.spatialBlend = 0f; // ¡ESTO ES LA CLAVE! 0 significa 2D (volumen máximo sin importar la cámara)

                fuenteAudio.Play();

                // Destruimos el altavoz fantasma cuando termine el grito
                Destroy(altavozTemporal, gritoCaida.length);
            }
        }
    }

    // Cuando el personaje reaparece arriba y sale de la zona, reseteamos el grito para la próxima vez que se caiga
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            yaHaGritado = false;
        }
    }
}

using UnityEngine;

// Ya no necesitamos exigir un AudioSource aquí, porque lo crearemos por código
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ControlBala : MonoBehaviour
{
    [Header("Configuración de Vuelo")]
    public float velocidad = 15f;
    public float tiempoDeVida = 1.5f;

    [Header("Efectos")]
    [Tooltip("Arrastra aquí tu MP3 de disparo")]
    public AudioClip sonidoAlNacer;

    void Start()
    {
        // 1. EL TRUCO DEL ALTAVOZ INDEPENDIENTE
        if (sonidoAlNacer != null)
        {
            // Creamos un objeto vacío y nuevo en la escena
            GameObject altavoz = new GameObject("Sonido_Disparo_Bala");

            // Le añadimos un reproductor de audio
            AudioSource fuente = altavoz.AddComponent<AudioSource>();

            // Lo configuramos en 2D y le metemos tu MP3
            fuente.spatialBlend = 0f;
            fuente.clip = sonidoAlNacer;

            // ¡Que suene!
            fuente.Play();

            // Le decimos a Unity que destruya ESTE ALTAVOZ solo cuando termine el audio
            // Así, la bala puede morir tranquila, el sonido está a salvo.
            Destroy(altavoz, sonidoAlNacer.length);
        }

        // 2. Movimiento normal
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(velocidad, 0f);

        // 3. Autodestrucción por si se pierde en el infinito
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si choca contra el enemigo...
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Destroy(collision.gameObject); // Destruye al enemigo
            Destroy(gameObject); // La bala muere, pero el sonido ya es independiente
        }
    }
}
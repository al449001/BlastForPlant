using UnityEngine;

//Añadimos el AudioSource a la lista de componentes obligatorios
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(AudioSource))]
public class ControlBala : MonoBehaviour
{
    [Header("Configuración de Vuelo")]
    public float velocidad = 15f;
    public float tiempoDeVida = 1.5f;

    [Header("Efectos")]
    [Tooltip("Arrastra aquí tu MP3 de disparo")]
    public AudioClip sonidoAlNacer;

    private AudioSource fuenteAudio;

    void Awake()
    {
        // 1. Preparamos el altavoz de la bala
        fuenteAudio = GetComponent<AudioSource>();
        fuenteAudio.spatialBlend = 0f; //Sonido 2D puro
        fuenteAudio.playOnAwake = false;
    }

    void Start()
    {
        // 2. ¡Hacemos sonar el disparo nada más nacer!
        if (sonidoAlNacer != null)
        {
            fuenteAudio.PlayOneShot(sonidoAlNacer);
        }

        // 3. Arreglo del movimiento: Usamos un Vector2 directo en el eje X. 
        // Como el ControlPersonaje ya le pone la velocidad en positivo o negativo, esto nunca falla.
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(velocidad, 0f);

        // 4. Autodestrucción si se pierde en el infinito
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si choca contra el enemigo...
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Destroy(collision.gameObject); // Destruye al enemigo
            Destroy(gameObject); // Destruye la bala
        }
    }
}
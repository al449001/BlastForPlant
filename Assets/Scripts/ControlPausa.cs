using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// Le obligamos a Unity a ponernos un altavoz permanente para la Interfaz
[RequireComponent(typeof(AudioSource))]
public class ControlPausa : MonoBehaviour
{
    [Header("Lo que aparece al pausar")]
    public GameObject todoElMenuPausa;

    [Header("El Botón de la Mochila")]
    public Image imagenMochila;
    public Sprite mochilaCerrada;
    public Sprite mochilaAbierta;

    [Header("Animación y Sonido del Menú")]
    public Animator animadorMenu;
    public AudioClip sonidoClic;
    public float tiempoDeEspera = 0.5f;

    [Header("Sonidos de la Mochila")]
    [Tooltip("Arrastra el MP3 para cuando se ABRE la pausa")]
    public AudioClip sonidoAbrir;
    [Tooltip("Arrastra el MP3 para cuando se CIERRA la pausa")]
    public AudioClip sonidoCerrar;

    // ==========================================
    // --- NUEVO: CONFIGURACIÓN REINICIAR ---
    // ==========================================
    [Header("Configuración Reiniciar")]
    public AudioClip sonidoReiniciar;
    public float tiempoEsperaReiniciar = 0.5f;

    private bool estaPausado = false;
    private AudioSource fuenteAudioUI; // Nuestro altavoz dedicado

    void Start()
    {
        Time.timeScale = 1f;
        if (todoElMenuPausa != null) todoElMenuPausa.SetActive(false);
        if (imagenMochila != null) imagenMochila.sprite = mochilaCerrada;

        // Configuramos el altavoz al empezar
        fuenteAudioUI = GetComponent<AudioSource>();
        fuenteAudioUI.spatialBlend = 0f; // Sonido puro 2D

        // ¡LA MAGIA!: Le decimos que siga sonando aunque Time.timeScale sea 0
        fuenteAudioUI.ignoreListenerPause = true;
    }

    public void TocarMochila()
    {
        if (estaPausado == true)
        {
            // --- CERRAR LA MOCHILA ---
            estaPausado = false;
            Time.timeScale = 1f;
            todoElMenuPausa.SetActive(false);
            imagenMochila.sprite = mochilaCerrada;

            // Reproducimos el sonido de cierre
            if (sonidoCerrar != null) fuenteAudioUI.PlayOneShot(sonidoCerrar);
        }
        else
        {
            // --- ABRIR LA MOCHILA ---
            estaPausado = true;
            Time.timeScale = 0f; // ¡Congelamos el juego!
            todoElMenuPausa.SetActive(true);
            imagenMochila.sprite = mochilaAbierta;

            // Reproducimos el sonido de apertura (sonará gracias al ignoreListenerPause)
            if (sonidoAbrir != null) fuenteAudioUI.PlayOneShot(sonidoAbrir);
        }
    }

    // ==========================================
    // --- FUNCIÓN DEL BOTÓN REINICIAR ---
    // ==========================================
    public void BotonReiniciarConEfecto()
    {
        StartCoroutine(RutinaReiniciar());
    }

    private IEnumerator RutinaReiniciar()
    {
        // 1. Volvemos a poner el tiempo a 1 para que la animación fluya
        Time.timeScale = 1f;

        // 2. Reproducimos el sonido de "Repetir"
        if (sonidoReiniciar != null && fuenteAudioUI != null)
        {
            fuenteAudioUI.PlayOneShot(sonidoReiniciar);
        }

        // 4. Esperamos el tiempo necesario
        yield return new WaitForSeconds(tiempoEsperaReiniciar);

        // 5. Recargamos la escena actual
        string nombreEscenaActual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nombreEscenaActual);
    }

    // ==========================================
    // --- FUNCIÓN DEL BOTÓN IR AL MENÚ PRINCIPAL
    // ==========================================
    public void BotonIrAlMenu(string nombreEscenaPrincipal)
    {
        StartCoroutine(CargarMenuConRetraso(nombreEscenaPrincipal));
    }

    private IEnumerator CargarMenuConRetraso(string nombreEscena)
    {
        Time.timeScale = 1f; // El tiempo vuelve a la normalidad para cargar la escena

        if (animadorMenu != null) animadorMenu.Play("Pulsado");

        // Tu código original para ir al menú principal
        if (sonidoClic != null)
        {
            GameObject altavozTemporal = new GameObject("EfectoUI_Clic");
            DontDestroyOnLoad(altavozTemporal);
            AudioSource fuenteTemporal = altavozTemporal.AddComponent<AudioSource>();
            fuenteTemporal.spatialBlend = 0f;
            fuenteTemporal.clip = sonidoClic;
            fuenteTemporal.Play();
            Destroy(altavozTemporal, sonidoClic.length);
        }

        yield return new WaitForSeconds(tiempoDeEspera);
        SceneManager.LoadScene(nombreEscena);
    }
}
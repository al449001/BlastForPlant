using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlCartelWin : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject arbol;
    public string nombreEscenaWin = "Win";
    public float tiempoEsperaExtra = 0.5f;

    [Header("Sonidos y Música")]
    public AudioClip sonidoTocarCartel;
    public AudioClip sonidoCrecer;
    public AudioSource musicaDelNivel;

    private Animator animatorArbol;
    private SpriteRenderer spriteArbol;
    private bool yaActivado = false;

    void Start()
    {
        if (arbol != null)
        {
            animatorArbol = arbol.GetComponent<Animator>();
            spriteArbol = arbol.GetComponent<SpriteRenderer>();

            // Ocultamos el árbol al inicio
            if (spriteArbol != null) spriteArbol.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !yaActivado)
        {
            yaActivado = true;
            StartCoroutine(SecuenciaVictoria(collision.gameObject));
        }
    }

    private IEnumerator SecuenciaVictoria(GameObject jugador)
    {
        // 1. Reproducimos el sonido del cartel
        if (sonidoTocarCartel != null)
        {
            AudioSource.PlayClipAtPoint(sonidoTocarCartel, Camera.main.transform.position, 1f);
            yield return new WaitForSeconds(sonidoTocarCartel.length);
        }

        // 2. Paramos la música del nivel
        if (musicaDelNivel != null) musicaDelNivel.Stop();

        // 3. Ocultamos el cartel
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Pausa dramática
        yield return new WaitForSeconds(0.2f);

        // 4. Ocultamos al jugador
        jugador.SetActive(false);

        if (arbol != null)
        {
            // 5. Mostramos el árbol
            if (spriteArbol != null) spriteArbol.enabled = true;

            if (sonidoCrecer != null)
            {
                AudioSource.PlayClipAtPoint(sonidoCrecer, Camera.main.transform.position, 1f);
            }

            // 6. Forzamos la animación desde el frame 0
            if (animatorArbol != null)
            {
                animatorArbol.Play("AnimacionArbol", -1, 0f);
            }

            // 7. CÁLCULO AVANZADO DE TIEMPO
            yield return new WaitForSeconds(0.1f); // Esperamos a que el Animator se actualice

            // Obtenemos la información del estado actual
            AnimatorStateInfo infoEstado = animatorArbol.GetCurrentAnimatorStateInfo(0);

            // Calculamos el tiempo real: Duración original multiplicada por el modificador de velocidad
            // Dividimos entre speedMultiplier por si la velocidad es menor a 1 (ej: 1 / 0.5 = 2 segundos)
            float duracionReal = infoEstado.length / infoEstado.speedMultiplier;

            // Esperamos el tiempo real más el extra que tú decidas
            yield return new WaitForSeconds(duracionReal + tiempoEsperaExtra);
        }

        // 8. Cambio de escena
        SceneManager.LoadScene(nombreEscenaWin);
    }
}
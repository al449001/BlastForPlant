using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlCartelWin : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject arbol;
    public string nombreEscenaWin = "Win";
    public float tiempoEsperaExtra = 0.8f;

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
            // Empezamos con el árbol invisible para que no parpadee
            if (spriteArbol != null) spriteArbol.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !yaActivado)
        {
            yaActivado = true;
            // Bloqueamos el movimiento si tienes un script de control, 
            // o simplemente esperamos a la corrutina
            StartCoroutine(SecuenciaVictoria(collision.gameObject));
        }
    }

    private IEnumerator SecuenciaVictoria(GameObject jugador)
    {
        // 1. SONIDO DEL CARTEL
        if (sonidoTocarCartel != null)
        {
            AudioSource.PlayClipAtPoint(sonidoTocarCartel, Camera.main.transform.position, 1f);
            yield return new WaitForSeconds(sonidoTocarCartel.length * 0.8f); // Espera casi todo el sonido
        }

        // 2. DESAPARECE PERSONAJE Y CARTEL
        jugador.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        if (musicaDelNivel != null) musicaDelNivel.Stop();

        yield return new WaitForSeconds(0.2f); // Mini pausa dramática

        // 3. APARECE EL ÁRBOL Y CRECE
        if (arbol != null)
        {
            if (spriteArbol != null) spriteArbol.enabled = true;

            if (sonidoCrecer != null)
            {
                AudioSource.PlayClipAtPoint(sonidoCrecer, Camera.main.transform.position, 1f);
            }

            // Forzamos el reinicio de la animación por si acaso
            animatorArbol.Play("AnimacionArbol", 0, 0f);
            animatorArbol.SetTrigger("Crecer");

            // Esperamos a que la animación termine de verdad
            yield return new WaitForSeconds(0.1f);
            float duracion = animatorArbol.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(duracion + tiempoEsperaExtra);
        }

        // 4. CAMBIO DE ESCENA
        SceneManager.LoadScene(nombreEscenaWin);
    }
}
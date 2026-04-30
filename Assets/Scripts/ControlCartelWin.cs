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

            // ARREGLO DE ANIMACIÓN: En lugar de apagar el objeto entero (SetActive(false)),
            // solo apagamos el SpriteRenderer. Así el Animator sigue "despierto" y 
            // no dará el error de "Animator Controller is not valid".
            if (spriteArbol != null) spriteArbol.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ARREGLO DE ORDEN: Detectamos al jugador, pero no lo apagamos aún.
        if (collision.CompareTag("Player") && !yaActivado)
        {
            yaActivado = true;
            StartCoroutine(SecuenciaVictoria(collision.gameObject));
        }
    }

    private IEnumerator SecuenciaVictoria(GameObject jugador)
    {
        // 1. SONIDO DEL CARTEL Y ESPERA
        if (sonidoTocarCartel != null)
        {
            AudioSource.PlayClipAtPoint(sonidoTocarCartel, Camera.main.transform.position, 1f);
            yield return new WaitForSeconds(sonidoTocarCartel.length);
        }

        // 2. CORTAMOS LA MÚSICA DEL NIVEL
        if (musicaDelNivel != null)
        {
            musicaDelNivel.Stop();
        }

        // 3. APAGAMOS EL CARTEL
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // --- PAUSA DRAMÁTICA DE MEDIO SEGUNDO ANTES DEL ÁRBOL ---
        yield return new WaitForSeconds(0.5f);

        // 4. ARREGLO DE PERSONAJE: El personaje desaparece JUSTO ANTES que el árbol.
        jugador.SetActive(false);

        if (arbol != null)
        {
            // 5. APARECE EL ÁRBOL (Hacemos visible su SpriteRenderer)
            if (spriteArbol != null) spriteArbol.enabled = true;

            // 6. SONIDO DEL ÁRBOL AL VOLUMEN MÁXIMO (1f)
            if (sonidoCrecer != null)
            {
                AudioSource.PlayClipAtPoint(sonidoCrecer, Camera.main.transform.position, 1f);
            }

            // --- TRUCO FINAL PARA LA ANIMACIÓN: Forzamos al Animator a reproducir 
            // la animación de crecer desde el frame 0. Esto arregla los problemas de frames congelados.
            if (animatorArbol != null)
            {
                animatorArbol.Play("AnimacionArbol", 0, 0f);
            }

            // 7. ESPERA A QUE TERMINE LA ANIMACIÓN
            yield return new WaitForSeconds(0.1f); // Pequeña espera para que Unity cargue la animación
            float duracionAnimacion = animatorArbol.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(duracionAnimacion + tiempoEsperaExtra);
        }

        // 8. ¡CAMBIO DE ESCENA!
        SceneManager.LoadScene(nombreEscenaWin);
    }
}
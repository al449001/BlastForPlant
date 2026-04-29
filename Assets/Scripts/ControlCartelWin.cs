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
    public AudioSource musicaDelNivel; // <--- AQUÍ PONDREMOS LA MÚSICA PARA APAGARLA

    private Animator animatorArbol;
    private bool yaActivado = false;

    void Start()
    {
        if (arbol != null)
        {
            animatorArbol = arbol.GetComponent<Animator>();
            arbol.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !yaActivado)
        {
            yaActivado = true;
            StartCoroutine(SecuenciaVictoria());
        }
    }

    private IEnumerator SecuenciaVictoria()
    {
        // 1. SONIDO DEL CARTEL Y ESPERA
        if (sonidoTocarCartel != null)
        {
            AudioSource.PlayClipAtPoint(sonidoTocarCartel, Camera.main.transform.position);
            yield return new WaitForSeconds(sonidoTocarCartel.length);
        }

        // 2. ¡CORTAMOS LA MÚSICA DEL NIVEL!
        if (musicaDelNivel != null)
        {
            musicaDelNivel.Stop();
        }

        // 3. APAGAMOS EL CARTEL
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        if (arbol != null)
        {
            // 4. PREPARAMOS EL ÁRBOL
            arbol.transform.position = transform.position;
            arbol.SetActive(true);

            // 5. SONIDO DEL ÁRBOL CRECIENDO
            if (sonidoCrecer != null)
            {
                AudioSource.PlayClipAtPoint(sonidoCrecer, Camera.main.transform.position);
            }

            // Pausa de 1 frame para evitar el bug del Animator dormido
            yield return null;

            // 6. ANIMACIÓN Y ESPERA
            animatorArbol.SetTrigger("Crecer");

            yield return new WaitForSeconds(0.1f);

            float duracionAnimacion = animatorArbol.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(duracionAnimacion + tiempoEsperaExtra);
        }

        // 7. ¡CAMBIO DE ESCENA!
        SceneManager.LoadScene(nombreEscenaWin);
    }
}
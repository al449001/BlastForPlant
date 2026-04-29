using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class ControlCartelWin : MonoBehaviour
{
    [Header("Configuración")]
    public Animator animatorArbol; // Arrastra tu árbol aquí
    public string nombreEscenaWin = "Win"; // El nombre exacto de tu escena
    public float tiempoEsperaExtra = 0.5f; // Unos segundos de margen al acabar

    private bool yaActivado = false; // Evita que se active 20 veces si te quedas quieto en el cartel

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si nos toca el jugador y aún no se ha activado
        if (collision.CompareTag("Player") && !yaActivado)
        {
            yaActivado = true;
            StartCoroutine(SecuenciaVictoria());
        }
    }

    private IEnumerator SecuenciaVictoria()
    {
        // 1. Damos la orden de que empiece la animación del árbol
        animatorArbol.SetTrigger("Crecer");

        // 2. Esperamos una pequeñísima fracción de segundo para que Unity empiece a reproducirla
        yield return new WaitForSeconds(0.1f);

        // 3. Leemos cuánto dura exactamente la animación actual del árbol
        float duracionAnimacion = animatorArbol.GetCurrentAnimatorStateInfo(0).length;

        // 4. Pausamos este script hasta que la animación termine (+ un tiempo extra para que no sea un corte brusco)
        yield return new WaitForSeconds(duracionAnimacion + tiempoEsperaExtra);

        // 5. ¡Cambiamos a la pantalla de victoria!
        SceneManager.LoadScene(nombreEscenaWin);
    }
}
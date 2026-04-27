using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena


public class NavegadorEscenas : MonoBehaviour
{
    [Header("Configuración de Destino")]
    [Tooltip("Escribe aquí el nombre exacto de la escena a la que quieres ir")]
    public string nombreEscenaDestino;

    [Header("Efectos")]
    public Animator animadorBoton;
    public AudioClip sonidoClic;

    [Range(0.1f, 2f)]
    public float tiempoDeEspera = 0.5f;

    private AudioSource miAltavoz;

    private void Awake()
    {
        miAltavoz = GetComponent<AudioSource>();
        miAltavoz.playOnAwake = false; // Que no suene al empezar la escena
    }

    // Esta es la función que llamaremos desde el botón
    public void ClickBoton()
    {
        StartCoroutine(RutinaCambio());
    }

    private IEnumerator RutinaCambio()
    {
        // 1. Ejecutar animación (si hay un animador asignado)
        if (animadorBoton != null)
        {
            animadorBoton.SetTrigger("Pulsar");
        }

        // 2. Ejecutar sonido (si hay un clip asignado)
        if (sonidoClic != null)
        {
            miAltavoz.PlayOneShot(sonidoClic);
        }

        // 3. Esperar a que el jugador vea la animación y oiga el clic
        yield return new WaitForSeconds(tiempoDeEspera);

        // 4. Cambiar a la escena que escribimos en el Inspector
        if (!string.IsNullOrEmpty(nombreEscenaDestino))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }
}
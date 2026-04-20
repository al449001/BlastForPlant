using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioFeedback : MonoBehaviour
{
    [Header("Configuración de Sonido")]
    public AudioClip sonidoClic;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void ReproducirSonidoClic()
    {
        // [Gema de Conocimiento]: El Debug.Log es el mejor amigo del programador.
        // Si este mensaje NO aparece en la consola, significa que el OnClick() 
        // del botón en el Inspector está mal conectado.
        Debug.Log("¡El botón ha sido pulsado y ha llegado al script!");

        if (sonidoClic != null)
        {
            // Si llega aquí, el código y la conexión están perfectos.
            Debug.Log("¡El AudioClip existe, intentando reproducir por el altavoz!");
            audioSource.PlayOneShot(sonidoClic);
        }
        else
        {
            Debug.LogWarning("¡Error! La variable sonidoClic está vacía en el Inspector.");
        }
    }
}

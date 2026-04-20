using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioFeedback : MonoBehaviour
{
    [Header("Configuración de Sonido")]
    [Tooltip("Arrastra aquí el archivo de sonido del clic")]
    public AudioClip sonidoClic;

    //Referencia a altavoz
    private AudioSource audioSource;

    private void Awake()
    {
        //Obtenemos el componente AudioSource que está oculto en este mismo botón
        audioSource = GetComponent<AudioSource>();

        //Nos aseguramos de que el sonido NO suene por accidente nada más cargar el juego
        audioSource.playOnAwake = false;
    }
    public void ReproducirSonidoClic()
    {
        // Comprobamos que haya un sonido asignado para evitar un error grave (NullReferenceException)
        if (sonidoClic != null)
        {
            // Usamos PlayOneShot en lugar de Play() tradicional. 
            // PlayOneShot es ideal para efectos cortos (UI, disparos, pasos),
            // ya que permite que los sonidos se solapen si el jugador hace muchos clics rápidos.
            audioSource.PlayOneShot(sonidoClic);
        }
        else
        {
            //Si olvidaste poner el audio, Unity te avisará amablemente en la consola.
            Debug.LogWarning("¡Falta asignar el AudioClip en el botón: " + gameObject.name + "!");
        }
    }
}

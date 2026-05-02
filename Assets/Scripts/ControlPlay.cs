using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControlPlay : MonoBehaviour
{
    public Animator animadorBoton;
    public float tiempoDeEspera = 0.5f;

    public void PulsarPlay()
    {
        StartCoroutine(RutinaEsperaYCambio());
    }

    private IEnumerator RutinaEsperaYCambio()
    {
        //Activa la animación pulsar del botón
        if (animadorBoton != null) animadorBoton.SetTrigger("Pulsar");
        //Espera medio segundo
        yield return new WaitForSeconds(tiempoDeEspera);
        SceneManager.LoadScene("Intro");
    }
}

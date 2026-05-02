using UnityEngine;
using UnityEngine.UI; // Necesario para el Botón
using UnityEngine.SceneManagement; // Necesario para cargar el Nivel 1

public class IntroStarWars : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 20f;
    public float puntoFinalY = 1500f; // Ajusta esto según lo largo que sea tu texto

    [Header("Referencias UI")]
    public GameObject botonContinuar; // Arrastra aquí tu objeto "Button"

    private bool moviendo = true;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Nos aseguramos de que el botón esté oculto al empezar
        if (botonContinuar != null)
            botonContinuar.SetActive(false);
    }

    void Update()
    {
        if (moviendo)
        {
            // El texto sube
            rectTransform.anchoredPosition += Vector2.up * velocidad * Time.deltaTime;

            // Si llegamos al final de la historia...
            if (rectTransform.anchoredPosition.y >= puntoFinalY)
            {
                TerminarIntro();
            }
        }

        //Si el jugador pulsa Espacio, también termina la intro
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TerminarIntro();
        }
    }

    void TerminarIntro()
    {
        moviendo = false;
        if (botonContinuar != null)
            botonContinuar.SetActive(true); //Aparece el botón de la flecha
    }

    // Esta función la llamará el botón al hacer clic
    public void CargarNivel1()
    {
        SceneManager.LoadScene("Nivel 1"); // Asegúrate de que se llame EXACTAMENTE así
    }
}

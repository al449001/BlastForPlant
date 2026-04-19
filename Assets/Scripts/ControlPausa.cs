using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlPausa : MonoBehaviour
{
    [Header("Tus Objetos")]
    public GameObject menuPausa; // Arrastra aquí tu objeto "MenuPausa" (el botón MENU)
    public Image imagenMochila;  // Arrastra aquí el Image de tu "BotonMochila"
    public Sprite mochilaCerrada;
    public Sprite mochilaAbierta;

    private bool juegoPausado = false;

    void Start()
    {
        // Al empezar, nos aseguramos de que el juego corre
        Time.timeScale = 1f;

        // Ocultamos el botón MENU y cerramos la mochila
        if (menuPausa != null) menuPausa.SetActive(false);
        if (imagenMochila != null) imagenMochila.sprite = mochilaCerrada;
    }

    // Esta función va en el OnClick de la Mochila
    public void AlternarPausa()
    {
        if (juegoPausado)
        {
            ReanudarJuego();
        }
        else
        {
            PausarJuego();
        }
    }

    private void PausarJuego()
    {
        juegoPausado = true;
        Time.timeScale = 0f; // Pausa el juego

        if (menuPausa != null) menuPausa.SetActive(true); // Aparece el botón MENU
        if (imagenMochila != null) imagenMochila.sprite = mochilaAbierta; // Mochila abierta
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        Time.timeScale = 1f; // Reanuda el juego

        if (menuPausa != null) menuPausa.SetActive(false); // Desaparece el botón MENU
        if (imagenMochila != null) imagenMochila.sprite = mochilaCerrada; // Mochila cerrada
    }

    // Esta función va en el OnClick del botón MENU
    public void IrAlMenuPrincipal(string nombreEscena)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscena);
    }
}
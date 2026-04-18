using UnityEngine;
using UnityEngine.UI; //Necesario para cambiar la imagen del botón

public class ControlPausa : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject panelPausa; //El menú oscuro que aparece
    public Image imagenBoton; //Para cambiar el dibujo de la mochila
    public Sprite mochilaCerrada;
    public Sprite mochilaAbierta;

    private bool juegoPausado = false;

    void Start()
    {
        //Al arrancar el nivel, el menú debe estar oculto y el tiempo normal
        if (panelPausa != null) panelPausa.SetActive(false);
        Time.timeScale = 1f;
        imagenBoton.sprite = mochilaCerrada;
    }

    //Esta función la conectaremos al OnClick del botón de la mochila
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
        //Esto congela a los enemigos, al jugador, las físicas y las balas
        Time.timeScale = 0f;

        panelPausa.SetActive(true); //Mostramos la ventana de menú
        imagenBoton.sprite = mochilaAbierta; //Cambiamos el dibujo de la mochila
    }

    private void ReanudarJuego()
    {
        juegoPausado = false;
        //El tiempo vuelve a correr con normalidad
        Time.timeScale = 1f;

        panelPausa.SetActive(false); //Ocultamos la ventana de menú
        imagenBoton.sprite = mochilaCerrada; //Volvemos a la mochila cerrada
    }
}
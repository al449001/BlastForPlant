using UnityEngine;

public class VidaExtra : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ControlPersonaje personaje = collision.GetComponent<ControlPersonaje>();

            if (personaje != null)
            {
                //Comprobamos si el personaje NECESITA curarse
                if (personaje.vidas < personaje.vidasMaximas)
                {
                    //Te cura y el corazón desaparece
                    personaje.GanarVida();
                    Destroy(gameObject);
                }
            }
        }
    }
}

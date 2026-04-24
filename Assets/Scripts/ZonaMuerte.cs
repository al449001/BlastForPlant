using UnityEngine;
using UnityEngine.SceneManagement;

public class ZonaMuerte : MonoBehaviour
{
    public Transform puntoDeRespawn;
    public string escenaGameOver = "GameOver";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ControlPersonaje personaje = collision.GetComponent<ControlPersonaje>();

            if (personaje != null)
            {
                personaje.PerderVida();

                if (personaje.vidas > 0)
                {
                    collision.transform.position = puntoDeRespawn.position;
                }
                else
                {
                    SceneManager.LoadScene(escenaGameOver);
                }
            }
        }
    }
}
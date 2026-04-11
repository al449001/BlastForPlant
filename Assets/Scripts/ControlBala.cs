using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ControlBala : MonoBehaviour
{
    public float velocidad = 15f;
    public float tiempoDeVida = 1.5f; //Esto limita su alcance. Se destruirá en 1.5 segundos.

    void Start()
    {
        //Le damos velocidad hacia la derecha (si el personaje gira, el transform.right girará con él)
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * velocidad;

        //Autodestrucción por si no choca con nada
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si choca contra el enemigo
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Destroy(collision.gameObject); //Destruye al enemigo
            Destroy(gameObject); //Destruye la bala
        }
    }
}

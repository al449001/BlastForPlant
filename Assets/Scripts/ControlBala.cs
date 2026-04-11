using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ControlBala : MonoBehaviour
{
    public float velocidad = 15f;
    public float tiempoDeVida = 1.5f;

    // Guardaremos aquí la dirección (1 para derecha, -1 para izquierda)
    private float direccion = 1f;

    // Esta función la llamará el jugador justo después de crear la bala
    public void ConfigurarDireccion(float dirJugador)
    {
        direccion = dirJugador;

        // Extra: Volteamos el sprite de la bala si vamos a la izquierda
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * direccion;
        transform.localScale = escala;
    }

    void Start()
    {
        //En lugar de usar transform.right, usamos un Vector2 puro multiplicado por la dirección
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(velocidad * direccion, 0f);

        // Autodestrucción
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si choca contra el enemigo
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            // En lugar de destruirlo de golpe, le decimos que ejecute su muerte especial
            ControlEnemigo enemigo = collision.gameObject.GetComponent<ControlEnemigo>();
            if (enemigo != null)
            {
                enemigo.MorirPorDisparo();
            }

            Destroy(gameObject); //Destruye la bala
        }
    }
}
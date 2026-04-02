using UnityEngine;

public class CamaraPrincipal : MonoBehaviour
{
    [Header("Configuración")]
    public Transform objetivo; // Arrastra a tu Jugador aquí en el Inspector
    public float suavizado = 0.125f; // Qué tan suave es el seguimiento (Lerp)
    public Vector3 offset; // Distancia base (ej. Z = -10 para que no atraviese la pantalla)

    [Header("Estado")]
    public bool puedeSeguir = true; // Controla si la cámara debe moverse o no (útil para la Meta)

    void FixedUpdate()
    {
        // Si no hay objetivo asignado o le dijimos que pare al llegar a la meta, no hacemos nada
        if (objetivo == null || !puedeSeguir) return;

        // Calculamos la posición exacta a la que queremos que vaya la cámara
        Vector3 posicionDeseada = objetivo.position + offset;

        // Lerp suaviza el salto matemático entre la posición actual y la deseada
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado);

        // Aplicamos la nueva posición a nuestra CamaraPrincipal
        transform.position = posicionSuavizada;
    }
}

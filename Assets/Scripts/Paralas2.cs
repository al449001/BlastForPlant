using UnityEngine;

// Esto asegura que Unity no te deje añadir este script si no hay un SpriteRenderer
[RequireComponent(typeof(SpriteRenderer))]
public class Paralas2 : MonoBehaviour
{
    private float longitudSprite;
    private float posicionInicialX;
    private float posicionInicialY;

    [Header("Configuración")]
    public Transform camaraTransform;

    [Tooltip("0 = Se mueve con la cámara (cielo). 1 = Estático (primer plano). 0.5 = Mitad de velocidad.")]
    public float efectoParallax;

    [Header("Ajustes de Altura")]
    [Tooltip("Activa esto para que el fondo suba y baje con la cámara.")]
    public bool seguirEnVertical = true;
    [Tooltip("Úsalo para empujar el fondo hacia abajo y que coincida con la cámara.")]
    public float offsetVertical = 0f;

    [Header("Corrección visual")]
    [Tooltip("Cantidad de superposición para tapar las líneas entre imágenes.")]
    public float correccionSolapamiento = 0.05f;

    void Start()
    {
        // Guardamos las posiciones iniciales
        posicionInicialX = transform.position.x;
        posicionInicialY = transform.position.y;

        // Obtenemos el ancho exacto del sprite
        float tamañoReal = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        // Le restamos tu corrección para evitar líneas transparentes
        longitudSprite = tamañoReal - correccionSolapamiento;
    }

    void LateUpdate() // Usamos LateUpdate para que se mueva DESPUÉS de que la cámara se haya movido
    {
        if (camaraTransform == null) return;

        // Calcula cuánto nos hemos movido respecto a la cámara para saber si debemos "reiniciar" el fondo
        float temp = (camaraTransform.position.x * (1 - efectoParallax));

        // Calcula la distancia que debe moverse el fondo
        float distancia = (camaraTransform.position.x * efectoParallax);

        // Ajustamos la altura si decidimos que siga a la cámara verticalmente
        float nuevaPosicionY = posicionInicialY;
        if (seguirEnVertical)
        {
            nuevaPosicionY = camaraTransform.position.y + offsetVertical;
        }

        // Movemos el fondo a su nueva posición
        transform.position = new Vector3(posicionInicialX + distancia, nuevaPosicionY, transform.position.z);

        // ¡AQUÍ ESTÁ LA MAGIA INFINITA!
        // Si la cámara avanzó más allá de la longitud del sprite, movemos la posición inicial hacia adelante
        if (temp > posicionInicialX + longitudSprite)
        {
            posicionInicialX += longitudSprite;
        }
        // Si el jugador retrocede, movemos la posición inicial hacia atrás
        else if (temp < posicionInicialX - longitudSprite)
        {
            posicionInicialX -= longitudSprite;
        }
    }
}
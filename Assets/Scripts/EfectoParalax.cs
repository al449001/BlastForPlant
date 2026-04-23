using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EfectoParalax : MonoBehaviour
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

    // --- NUEVO BLOQUE: Corrección de la línea/costura ---
    [Header("Corrección visual")]
    [Tooltip("Cantidad de superposición para tapar las líneas entre imágenes.")]
    public float correccionSolapamiento = 0.05f;

    void Start()
    {
        // 1. Guardamos las posiciones iniciales (¡Muy importante no perder la Y!)
        posicionInicialX = transform.position.x;
        posicionInicialY = transform.position.y;

        // 2. Calculamos el tamaño real del sprite que estamos usando
        float tamañoReal = GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        // 3. Le restamos una minúscula cantidad para forzar que se solapen y no dejen huecos
        longitudSprite = tamañoReal - correccionSolapamiento;
    }

    void LateUpdate()
    {
        if (camaraTransform == null) return;

        // --- LÓGICA HORIZONTAL ---
        float distancia = (camaraTransform.position.x * efectoParallax);

        // --- LÓGICA VERTICAL ---
        float nuevaPosicionY = posicionInicialY;

        if (seguirEnVertical)
        {
            nuevaPosicionY = camaraTransform.position.y + offsetVertical;
        }

        // Aplicamos la posición
        transform.position = new Vector3(posicionInicialX + distancia, nuevaPosicionY, transform.position.z);
    }
}
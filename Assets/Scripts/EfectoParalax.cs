using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EfectoParalax : MonoBehaviour
{
    private float longitudSprite;
    private float posicionInicialX;

    [Header("Configuración")]
    public Transform camaraTransform;
    [Tooltip("0 = Se mueve con la cámara (cielo). 1 = Estático (primer plano). 0.5 = Mitad de velocidad.")]
    public float efectoParallax;

    void Start()
    {
        posicionInicialX = transform.position.x;
        longitudSprite = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
    }
    void LateUpdate()
    {
        float temp = (camaraTransform.position.x * (1 - efectoParallax));
        float distancia = (camaraTransform.position.x * efectoParallax);

        transform.position = new Vector3(posicionInicialX + distancia, transform.position.y, transform.position.z);

        if (temp > posicionInicialX + longitudSprite)
        {
            posicionInicialX += longitudSprite;
        }
        else if (temp < posicionInicialX - longitudSprite)
        {
            posicionInicialX -= longitudSprite;
        }
    }
}

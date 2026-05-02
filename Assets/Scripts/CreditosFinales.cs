using UnityEngine;
using System.Collections; // Necesario para poder usar Corrutinas

public class CreditosFinales : MonoBehaviour
{
    [Header("Ajustes de los Créditos")]
    public float velocidad = 70f;
    public float limiteY = 3000f;

    [Header("El Botón que aparecerá")]
    [Tooltip("Arrastra aquí tu botón. OJO: Ahora pide un CanvasGroup, no un GameObject")]
    public CanvasGroup grupoBoton; // ¡Cambio clave! Usamos CanvasGroup

    private RectTransform rectTransform;
    private bool subiendo = true;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // 1. En lugar de apagar el objeto con SetActive, lo hacemos "fantasma"
        if (grupoBoton != null)
        {
            grupoBoton.alpha = 0f; // 100% transparente
            grupoBoton.interactable = false; // No se puede interactuar con él
            grupoBoton.blocksRaycasts = false; // No bloquea el ratón
        }
    }

    void Update()
    {
        if (subiendo)
        {
            // El texto sigue subiendo
            rectTransform.anchoredPosition += Vector2.up * velocidad * Time.deltaTime;

            // Comprobamos si el texto ha llegado arriba
            if (rectTransform.anchoredPosition.y >= limiteY)
            {
                MostrarBotonSinLag();
            }
        }

        // Si el jugador pulsa Espacio, se salta los créditos
        if (subiendo && Input.GetKeyDown(KeyCode.Space))
        {
            MostrarBotonSinLag();
        }
    }

    void MostrarBotonSinLag()
    {
        subiendo = false; // Paramos el texto

        // 2. Iniciamos la Gema de aparición suave
        if (grupoBoton != null)
        {
            StartCoroutine(FadeInBoton());
        }
    }

    // --- LA GEMA ---
    // Esta corrutina ejecuta un bucle que va subiendo la opacidad poco a poco
    private IEnumerator FadeInBoton()
    {
        float tiempo = 0f;
        float duracion = 0.5f; // Tarda medio segundo en aparecer completamente

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime; // Vamos sumando el tiempo que pasa

            // Mathf.Lerp calcula la transición exacta entre 0 y 1 según el tiempo
            grupoBoton.alpha = Mathf.Lerp(0f, 1f, tiempo / duracion);

            yield return null; // Pausa hasta el siguiente frame para que sea fluido
        }

        // 3. Nos aseguramos de que al terminar quede perfecto y sea clicable
        grupoBoton.alpha = 1f;
        grupoBoton.interactable = true;
        grupoBoton.blocksRaycasts = true;
    }
}
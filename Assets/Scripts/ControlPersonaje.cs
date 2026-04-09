using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class ControlPersonaje : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 6.5f;

    [Header("Ajustes de Gravedad")]
    public float multiplicadorCaida = 2.5f; //Si el número es más grande, más rápido cae

    [Header("Doble Salto y Suelo")]
    public Transform controladorSuelo; //Un objeto en los pies de tu personaje
    public float radioSuelo = 0.2f;
    public LayerMask EsSuelo; //Para decirle qué es una plataforma

    private Rigidbody2D rb;
    private Animator animator;
    private float movimientoHorizontal;

    private Vector3 escalaInicial;

    private bool enSuelo;
    private bool puedeDobleSalto;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        //Mira si está en el suelo
        enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioSuelo, EsSuelo);
        animator.SetBool("EnSuelo", enSuelo);

        //Esto sirve para saber si está en el suelo para el 2º salto
        if (enSuelo)
        {
            puedeDobleSalto = true;
        }

        //Cambiar animación de caminar
        float velocidadActual = Mathf.Abs(movimientoHorizontal);
        animator.SetFloat("Velocidad", velocidadActual);

        //Girar sprite
        if (movimientoHorizontal > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(escalaInicial.x), escalaInicial.y, escalaInicial.z);
        }
        else if (movimientoHorizontal < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(escalaInicial.x), escalaInicial.y, escalaInicial.z);
        }

        //Lógica del Salto y el doble salto
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (enSuelo)
            {
                EjecutarSalto();
            }
            else if (puedeDobleSalto) // Si cae de un borde o está en el aire y tiene el salto cargado
            {
                EjecutarSalto();
                puedeDobleSalto = false; // Gastamos el doble salto
            }
        }
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (multiplicadorCaida - 1) * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidadMovimiento, rb.linearVelocity.y);
    }

    private void EjecutarSalto()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        animator.SetTrigger("Saltar");
    }

    //Esto dibujará un círculo amarillo en Unity para que veas dónde está el detector
    private void OnDrawGizmos()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(controladorSuelo.position, radioSuelo);
        }
    }
}
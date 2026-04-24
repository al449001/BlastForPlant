using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class ControlPersonaje : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 6.5f;
    public float multiplicadorCaida = 2.5f;

    [Header("Doble Salto y Suelo")]
    public Transform controladorSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask EsSuelo;

    [Header("Sistema de Vidas e Interfaz")]
    public int vidasMaximas = 3;
    public int vidas = 3;
    public Animator BarraVida;
    public TextMeshProUGUI textoVidas;
    public string nombreEscenaGameOver = "GameOver";

    [Header("Efectos de Sonido del Personaje")]
    public AudioClip sonidoDaño;
    public AudioClip sonidoDisparo; // Tu sonido de disparo
    private AudioSource fuenteAudio;

    [Header("Sistema de Disparo Fluido")]
    public GameObject prefabBala;
    public Transform puntoDeDisparo;
    public float tiempoRecarga = 0.25f;
    public float retrasoBala = 0.05f;
    public float tiempoRecuperacion = 0.1f;

    // ¡AQUÍ ESTÁ LA VARIABLE QUE FALTABA!
    private bool estaDisparando = false;

    // Variables internas de control
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool esInvulnerable = false;
    private float tiempoUltimoDisparo = -10f;
    private float movimientoHorizontal;
    private Vector3 escalaInicial;
    private bool enSuelo;
    private bool puedeDobleSalto;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        fuenteAudio = GetComponent<AudioSource>();

        if (fuenteAudio != null)
        {
            fuenteAudio.spatialBlend = 0f;
            fuenteAudio.playOnAwake = false;
        }
    }

    void Start()
    {
        escalaInicial = transform.localScale;
        ActualizarUI();
    }

    void Update()
    {
        // Si está disparando, no le dejamos moverse
        if (estaDisparando)
        {
            movimientoHorizontal = 0f;
            animator.SetFloat("Velocidad", 0f);
        }
        else
        {
            movimientoHorizontal = Input.GetAxisRaw("Horizontal");
            animator.SetFloat("Velocidad", Mathf.Abs(movimientoHorizontal));

            if (movimientoHorizontal > 0) transform.localScale = escalaInicial;
            else if (movimientoHorizontal < 0) transform.localScale = new Vector3(-escalaInicial.x, escalaInicial.y, escalaInicial.z);
        }

        enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioSuelo, EsSuelo);
        animator.SetBool("EnSuelo", enSuelo);

        if (enSuelo) puedeDobleSalto = true;

        if (Input.GetButtonDown("Jump") && !estaDisparando)
        {
            if (enSuelo) EjecutarSalto();
            else if (puedeDobleSalto)
            {
                EjecutarSalto();
                puedeDobleSalto = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Disparar();
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (multiplicadorCaida - 1) * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidadMovimiento, rb.linearVelocity.y);
    }

    private void EjecutarSalto()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        enSuelo = false;
        animator.SetBool("EnSuelo", false);
        animator.SetTrigger("Saltar");
    }

    public void RecibirDaño()
    {
        if (esInvulnerable) return;

        if (sonidoDaño != null && fuenteAudio != null) fuenteAudio.PlayOneShot(sonidoDaño);

        PerderVida();

        if (vidas <= 0) SceneManager.LoadScene(nombreEscenaGameOver);
        else StartCoroutine(RutinaInvulnerabilidad());
    }

    public void PerderVida()
    {
        vidas--;
        ActualizarUI();
    }

    //El método para que la vida extra te cure
    public void GanarVida()
    {
        if (vidas < vidasMaximas) //Si no ha llegado al tope de vida
        {
            vidas++;
            ActualizarUI();
        }
    }

    private IEnumerator RutinaInvulnerabilidad()
    {
        esInvulnerable = true;
        float duracionParpadeo = 2f;
        float tiempoPasad = 0f;

        while (tiempoPasad < duracionParpadeo)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            tiempoPasad += 0.1f;
        }

        spriteRenderer.enabled = true;
        esInvulnerable = false;
    }
    private void Disparar()
    {
        if (Time.time >= tiempoUltimoDisparo + tiempoRecarga)
        {
            estaDisparando = true;

            animator.SetTrigger("Disparar");
            StartCoroutine(EsperarParaDisparar());
            tiempoUltimoDisparo = Time.time;
        }
    }

    private IEnumerator EsperarParaDisparar()
    {
        // 2º: Esperamos a que la animación haga el movimiento de sacar el arma
        // (Este tiempo lo controlas desde el Inspector de Unity con "Retraso Bala")
        yield return new WaitForSeconds(retrasoBala);

        // 3º: ¡AHORA SÍ! Aparece la bala, se mueve, y suena el PUM
        if (sonidoDisparo != null && fuenteAudio != null)
        {
            fuenteAudio.PlayOneShot(sonidoDisparo);
        }

        if (prefabBala != null && puntoDeDisparo != null)
        {
            GameObject balaCreada = Instantiate(prefabBala, puntoDeDisparo.position, transform.rotation) as GameObject;
            float direccion = (transform.localScale.x > 0) ? 1f : -1f;

            ControlBala scriptBala = balaCreada.GetComponent<ControlBala>();
            if (scriptBala != null) scriptBala.velocidad = Mathf.Abs(scriptBala.velocidad) * direccion;

            balaCreada.transform.localScale = new Vector3(Mathf.Abs(balaCreada.transform.localScale.x) * direccion, balaCreada.transform.localScale.y, balaCreada.transform.localScale.z);
        }

        // Esperamos un poquito para que no haga metralleta
        yield return new WaitForSeconds(tiempoRecuperacion);

        estaDisparando = false;
    }

    private void ActualizarUI()
    {
        if (textoVidas != null) textoVidas.text = "Vidas: " + vidas;
        if (BarraVida != null) BarraVida.SetInteger("VidasActuales", vidas);
    }

    private void OnDrawGizmosSelected()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(controladorSuelo.position, radioSuelo);
        }
    }
}
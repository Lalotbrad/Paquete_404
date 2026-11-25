using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    [Header("Controles")]
    public InputActionReference moverse;
    public InputActionReference saltar;

    [Header("Movimiento")]
    public float fuerzaSalto = 5f;
    public Transform checadorSuelo;
    public LayerMask suelo;
    public bool enSuelo;
    public int saltos;
    public float velocidad = 4f;

    [Header("Combate")]
    public float radioGolpe = 1f;
    public int dano = 25;
    public Transform golpe;

    [Header("Animaciones")]
    public Animator animator;

    private Rigidbody2D rb;
    private PoderJugador poderJugador;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        poderJugador = GetComponent<PoderJugador>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void OnDrawGizmosSelected()
    {
        if (golpe != null)
            Gizmos.DrawWireSphere(golpe.position, radioGolpe);
    }

    private void Update()
    {
        if (rb == null) return;

        Vector2 input = moverse != null && moverse.action != null
            ? moverse.action.ReadValue<Vector2>()
            : Vector2.zero;

        rb.linearVelocity = new Vector2(input.x * velocidad, rb.linearVelocity.y);

        // Girar al personaje hacia la dirección en la que se mueve.
        if (input.x > 0.01f)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else if (input.x < -0.01f)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }

        ComprobarSuelo();

        // Doble salto
        if (saltar != null && saltar.action != null &&
            saltar.action.WasPressedThisFrame() && saltos < 2)
        {
            saltos++;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            enSuelo = false;


        }

        // Ataque cuerpo a cuerpo con C
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
            Atacar();

        ActualizarAnimaciones();
    }

    private void ActualizarAnimaciones()
    {
        if (animator == null) return;
        animator.SetFloat("Velocidad", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("EnSuelo", enSuelo);
    }

    private void ComprobarSuelo()
    {
        if (checadorSuelo == null) return;

        RaycastHit2D hit = Physics2D.Raycast(
            checadorSuelo.position, Vector2.down, 0.2f, suelo);

        Debug.DrawRay(checadorSuelo.position, Vector2.down * 0.2f, Color.red);
        enSuelo = hit.collider != null && rb.linearVelocity.y <= 0.01f;

        if (enSuelo)
            saltos = 0;
    }

    private void Atacar()
    {
        if (golpe == null)
        {
            Debug.LogWarning("Jugador: asigna el Transform 'golpe' en el Inspector.");
            return;
        }

        Collider2D[] enemigos = Physics2D.OverlapCircleAll(golpe.position, radioGolpe);
        bool golpeAcertado = false;
        int danoFinal = poderJugador != null ? poderJugador.ObtenerDano(dano) : dano;

        foreach (Collider2D enemigo in enemigos)
        {
            VidaEnemigo vidaEnemigo = enemigo.GetComponentInParent<VidaEnemigo>();

            if (vidaEnemigo != null && vidaEnemigo.VidaActual > 0)
            {
                vidaEnemigo.DanoRecibido(danoFinal);
                golpeAcertado = true;
                Debug.Log("Golpe acertado. Daño: " + danoFinal);
                break;
            }
        }

        // Seleccionar la animación antes de consumir el poder.
        if (animator != null)
            animator.SetTrigger(poderJugador != null && poderJugador.PoderActivo()
                ? "GolpePoder" : "Golpear");

        // Conserva el comportamiento de carga/consumo del poder.
        if (poderJugador != null && (golpeAcertado || poderJugador.PoderActivo()))
            poderJugador.GolpeAcertado();
    }

}

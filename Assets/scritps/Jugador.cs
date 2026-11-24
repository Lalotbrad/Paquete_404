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
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (input.x < -0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);

        ActualizarAnimaciones(input);

        // Doble salto
        if (saltar != null && saltar.action != null &&
            saltar.action.WasPressedThisFrame() && saltos < 2)
        {
            saltos++;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            enSuelo = false;

            SetBool("EnSuelo", false);
            SetTrigger("Saltar");
        }

        // Ataque cuerpo a cuerpo con Q
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
            Atacar();

        ComprobarSuelo();
    }

    private void ActualizarAnimaciones(Vector2 input)
    {
        bool estaCaminando = Mathf.Abs(input.x) > 0.01f;
        bool estaEnNahual = poderJugador != null && poderJugador.PoderActivo();

        SetBool("Correr", estaCaminando);
        SetBool("EnSuelo", enSuelo);
        SetFloat("VelY", rb.linearVelocity.y);
        SetBool("Nahual", estaEnNahual);
    }

    private void ComprobarSuelo()
    {
        if (checadorSuelo == null) return;

        RaycastHit2D hit = Physics2D.Raycast(
            checadorSuelo.position, Vector2.down, 0.2f, suelo);

        Debug.DrawRay(checadorSuelo.position, Vector2.down * 0.2f, Color.red);
        enSuelo = hit.collider != null;

        if (enSuelo && rb.linearVelocity.y <= 0.01f)
        {
            saltos = 0;
            SetBool("EnSuelo", true);
        }
        else
        {
            SetBool("EnSuelo", false);
        }
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

        // Animación de ataque: normal o nahual.
        if (poderJugador != null && poderJugador.PoderActivo())
            SetTrigger("MorderNahual");
        else
            SetTrigger("Proyectada");

        // Conserva el comportamiento de carga/consumo del poder.
        if (poderJugador != null && (golpeAcertado || poderJugador.PoderActivo()))
            poderJugador.GolpeAcertado();
    }

    private void SetBool(string parametro, bool valor)
    {
        if (animator != null && TieneParametro(parametro, AnimatorControllerParameterType.Bool))
            animator.SetBool(parametro, valor);
    }

    private void SetFloat(string parametro, float valor)
    {
        if (animator != null && TieneParametro(parametro, AnimatorControllerParameterType.Float))
            animator.SetFloat(parametro, valor);
    }

    private void SetTrigger(string parametro)
    {
        if (animator != null && TieneParametro(parametro, AnimatorControllerParameterType.Trigger))
            animator.SetTrigger(parametro);
    }

    private bool TieneParametro(string parametro, AnimatorControllerParameterType tipo)
    {
        foreach (AnimatorControllerParameter p in animator.parameters)
            if (p.name == parametro && p.type == tipo)
                return true;
        return false;
    }
}

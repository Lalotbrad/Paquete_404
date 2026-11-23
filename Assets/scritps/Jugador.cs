using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    public InputActionReference moverse;
    public InputActionReference saltar;
    public float fuerzaSalto = 5f;
    public Transform checadorSuelo;
    public LayerMask suelo;
    public bool enSuelo;
    public int saltos;
    public float velocidad = 4f;
    public float radioGolpe = 1f;
    public int dano = 25;
    Rigidbody2D rb;

    public Transform golpe;

    private PoderJugador poderJugador;

    Animator animator;
    void Awake()
    {
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        poderJugador = GetComponent<PoderJugador>();
    }

    private void OnDrawGizmos()
    {
        if (golpe != null)
        {
            Gizmos.DrawWireSphere(golpe.position, radioGolpe);
        }
    }

    void Update()
    {
        Vector2 input = moverse.action.ReadValue<Vector2>();

        rb.linearVelocity = new Vector2(input.x * velocidad, rb.linearVelocity.y);

        if (input.x > 0) transform.localScale = new Vector3(1, 1, 1);

        else if (input.x < 0) transform.localScale = new Vector3(-1, 1, 1);

        // animator.SetBool("Correr", input.x != 0);    error no usar

        if (saltar.action.WasPressedThisFrame() && saltos < 2)
        {
            saltos++;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            //animator.SetTrigger("Saltar");
            enSuelo = false;
        }

        /* if (!enSuelo && rb.linearVelocity.y < 0)
         {
             animator.SetBool("Caer", true);
         }
         else
         {
             animator.SetBool("Caer", false);
         }*/

        // animator.SetBool("Correr", input.x != 0);

        RaycastHit2D hit = Physics2D.Raycast(checadorSuelo.position, Vector2.down, 0.2f, suelo);


        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            Collider2D[] enemigos = Physics2D.OverlapCircleAll(
                golpe.position,
                radioGolpe
            );

            bool golpeAcertado = false;

            int danoFinal = dano;

            if (poderJugador != null)
            {
                danoFinal = poderJugador.ObtenerDano(dano);
            }

            foreach (Collider2D enemigo in enemigos)
            {
                VidaEnemigo vidaEnemigo =
                    enemigo.GetComponentInParent<VidaEnemigo>();

                if (vidaEnemigo != null && vidaEnemigo.VidaActual > 0)
                {
                    vidaEnemigo.DanoRecibido(danoFinal);

                    golpeAcertado = true;

                    Debug.Log("Golpe acertado. Daño: " + danoFinal);

                    break;
                }
            }

            if (poderJugador != null)
            {
                if (golpeAcertado || danoFinal == poderJugador.DanoPotenciado)
                {
                    poderJugador.GolpeAcertado();
                }
            }
        }


        Debug.DrawRay(checadorSuelo.position, Vector2.down * 0.2f, Color.red);

        enSuelo = hit.collider != null;

        if (enSuelo && rb.linearVelocity.y <= 0)
        {
            saltos = 0;
        }
    }
}

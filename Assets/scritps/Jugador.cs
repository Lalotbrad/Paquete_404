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
    Rigidbody2D rb;

    public Transform golpe;

    public GameObject vidaE;
    private Enemigo1 vidaEnemigo;

    Animator animator;
    void Awake()
    {
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        vidaEnemigo = vidaE.GetComponent<Enemigo1>();
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(golpe.position, radioGolpe);
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

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            RaycastHit2D Golpe = Physics2D.CircleCast(golpe.position, radioGolpe, Vector2.right, 2f);
            if(Golpe.collider != null)
            {
                Debug.Log("Le pegaste a " + Golpe.collider.name);
                vidaEnemigo.vida = vidaEnemigo.vida - 10;
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

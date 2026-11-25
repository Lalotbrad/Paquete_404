using UnityEngine;

public class EnemigoZombi : MonoBehaviour
{
    [Header("Movimiento")]
    public float Velocidad = 2f;
    public Transform Jugador;

    [Header("Ataque")]
    public int Dano = 10;
    public float DistanciaAtaque = 1f;
    public float TiempoEntreAtaques = 1.5f;

    private Rigidbody2D rb;
    private JugadorVida vidaJugador;
    private float contadorAtaque = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Buscar automáticamente al jugador
        if (Jugador == null)
        {
            GameObject objetoJugador =
                GameObject.FindGameObjectWithTag("Player");

            if (objetoJugador != null)
            {
                Jugador = objetoJugador.transform;
            }
        }

        // Obtener el componente de vida del jugador
        if (Jugador != null)
        {
            vidaJugador = Jugador.GetComponent<JugadorVida>();
        }
    }

    private void FixedUpdate()
    {
        if (Jugador == null || rb == null)
            return;

        // Calcular la distancia entre el zombi y el jugador
        float distancia = Vector2.Distance(
            transform.position,
            Jugador.position
        );

        // Obtener la dirección horizontal del jugador
        float direccion = Jugador.position.x -
                          transform.position.x;

        // Girar al zombi sin modificar su tamaño
        if (Mathf.Abs(direccion) > 0.01f)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x) *
                Mathf.Sign(direccion),
                transform.localScale.y,
                transform.localScale.z
            );
        }

        // Reducir el tiempo de espera entre ataques
        contadorAtaque -= Time.fixedDeltaTime;

        if (distancia > DistanciaAtaque)
        {
            PerseguirJugador(direccion);
        }
        else
        {
            // Detener al zombi cuando esté cerca
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            AtacarJugador();
        }
    }

    private void PerseguirJugador(float direccion)
    {
        // Evitar movimientos innecesarios en el eje X
        float movimiento = Mathf.Abs(direccion) > 0.05f
            ? Mathf.Sign(direccion) * Velocidad
            : 0f;

        // Conservar la gravedad del enemigo
        rb.linearVelocity = new Vector2(
            movimiento,
            rb.linearVelocity.y
        );
    }

    private void AtacarJugador()
    {
        // Comprobar el tiempo entre ataques
        if (contadorAtaque > 0f || vidaJugador == null)
            return;

        // Restar vida al jugador
        vidaJugador.TakeDamage(Dano);

        // Reiniciar el contador de ataque
        contadorAtaque = TiempoEntreAtaques;

        Debug.Log("El zombi atacó al jugador");
    }

    private void OnDrawGizmosSelected()
    {
        // Mostrar la distancia de ataque
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            DistanciaAtaque
        );
    }
}

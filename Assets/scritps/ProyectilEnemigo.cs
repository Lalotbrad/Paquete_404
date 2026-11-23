using UnityEngine;

public class ProyectilEnemigo : MonoBehaviour
{
    [Header("Configuración del proyectil")]
    public float Velocidad = 7f;
    public int Dano = 20;
    public float TiempoVida = 5f;

    private Rigidbody2D rb;
    private bool direccionAsignada = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, TiempoVida);
    }

    public void EstablecerDireccion(Vector2 direccion)
    {
        if (rb == null)
            return;

        if (direccion.sqrMagnitude <= 0.001f)
            return;

        direccion = direccion.normalized;

        rb.linearVelocity = direccion * Velocidad;

        // Girar visualmente el proyectil
        float angulo = Mathf.Atan2(
            direccion.y,
            direccion.x
        ) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angulo);

        direccionAsignada = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!direccionAsignada)
            return;

        // Ignorar otros enemigos
        if (collision.GetComponentInParent<EnemigoBowser>() != null)
            return;

        JugadorVida jugador =
            collision.GetComponentInParent<JugadorVida>();

        if (jugador != null)
        {
            jugador.TakeDamage(Dano);

            Debug.Log("El proyectil dañó al jugador");

            Destroy(gameObject);
            return;
        }

        // Destruir al chocar contra el escenario
        if (!collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}

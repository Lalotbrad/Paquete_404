using UnityEngine;
using System.Collections;

public class EnemigoBowser : MonoBehaviour
{
    [Header("Movimiento")]
    public float Velocidad = 2f;
    public float DistanciaPatrulla = 3f;

    [Header("Saltos")]
    public float FuerzaSalto = 6f;
    public float TiempoEntreSaltos = 3f;
    public Transform ChecadorSuelo;
    public LayerMask CapaSuelo;
    public float RadioSuelo = 0.2f;

    [Header("Disparo aleatorio")]
    public GameObject PrefabProyectil;
    public Transform PuntoDisparo;
    public float TiempoEntreDisparos = 2f;

    [Header("Jugador")]
    public Transform Jugador;

    private Rigidbody2D rb;
    private Vector2 posicionInicial;
    private float direccion = -1f;
    private bool enSuelo;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        posicionInicial = rb.position;

        if (Jugador == null)
        {
            GameObject objetoJugador =
                GameObject.FindGameObjectWithTag("Player");

            if (objetoJugador != null)
                Jugador = objetoJugador.transform;
        }

        StartCoroutine(SistemaDisparo());
        StartCoroutine(SistemaSaltos());
    }

    private void FixedUpdate()
    {
        MoverEnemigo();
    }

    private void MoverEnemigo()
    {
        // Patrullar alrededor de la posición inicial
        float limiteIzquierdo =
            posicionInicial.x - DistanciaPatrulla;

        float limiteDerecho =
            posicionInicial.x + DistanciaPatrulla;

        if (rb.position.x <= limiteIzquierdo)
            direccion = 1f;

        else if (rb.position.x >= limiteDerecho)
            direccion = -1f;

        // Mantener la velocidad vertical para permitir saltos
        rb.linearVelocity = new Vector2(
            direccion * Velocidad,
            rb.linearVelocity.y
        );

        // Girar el sprite según la dirección
        Vector3 escala = transform.localScale;

        escala.x = Mathf.Abs(escala.x) * direccion;

        transform.localScale = escala;
    }

    private IEnumerator SistemaSaltos()
    {
        while (true)
        {
            yield return new WaitForSeconds(TiempoEntreSaltos);

            enSuelo = Physics2D.OverlapCircle(
                ChecadorSuelo.position,
                RadioSuelo,
                CapaSuelo
            ) != null;

            if (enSuelo)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    FuerzaSalto
                );

                Debug.Log("El enemigo saltó");
            }
        }
    }

    private IEnumerator SistemaDisparo()
    {
        while (true)
        {
            yield return new WaitForSeconds(TiempoEntreDisparos);

            // Generar un número aleatorio entre 1 y 6
            int numero = Random.Range(1, 7);

            Debug.Log("Número aleatorio: " + numero);

            // Disparar únicamente si sale 5 o 6
            if (numero == 5 || numero == 6)
            {
                Disparar();
            }
        }
    }

    private void Disparar()
    {
        if (PrefabProyectil == null ||
            PuntoDisparo == null ||
            Jugador == null)
        {
            return;
        }

        GameObject bala = Instantiate(
            PrefabProyectil,
            PuntoDisparo.position,
            Quaternion.identity
        );

        ProyectilEnemigo proyectil =
            bala.GetComponent<ProyectilEnemigo>();

        if (proyectil != null)
        {
            Vector2 direccionDisparo =
                (Jugador.position - PuntoDisparo.position).normalized;

            proyectil.EstablecerDireccion(direccionDisparo);
        }

        Debug.Log("¡El enemigo disparó!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 centro = Application.isPlaying
            ? new Vector3(
                posicionInicial.x,
                transform.position.y,
                transform.position.z
            )
            : transform.position;

        Gizmos.DrawLine(
            centro + Vector3.left * DistanciaPatrulla,
            centro + Vector3.right * DistanciaPatrulla
        );

        if (ChecadorSuelo != null)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(
                ChecadorSuelo.position,
                RadioSuelo
            );
        }
    }
}

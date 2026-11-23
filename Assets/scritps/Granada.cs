
using UnityEngine;
using System.Collections.Generic;

public class Granada : MonoBehaviour
{
    [Header("Configuración de la granada")]
    public float TiempoExplosion = 3f;
    public float RadioExplosion = 3f;
    public int Dano = 50;

    [Header("Fuerza de lanzamiento")]
    public float FuerzaHorizontal = 8f;
    public float FuerzaVertical = 5f;

    [Header("Capas que pueden recibir daño")]
    public LayerMask CapasDano;

    private Rigidbody2D rb;
    private bool exploto = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Invoke(nameof(Explotar), TiempoExplosion);
    }

    public void Lanzar(float direccion)
    {
        if (rb == null)
            return;

        rb.linearVelocity = new Vector2(
            direccion * FuerzaHorizontal,
            FuerzaVertical
        );
    }

    private void Explotar()
    {
        if (exploto)
            return;

        exploto = true;

        Collider2D[] objetos = Physics2D.OverlapCircleAll(
            transform.position,
            RadioExplosion,
            CapasDano
        );

        HashSet<VidaEnemigo> enemigosGolpeados = new HashSet<VidaEnemigo>();
        HashSet<JugadorVida> jugadoresGolpeados = new HashSet<JugadorVida>();

        foreach (Collider2D objeto in objetos)
        {
            VidaEnemigo enemigo = objeto.GetComponentInParent<VidaEnemigo>();

            if (enemigo != null && enemigosGolpeados.Add(enemigo))
            {
                enemigo.DanoRecibido(Dano);
            }

            JugadorVida jugador = objeto.GetComponentInParent<JugadorVida>();

            if (jugador != null && jugadoresGolpeados.Add(jugador))
            {
                jugador.TakeDamage(Dano);
            }
        }

        Debug.Log("¡La granada explotó!");

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, RadioExplosion);
    }
}

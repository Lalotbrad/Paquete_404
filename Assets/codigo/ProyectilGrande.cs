using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProyectilGrande : MonoBehaviour
{
    [Header("Granada")]
    public int danio = 20;
    public float tiempoParaExplotar = 1.5f;
    public float radioExplosion = 2f;
    public float alturaParabola = 3f;

    private Rigidbody2D rb;
    private bool detonacionIniciada;
    private bool yaExploto;

    private void Awake() { rb = GetComponent<Rigidbody2D>(); }

    public void LanzarEnParabola(Vector2 destino, float altura)
    {
        // Asegura que el proyectil use gravedad (evita que se vaya hacia arriba).
        rb.bodyType = RigidbodyType2D.Dynamic;
        if (rb.gravityScale <= 0.01f) rb.gravityScale = 1f;

        alturaParabola = Mathf.Max(0.1f, altura);
        float g = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        if (g < 0.01f) g = 9.81f;

        float dy = destino.y - transform.position.y;
        float alturaNecesaria = Mathf.Max(alturaParabola, dy + 0.1f);
        float vy = Mathf.Sqrt(2f * g * alturaNecesaria);
        float discriminante = vy * vy - 2f * g * dy;
        float tiempo = (vy + Mathf.Sqrt(Mathf.Max(0f, discriminante))) / g;
        tiempo = Mathf.Max(0.05f, tiempo);

        float vx = (destino.x - transform.position.x) / tiempo;
        rb.linearVelocity = new Vector2(vx, vy);
    }

    // Compatibilidad con el método anterior.
    public void Lanzar(Vector2 direccion, float velocidadNueva)
    {
        LanzarEnParabola((Vector2)transform.position + direccion.normalized * velocidadNueva,
                         alturaParabola);
    }

    private void OnTriggerEnter2D(Collider2D other) { ProcesarImpacto(other); }
    private void OnCollisionEnter2D(Collision2D collision) { ProcesarImpacto(collision.collider); }

    private void ProcesarImpacto(Collider2D other)
    {
        if (other == null || detonacionIniciada || yaExploto) return;
        if (other.GetComponentInParent<JefeFinal>() != null) return;
        if (other.GetComponentInParent<ProyectilGrande>() != null) return;
        if (other.GetComponentInParent<ProyectilPequeno>() != null) return;

        IniciarDetonacion();
    }

    private void IniciarDetonacion()
    {
        if (detonacionIniciada) return;
        detonacionIniciada = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        Invoke(nameof(Explotar), tiempoParaExplotar);
    }

    private void Explotar()
    {
        if (yaExploto) return;
        yaExploto = true;

        Collider2D[] objetivos = Physics2D.OverlapCircleAll(transform.position, radioExplosion);
        foreach (Collider2D objetivo in objetivos)
        {
            JugadorVida vidaJugador = objetivo.GetComponentInParent<JugadorVida>();
            if (vidaJugador != null)
            {
                vidaJugador.TakeDamage(danio);
                break;
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}
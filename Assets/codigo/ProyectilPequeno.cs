using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProyectilPequeno : MonoBehaviour
{
    public int danio = 10;
    public float alturaParabola = 2.2f;
    public float tiempoVida = 8f;

    private Rigidbody2D rb;
    private bool impactado;

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
        Destroy(gameObject, tiempoVida);
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
        if (other == null || impactado) return;
        if (other.GetComponentInParent<JefeFinal>() != null) return;
        if (other.GetComponentInParent<ProyectilGrande>() != null) return;
        if (other.GetComponentInParent<ProyectilPequeno>() != null) return;

        JugadorVida jugador = other.GetComponentInParent<JugadorVida>();
        if (jugador != null)
        {
            impactado = true;
            jugador.TakeDamage(danio);
            Destroy(gameObject);
            return;
        }

        if (!other.isTrigger)
        {
            impactado = true;
            Destroy(gameObject);
        }
    }
}
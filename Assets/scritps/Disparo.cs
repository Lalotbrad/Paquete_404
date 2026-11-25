using UnityEngine;
using UnityEngine.InputSystem;

public class Disparo : MonoBehaviour
{
    [Header("Prefab y posición")]
    public GameObject Proyectil;
    public Transform PuntoDisparo;
    public Transform Jugador;

    [Header("Animación")]
    public Animator animator;
    private PoderJugador poderJugador;

    private void Awake()
    {
        if (Jugador == null)
            Jugador = transform;
        if (animator == null)
            animator = Jugador.GetComponent<Animator>();
        poderJugador = GetComponent<PoderJugador>();
    }

    private void Update()
    {
        // Bloquear el disparo durante la fase de poder
        if (poderJugador != null && poderJugador.PoderActivo())
            return;

        // Disparar únicamente en estado normal
        if (Keyboard.current != null &&
            Keyboard.current.xKey.wasPressedThisFrame)
        {
            Disparar();
        }
    }

    private void Disparar()
    {
        if (Proyectil == null || PuntoDisparo == null || Jugador == null)
        {
            Debug.LogWarning("Disparo: configura Proyectil, PuntoDisparo y Jugador.");
            return;
        }

        GameObject bala = Instantiate(Proyectil, PuntoDisparo.position, Quaternion.identity);
        Proyectil proyectil = bala.GetComponent<Proyectil>();

        if (proyectil != null && proyectil.rb != null)
        {
            float direccion = Jugador.localScale.x >= 0 ? 1f : -1f;
            proyectil.rb.linearVelocity = new Vector2(direccion * proyectil.Velocidad, 0f);
            bala.transform.right = new Vector2(direccion, 0f);
        }
        else
        {
            Debug.LogWarning("El prefab del proyectil necesita el script Proyectil y Rigidbody2D asignado.");
        }

        if (animator != null)
            animator.SetTrigger("Disparar");
    }
}
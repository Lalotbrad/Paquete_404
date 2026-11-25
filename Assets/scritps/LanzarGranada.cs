using UnityEngine;
using UnityEngine.InputSystem;

public class LanzarGranada : MonoBehaviour
{
    [Header("Prefab de la granada")]
    public GameObject PrefabGranada;

    [Header("Punto desde donde se lanza")]
    public Transform PuntoLanzamiento;

    [Header("Animación")]
    public Animator animator;
    private PoderJugador poderJugador;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        poderJugador = GetComponent<PoderJugador>();

    }

    private void Update()
    {
        // Bloquear el lanzamiento durante la fase de poder
        if (poderJugador != null && poderJugador.PoderActivo())
            return;

        // Lanzar únicamente en estado normal
        if (Keyboard.current != null &&
            Keyboard.current.xKey.wasPressedThisFrame)
        {
            Lanzar();
        }
    }

    private void Lanzar()
    {
        if (PrefabGranada == null || PuntoLanzamiento == null)
        {
            Debug.LogWarning("Falta configurar la granada o el punto de lanzamiento.");
            return;
        }

        GameObject objetoGranada = Instantiate(
            PrefabGranada, PuntoLanzamiento.position, Quaternion.identity);

        Granada granada = objetoGranada.GetComponent<Granada>();
        if (granada != null)
        {
            float direccion = transform.localScale.x >= 0 ? 1f : -1f;
            granada.Lanzar(direccion);
        }
        else
        {
            Debug.LogWarning("El prefab de granada no tiene el script Granada.");
        }

        // Reproducir la animación del jugador.
        if (animator != null)
            animator.SetTrigger("LanzarGranada");
    }
}


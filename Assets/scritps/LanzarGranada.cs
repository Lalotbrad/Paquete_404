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

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
            Lanzar();
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

        // Se dispara al lanzar la granada. Si Explosion es solo la animación
        // del objeto explosivo, mueve este trigger al script de la granada.
        if (animator != null && TieneTrigger("Explosion"))
            animator.SetTrigger("Explosion");
    }

    private bool TieneTrigger(string nombre)
    {
        foreach (AnimatorControllerParameter p in animator.parameters)
            if (p.name == nombre && p.type == AnimatorControllerParameterType.Trigger)
                return true;
        return false;
    }
}


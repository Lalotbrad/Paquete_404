
using UnityEngine;
using UnityEngine.InputSystem;

public class LanzarGranada : MonoBehaviour
{
    [Header("Prefab de la granada")]
    public GameObject PrefabGranada;

    [Header("Punto desde donde se lanza")]
    public Transform PuntoLanzamiento;

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.gKey.wasPressedThisFrame)
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
            PrefabGranada,
            PuntoLanzamiento.position,
            Quaternion.identity
        );

        Granada granada = objetoGranada.GetComponent<Granada>();

        if (granada != null)
        {
            float direccion = transform.localScale.x >= 0 ? 1f : -1f;

            granada.Lanzar(direccion);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class Disparo : MonoBehaviour
{
    public GameObject Proyectil;
    public Transform PuntoDisparo;
    public Transform Jugador;

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.zKey.wasPressedThisFrame)
        {
            Disparar();
        }
    }

    private void Disparar()
    {
        if (Proyectil != null && PuntoDisparo != null && Jugador != null)
        {
            GameObject bala = Instantiate(
                Proyectil,
                PuntoDisparo.position,
                Quaternion.identity
            );

            Proyectil proyectil = bala.GetComponent<Proyectil>();

            if (proyectil != null)
            {
                float direccion = Jugador.localScale.x > 0 ? 1f : -1f;

                proyectil.rb.linearVelocity = new Vector2(
                    direccion * proyectil.Velocidad,
                    0f
                );

                bala.transform.right = new Vector2(direccion, 0f);
            }
        }
    }
}

using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float Velocidad = 15f;
    public int Dano = 25;
    public float TiempoBala = 3f;
    public Rigidbody2D rb;

    private void Start()
    {
        Destroy(gameObject, TiempoBala);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<VidaEnemigo>(out VidaEnemigo enemigo))
        {
            enemigo.DanoRecibido(Dano);
            Destroy(gameObject);
        }
        else if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class ItemColeccionable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto que lo tocó tiene la etiqueta "Player"
        if (collision.CompareTag("Player"))
        {
            // Aquí puedes agregar lógica adicional (sumar vida, puntos, etc.)
            Destroy(gameObject);
        }
    }
}

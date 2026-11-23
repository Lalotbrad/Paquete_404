using UnityEngine;

public class DetectorDialogo : MonoBehaviour
{
    [Header("Enemigo de diálogo")]
    public DialogoEnemigo dialogoEnemigo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (dialogoEnemigo != null)
        {
            dialogoEnemigo.IniciarDialogo(collision.gameObject);
        }
    }
}

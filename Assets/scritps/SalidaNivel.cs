using UnityEngine;
using UnityEngine.SceneManagement;

public class SalidaNivel : MonoBehaviour
{
    [Header("Siguiente nivel")]
    public string NombreSiguienteNivel = "Nivel2";

    private bool cambiandoNivel = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cambiandoNivel)
            return;

        if (!collision.CompareTag("Player"))
            return;

        cambiandoNivel = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.NivelActual++;
        }

        Time.timeScale = 1f;

        SceneManager.LoadScene(NombreSiguienteNivel);
    }
}

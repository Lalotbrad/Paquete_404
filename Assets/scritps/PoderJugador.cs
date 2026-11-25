using UnityEngine;
using TMPro;

public class PoderJugador : MonoBehaviour
{
    [Header("Configuración del poder")]
    public int GolpesParaCargar = 5;
    public int DanoPotenciado = 50;
    public int GolpesConPoder = 4;

    [Header("Interfaz del poder")]
    public TextMeshProUGUI TextoPoder;

    [Header("Animaciones")]
    public Animator animator;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private int golpesAcumulados = 0;
    private int golpesRestantes = 0;
    private bool poderActivo = false;

    private void Start()
    {
        // Recuperar el poder guardado al cambiar de nivel
        if (GameManager.Instance != null)
        {
            golpesAcumulados =
                GameManager.Instance.GolpesAcumulados;

            golpesRestantes =
                GameManager.Instance.GolpesRestantes;

            poderActivo =
                GameManager.Instance.PoderActivo;
        }

        if (animator != null)
            animator.SetBool("PoderActivo", poderActivo);

        ActualizarContador();
    }

    public int ObtenerDano(int danoNormal)
    {
        if (poderActivo)
        {
            return DanoPotenciado;
        }

        return danoNormal;
    }

    public bool PoderActivo()
    {
        return poderActivo;
    }

    public void GolpeAcertado()
    {
        if (poderActivo)
        {
            golpesRestantes--;

            if (golpesRestantes <= 0)
            {
                DesactivarPoder();
            }
        }
        else
        {
            golpesAcumulados++;

            if (golpesAcumulados >= GolpesParaCargar)
            {
                ActivarPoder();
            }
        }

        // Guardar los cambios en el GameManager
        GuardarPoder();

        ActualizarContador();
    }

    private void ActivarPoder()
    {
        poderActivo = true;
        golpesRestantes = GolpesConPoder;

        if (animator != null)
        {
            animator.SetBool("PoderActivo", true);
            animator.SetTrigger("ActivarPoder");
        }

        Debug.Log("¡Poder activado!");
    }

    private void DesactivarPoder()
    {
        poderActivo = false;
        golpesAcumulados = 0;
        golpesRestantes = 0;

        if (animator != null)
            animator.SetBool("PoderActivo", false);

        Debug.Log("Poder terminado. Daño normal restaurado.");
    }

    private void GuardarPoder()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GolpesAcumulados =
                golpesAcumulados;

            GameManager.Instance.GolpesRestantes =
                golpesRestantes;

            GameManager.Instance.PoderActivo =
                poderActivo;
        }
    }

    private void ActualizarContador()
    {
        if (TextoPoder == null)
            return;

        if (poderActivo)
        {
            TextoPoder.text = "     " + golpesRestantes +
                              " / " + GolpesConPoder;
        }
        else
        {
            TextoPoder.text = "     " + golpesAcumulados +
                              " / " + GolpesParaCargar;
        }
    }
}

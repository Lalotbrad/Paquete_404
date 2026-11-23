
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

    private int golpesAcumulados = 0;
    private int golpesRestantes = 0;
    private bool poderActivo = false;

    private void Start()
    {
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

        ActualizarContador();
    }

    private void ActivarPoder()
    {
        poderActivo = true;
        golpesRestantes = GolpesConPoder;

        Debug.Log("¡Poder activado!");
    }

    private void DesactivarPoder()
    {
        poderActivo = false;
        golpesAcumulados = 0;
        golpesRestantes = 0;

        Debug.Log("Poder terminado. Daño normal restaurado.");
    }

    private void ActualizarContador()
    {
        if (TextoPoder == null)
            return;

        if (poderActivo)
        {
            TextoPoder.text = "PODER: " + golpesRestantes + " / " + GolpesConPoder;
        }
        else
        {
            TextoPoder.text = "PODER: " + golpesAcumulados + " / " + GolpesParaCargar;
        }
    }
}

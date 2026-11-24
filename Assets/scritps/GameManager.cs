using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Progreso del jugador")]
    public int VidaActual = 100;
    public int VidaMaxima = 100;

    [Header("Sistema de poder")]
    public int GolpesAcumulados = 0;
    public int GolpesRestantes = 0;
    public bool PoderActivo = false;

    [Header("Arma seleccionada")]
    public int ArmaSeleccionada = 0;
    // 0 = Ninguna
    // 1 = Pistola
    // 2 = Granada

    [Header("Progreso de niveles")]
    public int NivelActual = 1;

    private void Awake()
    {
        // Evitar que existan varios GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Conservar el GameManager entre escenas
        DontDestroyOnLoad(gameObject);
    }

    public void NuevaPartida()
    {
        // Restaurar la vida
        VidaActual = VidaMaxima;

        // Reiniciar el poder
        GolpesAcumulados = 0;
        GolpesRestantes = 0;
        PoderActivo = false;

        // Permitir elegir un arma nuevamente
        ArmaSeleccionada = 0;

        // Reiniciar el progreso
        NivelActual = 1;
    }

    public void ReiniciarDespuesDeMorir()
    {
        // Restaurar la vida
        VidaActual = VidaMaxima;

        // Reiniciar el poder
        GolpesAcumulados = 0;
        GolpesRestantes = 0;
        PoderActivo = false;

        // Olvidar el arma para mostrar el menú
        ArmaSeleccionada = 0;

        // No modificar NivelActual
        // El jugador permanece en el mismo nivel
    }
}


using UnityEngine;

public class SeleccionArma : MonoBehaviour
{
    [Header("Scripts de las armas")]
    public Disparo disparo;
    public LanzarGranada lanzarGranada;

    [Header("Control del jugador")]
    public Jugador jugador;

    [Header("Menú de selección")]
    public GameObject MenuSeleccion;

    private bool armaSeleccionada = false;

    private void Start()
    {
        // Desactivar las armas al iniciar
        disparo.enabled = false;
        lanzarGranada.enabled = false;

        // Comprobar si el jugador ya eligió un arma
        if (GameManager.Instance != null)
        {
            int armaGuardada =
                GameManager.Instance.ArmaSeleccionada;

            if (armaGuardada == 1)
            {
                // Recuperar la pistola
                ElegirPistola();
                return;
            }
            else if (armaGuardada == 2)
            {
                // Recuperar las granadas
                ElegirGranada();
                return;
            }
        }

        // Si no tiene arma, mostrar el menú de selección
        jugador.enabled = false;
        armaSeleccionada = false;

        MenuSeleccion.SetActive(true);

        // Pausar el juego mientras elige un arma
        Time.timeScale = 0f;
    }

    public void ElegirPistola()
    {
        if (armaSeleccionada)
            return;

        armaSeleccionada = true;

        disparo.enabled = true;
        lanzarGranada.enabled = false;

        // Guardar el arma seleccionada
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ArmaSeleccionada = 1;
        }

        ComenzarNivel();
    }

    public void ElegirGranada()
    {
        if (armaSeleccionada)
            return;

        armaSeleccionada = true;

        disparo.enabled = false;
        lanzarGranada.enabled = true;

        // Guardar el arma seleccionada
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ArmaSeleccionada = 2;
        }

        ComenzarNivel();
    }

    private void ComenzarNivel()
    {
        // Activar los controles del jugador
        jugador.enabled = true;

        // Ocultar el menú de selección
        MenuSeleccion.SetActive(false);

        // Reanudar el juego
        Time.timeScale = 1f;
    }
}

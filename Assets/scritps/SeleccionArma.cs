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
        // Desactivar controles al comenzar el nivel
        jugador.enabled = false;
        disparo.enabled = false;
        lanzarGranada.enabled = false;

        // Mostrar el menú de selección
        MenuSeleccion.SetActive(true);

        armaSeleccionada = false;
        Time.timeScale = 0f;
    }

    public void ElegirPistola()
    {
        if (armaSeleccionada)
            return;

        armaSeleccionada = true;

        disparo.enabled = true;
        lanzarGranada.enabled = false;

        ComenzarNivel();
    }

    public void ElegirGranada()
    {
        if (armaSeleccionada)
            return;

        armaSeleccionada = true;

        disparo.enabled = false;
        lanzarGranada.enabled = true;

        ComenzarNivel();
    }

    private void ComenzarNivel()
    {
        jugador.enabled = true;
        MenuSeleccion.SetActive(false);
        Time.timeScale = 1f;
    }
}

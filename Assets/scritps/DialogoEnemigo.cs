using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PreguntaEnemigo
{
    [TextArea(2, 4)]
    public string Dialogo;

    public string RespuestaA;
    public string RespuestaB;
    public string RespuestaC;

    [Range(0, 2)]
    public int RespuestaCorrecta;
}

public class DialogoEnemigo : MonoBehaviour
{
    [Header("Preguntas del enemigo")]
    public PreguntaEnemigo[] Preguntas;

    [Header("Interfaz")]
    public GameObject PanelDialogo;
    public TextMeshProUGUI TextoPregunta;
    public TextMeshProUGUI TextoAciertos;
    public TextMeshProUGUI TextoErrores;

    public Button BotonA;
    public Button BotonB;
    public Button BotonC;

    [Header("Configuración")]
    public int AciertosParaGanar = 3;
    public int ErroresParaPerder = 3;

    private int aciertos = 0;
    private int errores = 0;
    private int preguntaActual = 0;

    private bool dialogoActivo = false;
    private bool terminado = false;

    private JugadorVida vidaJugador;
    private Jugador jugador;
    private Disparo disparo;
    private LanzarGranada granadas;
    private Pausa pausa;

    private bool disparoHabilitado;
    private bool granadasHabilitadas;
    private bool pausaHabilitada;

    private void Start()
    {
        PanelDialogo.SetActive(false);

        BotonA.onClick.AddListener(() => Responder(0));
        BotonB.onClick.AddListener(() => Responder(1));
        BotonC.onClick.AddListener(() => Responder(2));
    }

    public void IniciarDialogo(GameObject objetoJugador)
    {
        if (dialogoActivo || terminado)
            return;

        if (Preguntas == null || Preguntas.Length == 0)
        {
            Debug.LogWarning("El enemigo no tiene preguntas.");
            return;
        }

        vidaJugador = objetoJugador.GetComponent<JugadorVida>();
        jugador = objetoJugador.GetComponent<Jugador>();
        disparo = objetoJugador.GetComponent<Disparo>();
        granadas = objetoJugador.GetComponent<LanzarGranada>();
        pausa = FindFirstObjectByType<Pausa>();

        // Guardar qué controles estaban disponibles
        disparoHabilitado = disparo != null && disparo.enabled;
        granadasHabilitadas = granadas != null && granadas.enabled;
        pausaHabilitada = pausa != null && pausa.enabled;

        dialogoActivo = true;
        aciertos = 0;
        errores = 0;
        preguntaActual = 0;

        // Bloquear los controles durante el diálogo
        if (jugador != null)
            jugador.enabled = false;

        if (disparo != null)
            disparo.enabled = false;

        if (granadas != null)
            granadas.enabled = false;

        if (pausa != null)
            pausa.enabled = false;

        // Detener el movimiento horizontal
        Rigidbody2D rb = objetoJugador.GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        PanelDialogo.SetActive(true);

        // Pausar el mundo mientras se responde
        Time.timeScale = 0f;

        MostrarPregunta();
    }

    private void MostrarPregunta()
    {
        if (!dialogoActivo)
            return;

        if (preguntaActual >= Preguntas.Length)
        {
            Debug.LogWarning("Se terminaron las preguntas.");
            FinalizarDialogo(false);
            return;
        }

        PreguntaEnemigo pregunta = Preguntas[preguntaActual];

        TextoPregunta.text = pregunta.Dialogo;

        BotonA.GetComponentInChildren<TextMeshProUGUI>().text =
            pregunta.RespuestaA;

        BotonB.GetComponentInChildren<TextMeshProUGUI>().text =
            pregunta.RespuestaB;

        BotonC.GetComponentInChildren<TextMeshProUGUI>().text =
            pregunta.RespuestaC;

        ActualizarContadores();
    }

    public void Responder(int respuesta)
    {
        if (!dialogoActivo || terminado)
            return;

        PreguntaEnemigo pregunta = Preguntas[preguntaActual];

        if (respuesta == pregunta.RespuestaCorrecta)
        {
            aciertos++;
            Debug.Log("Respuesta correcta");
        }
        else
        {
            errores++;
            Debug.Log("Respuesta incorrecta");
        }

        ActualizarContadores();

        if (aciertos >= AciertosParaGanar)
        {
            GanarDialogo();
            return;
        }

        if (errores >= ErroresParaPerder)
        {
            PerderDialogo();
            return;
        }

        preguntaActual++;
        MostrarPregunta();
    }

    private void ActualizarContadores()
    {
        TextoAciertos.text = "Aciertos: " + aciertos +
                            " / " + AciertosParaGanar;

        TextoErrores.text = "Errores: " + errores +
                           " / " + ErroresParaPerder;
    }

    private void GanarDialogo()
    {
        Debug.Log("¡El jugador ganó el enfrentamiento!");

        FinalizarDialogo(true);

        // Eliminar al enemigo
        Destroy(gameObject);
    }

    private void PerderDialogo()
    {
        Debug.Log("El jugador perdió el enfrentamiento");

        FinalizarDialogo(false);

        // Quitar toda la vida del jugador
        if (vidaJugador != null)
        {
            vidaJugador.TakeDamage(int.MaxValue);
        }
    }

    private void FinalizarDialogo(bool jugadorGano)
    {
        dialogoActivo = false;
        terminado = true;

        PanelDialogo.SetActive(false);

        Time.timeScale = 1f;

        if (jugadorGano)
        {
            // Restaurar los controles del jugador
            if (jugador != null)
                jugador.enabled = true;

            if (disparo != null)
                disparo.enabled = disparoHabilitado;

            if (granadas != null)
                granadas.enabled = granadasHabilitadas;
        }

        if (pausa != null)
            pausa.enabled = pausaHabilitada;
    }
}

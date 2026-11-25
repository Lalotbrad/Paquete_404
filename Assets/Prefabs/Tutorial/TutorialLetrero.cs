using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;   // el mismo sistema de teclado que usa el Jugador del equipo

// TUTORIAL DE PAQUETE 404 - funciona solo, no usa ni cambia ningún otro script del proyecto.
// Ponlo en un objeto con Box Collider 2D (Is Trigger) en el camino del jugador.
// Cuando el jugador entra, aparece el letrero; se quita cuando presiona la tecla indicada.
public class TutorialLetrero : MonoBehaviour
{
    public Sprite imagen;                         // tu dibujo del letrero (si lo pones, se muestra en vez del texto)
    public float altoImagen = 260f;               // qué tan grande se ve el dibujo en pantalla
    [TextArea] public string texto = "Usa las flechas ← → para moverte";
    public Key tecla = Key.None;                  // la tecla que tiene que presionar (None = ninguna)
    public bool cualquierFlecha = false;          // true = sirve la flecha izquierda o derecha
    public int vecesSeguidas = 1;                 // 2 = presionarla dos veces rápido (doble salto)
    public float segundosSiNoHayTecla = 3f;       // si no hay tecla, el letrero dura esto
    public GameObject activarAlTerminar;          // opcional (por ejemplo, un enemigo de práctica)
    public GameObject desactivarAlTerminar;       // opcional (por ejemplo, una pared que bloquea el paso)

    static Text letrero;
    static GameObject panel;
    static Image fondoPanel;
    static Image dibujo;
    bool activo, terminado;
    int presionadas;
    float ultima, fin;

    void Start()
    {
        if (activarAlTerminar) activarAlTerminar.SetActive(false);
    }

    // Detecta al jugador por su tag "Player".
    bool EsJugador(Collider2D c)
    {
        return c.CompareTag("Player") || (c.attachedRigidbody != null && c.attachedRigidbody.CompareTag("Player"));
    }

    static bool Presiono(Key k)
    {
        Keyboard kb = Keyboard.current;
        return kb != null && k != Key.None && kb[k].wasPressedThisFrame;
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (activo || terminado || !EsJugador(otro)) return;
        activo = true;
        if (imagen != null) MostrarImagen(imagen, altoImagen); else Mostrar(texto);
        fin = Time.time + segundosSiNoHayTecla;
    }

    void Update()
    {
        if (!activo || terminado) return;

        if (tecla == Key.None && !cualquierFlecha)
        {
            if (Time.time >= fin) Terminar();
            return;
        }

        bool presiono = Presiono(tecla) ||
                        (cualquierFlecha && (Presiono(Key.LeftArrow) || Presiono(Key.RightArrow)));
        if (!presiono) return;

        if (Time.time - ultima > 0.6f) presionadas = 0;
        presionadas++;
        ultima = Time.time;
        if (presionadas >= vecesSeguidas) Terminar();
    }

    void Terminar()
    {
        terminado = true;
        Mostrar("¡Bien!");
        Invoke(nameof(Ocultar), 0.8f);
        if (activarAlTerminar) activarAlTerminar.SetActive(true);
        if (desactivarAlTerminar) desactivarAlTerminar.SetActive(false);
    }

    void Ocultar() { if (panel) panel.SetActive(false); if (dibujo) dibujo.gameObject.SetActive(false); }

    // Crea su propio letrero en pantalla (no usa el Canvas de los demás).
    static void Mostrar(string t)
    {
        if (letrero == null)
        {
            GameObject c = new GameObject("TutorialCanvas");
            Canvas canvas = c.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 500;
            CanvasScaler cs = c.AddComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1920, 1080);

            panel = new GameObject("Letrero");
            panel.transform.SetParent(c.transform, false);
            fondoPanel = panel.AddComponent<Image>();
            fondoPanel.color = new Color(0f, 0f, 0f, 0.75f);
            RectTransform pr = panel.GetComponent<RectTransform>();
            pr.anchorMin = new Vector2(0.5f, 0.8f); pr.anchorMax = new Vector2(0.5f, 0.8f);
            pr.sizeDelta = new Vector2(1100, 140);

            GameObject tx = new GameObject("Texto");
            tx.transform.SetParent(panel.transform, false);
            letrero = tx.AddComponent<Text>();
            letrero.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            letrero.fontSize = 46;
            letrero.fontStyle = FontStyle.Bold;
            letrero.alignment = TextAnchor.MiddleCenter;
            letrero.color = Color.white;
            RectTransform tr = tx.GetComponent<RectTransform>();
            tr.anchorMin = Vector2.zero; tr.anchorMax = Vector2.one;
            tr.offsetMin = new Vector2(20, 10); tr.offsetMax = new Vector2(-20, -10);

            GameObject im = new GameObject("Dibujo");
            im.transform.SetParent(c.transform, false);
            dibujo = im.AddComponent<Image>();
            dibujo.preserveAspect = true;
            RectTransform ir = im.GetComponent<RectTransform>();
            ir.anchorMin = new Vector2(0.5f, 0.78f); ir.anchorMax = new Vector2(0.5f, 0.78f);
            im.SetActive(false);
        }
        dibujo.gameObject.SetActive(false);
        panel.SetActive(true);
        letrero.text = t;
    }

    static void MostrarImagen(Sprite s, float alto)
    {
        Mostrar("");
        panel.SetActive(false);
        float ancho = alto * s.rect.width / s.rect.height;
        dibujo.rectTransform.sizeDelta = new Vector2(ancho, alto);
        dibujo.sprite = s;
        dibujo.gameObject.SetActive(true);
    }
}

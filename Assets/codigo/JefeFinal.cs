using UnityEngine;

[RequireComponent(typeof(VidaEnemigo))]
public class JefeFinal : MonoBehaviour
{
    [Header("Prefabs de proyectiles")]
    public GameObject prefabProyectilGrande;
    public GameObject prefabProyectilPequeno;

    [Header("Los tres hijos existentes del jefe")]
    public Transform puntoSuperior;
    public Transform puntoCentral;
    public Transform puntoInferior;

    [Header("Movimiento entre los tres puntos")]
    public float velocidadMovimiento = 3f;
    public float distanciaLlegada = 0.05f;
    public float esperaEnPosicion = 0.4f;
    [Tooltip("Si los puntos están encimados, el jefe sube/baja esta distancia.")]
    public float distanciaRespaldo = 3f;

    [Header("Ataque")]
    public float tiempoEntreAtaques = 1.5f;
    public float distanciaActivacion = 100f;
    public float alturaParabolaGrande = 3f;
    public float alturaParabolaPequena = 2.2f;
    public Transform jugador; // Opcional: puedes arrastrar aquí al jugador.

    private VidaEnemigo vida;
    private Transform[] puntosLanzamiento;
    private Vector3[] destinosGuardados;
    private float temporizadorAtaque;
    private float temporizadorMovimiento;
    private int indiceMovimiento;
    private int indiceLanzamiento;
    private bool lanzarGrande;

    private void Awake()
    {
        vida = GetComponent<VidaEnemigo>();
        temporizadorAtaque = tiempoEntreAtaques;

        // Si el jefe tiene Rigidbody2D, lo volvemos Kinematic para que
        // la física no pelee con el movimiento por transform.
        Rigidbody2D rbJefe = GetComponent<Rigidbody2D>();
        if (rbJefe != null)
        {
            rbJefe.bodyType = RigidbodyType2D.Kinematic;
            rbJefe.linearVelocity = Vector2.zero;
        }

        puntosLanzamiento = new Transform[] { puntoSuperior, puntoCentral, puntoInferior };

        // Guardamos las posiciones mundiales una sola vez.
        destinosGuardados = new Vector3[3];
        bool faltaPunto = false;
        for (int i = 0; i < puntosLanzamiento.Length; i++)
        {
            if (puntosLanzamiento[i] != null)
                destinosGuardados[i] = puntosLanzamiento[i].position;
            else
            {
                faltaPunto = true;
                Debug.LogError("JefeFinal: falta asignar uno de los puntos Superior/Central/Inferior.", this);
            }
        }

        // Respaldo: si faltan puntos o están todos encimados, usa arriba/centro/abajo
        // a partir de la posición inicial del jefe.
        float separacion = Mathf.Abs(destinosGuardados[0].y - destinosGuardados[2].y);
        if (faltaPunto || separacion < 0.5f)
        {
            Vector3 p = transform.position;
            destinosGuardados[0] = p + Vector3.up * distanciaRespaldo;
            destinosGuardados[1] = p;
            destinosGuardados[2] = p + Vector3.down * distanciaRespaldo;
            Debug.LogWarning("JefeFinal: los puntos están encimados; usando movimiento de respaldo arriba/abajo.", this);
        }

        // Empieza hacia el destino más cercano.
        float menorDistancia = Mathf.Infinity;
        for (int i = 0; i < destinosGuardados.Length; i++)
        {
            float distancia = Vector2.Distance(transform.position, destinosGuardados[i]);
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                indiceMovimiento = i;
            }
        }
    }

    private void Update()
    {
        if (vida != null && vida.VidaActual <= 0) return;

        MoverJefe();

        if (jugador == null)
        {
            GameObject objetoJugador = GameObject.FindGameObjectWithTag("Player");
            if (objetoJugador != null)
                jugador = objetoJugador.transform;
        }

        if (jugador == null) return;
        if (Vector2.Distance(transform.position, jugador.position) > distanciaActivacion) return;

        temporizadorAtaque -= Time.deltaTime;
        if (temporizadorAtaque <= 0f)
        {
            Atacar();
            temporizadorAtaque = tiempoEntreAtaques;
        }
    }

    private void MoverJefe()
    {
        if (temporizadorMovimiento > 0f)
        {
            temporizadorMovimiento -= Time.deltaTime;
            return;
        }

        Vector3 destino = destinosGuardados[indiceMovimiento];
        transform.position = Vector3.MoveTowards(
            transform.position,
            destino,
            velocidadMovimiento * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, destino) <= distanciaLlegada)
        {
            transform.position = destino;
            temporizadorMovimiento = esperaEnPosicion;
            indiceMovimiento = (indiceMovimiento + 1) % destinosGuardados.Length;
        }
    }

    private void Atacar()
    {
        Transform punto = ObtenerPuntoLanzamiento();
        Vector3 origen = punto != null ? punto.position : transform.position;

        if (lanzarGrande)
            DispararGrande(origen);
        else
            DispararPequeno(origen);

        lanzarGrande = !lanzarGrande;
        indiceLanzamiento = (indiceLanzamiento + 1) % puntosLanzamiento.Length;
    }

    private Transform ObtenerPuntoLanzamiento()
    {
        for (int i = 0; i < puntosLanzamiento.Length; i++)
        {
            int indice = (indiceLanzamiento + i) % puntosLanzamiento.Length;
            if (puntosLanzamiento[indice] != null)
                return puntosLanzamiento[indice];
        }
        return null;
    }

    private void DispararGrande(Vector3 origen)
    {
        if (prefabProyectilGrande == null)
        {
            Debug.LogError("JefeFinal: asigna el Prefab Proyectil Grande en el Inspector.", this);
            return;
        }

        GameObject objeto = Instantiate(prefabProyectilGrande, origen, Quaternion.identity);
        ProyectilGrande proyectil = objeto.GetComponent<ProyectilGrande>();
        if (proyectil != null)
            proyectil.LanzarEnParabola(jugador.position, alturaParabolaGrande);
        else
            Debug.LogError("El prefab grande no tiene el componente ProyectilGrande.", objeto);
    }

    private void DispararPequeno(Vector3 origen)
    {
        if (prefabProyectilPequeno == null)
        {
            Debug.LogError("JefeFinal: asigna el Prefab Proyectil Pequeño en el Inspector.", this);
            return;
        }

        GameObject objeto = Instantiate(prefabProyectilPequeno, origen, Quaternion.identity);
        ProyectilPequeno proyectil = objeto.GetComponent<ProyectilPequeno>();
        if (proyectil != null)
            proyectil.LanzarEnParabola(jugador.position, alturaParabolaPequena);
        else
            Debug.LogError("El prefab pequeño no tiene el componente ProyectilPequeno.", objeto);
    }
}
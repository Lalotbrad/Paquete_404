using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SistemaHordas : MonoBehaviour
{
    [Header("Prefabs de zombis")]
    public GameObject[] PrefabsZombis;

    [Header("Puntos de aparición")]
    public Transform SpawnIzquierdo;
    public Transform SpawnDerecho;

    [Header("Limites de la zona")]
    public GameObject LimiteIzquierdo;
    public GameObject LimiteDerecho;

    [Header("Configuracion de hordas")]
    public int[] ZombisPorHorda = { 3, 6, 9 };
    public float TiempoEntreHordas = 3f;
    public float TiempoEntreZombis = 0.5f;

    [Header("Camara")]
    public Camera CamaraJugador;
    public float MargenFueraCamara = 2f;

    [Header("Estado de las hordas")]
    public int HordaActual = 0;
    public int ZombisVivos = 0;
    public bool HordasIniciadas = false;
    public bool HordasTerminadas = false;

    private List<GameObject> zombisActivos =
        new List<GameObject>();

    private void Start()
    {
        // Desactivar los limites inicialmente
        ActivarLimites(false);

        // Obtener la camara principal
        if (CamaraJugador == null)
        {
            CamaraJugador = Camera.main;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Evitar iniciar las hordas nuevamente
        if (HordasIniciadas || HordasTerminadas)
            return;

        // Comprobar si entro el jugador
        if (collision.CompareTag("Player"))
        {
            if (!ComprobarConfiguracion())
                return;

            HordasIniciadas = true;

            // Cerrar la zona de combate
            ActivarLimites(true);

            // Comenzar las hordas
            StartCoroutine(IniciarHordas());
        }
    }

    private IEnumerator IniciarHordas()
    {
        for (int i = 0; i < ZombisPorHorda.Length; i++)
        {
            HordaActual = i + 1;

            Debug.Log("Comienza la horda " + HordaActual);

            int cantidad = ZombisPorHorda[i];

            // Generar los zombis de la horda
            for (int j = 0; j < cantidad; j++)
            {
                // Alternar los lados de aparicion
                bool ladoIzquierdo = j % 2 == 0;

                GenerarZombi(ladoIzquierdo);

                yield return new WaitForSeconds(
                    TiempoEntreZombis
                );
            }

            // Esperar hasta eliminar todos los zombis
            while (zombisActivos.Count > 0)
            {
                ActualizarZombisVivos();

                yield return null;
            }

            Debug.Log("Horda " + HordaActual + " completada");

            // Esperar antes de comenzar la siguiente
            if (i < ZombisPorHorda.Length - 1)
            {
                yield return new WaitForSeconds(
                    TiempoEntreHordas
                );
            }
        }

        FinalizarHordas();
    }

    private void GenerarZombi(bool ladoIzquierdo)
    {
        // Elegir un prefab aleatoriamente
        int indice = Random.Range(0, PrefabsZombis.Length);

        GameObject prefab = PrefabsZombis[indice];

        // Obtener el punto de aparicion
        Transform punto = ladoIzquierdo
            ? SpawnIzquierdo
            : SpawnDerecho;

        Vector3 posicion = punto.position;

        // Comprobar los limites visibles de la camara
        float mitadAncho = CamaraJugador.orthographicSize *
                           CamaraJugador.aspect;

        float bordeIzquierdo =
            CamaraJugador.transform.position.x - mitadAncho;

        float bordeDerecho =
            CamaraJugador.transform.position.x + mitadAncho;

        // Evitar que el zombi aparezca dentro de la camara
        if (ladoIzquierdo)
        {
            posicion.x = Mathf.Min(
                posicion.x,
                bordeIzquierdo - MargenFueraCamara
            );
        }
        else
        {
            posicion.x = Mathf.Max(
                posicion.x,
                bordeDerecho + MargenFueraCamara
            );
        }

        // Generar el zombi
        GameObject nuevoZombi = Instantiate(
            prefab,
            posicion,
            punto.rotation
        );

        // Registrar el zombi generado
        zombisActivos.Add(nuevoZombi);

        ActualizarZombisVivos();
    }

    private void ActualizarZombisVivos()
    {
        // Eliminar referencias de zombis destruidos
        zombisActivos.RemoveAll(zombi => zombi == null);

        ZombisVivos = zombisActivos.Count;
    }

    private void ActivarLimites(bool activar)
    {
        if (LimiteIzquierdo != null)
            LimiteIzquierdo.SetActive(activar);

        if (LimiteDerecho != null)
            LimiteDerecho.SetActive(activar);
    }

    private void FinalizarHordas()
    {
        HordasTerminadas = true;
        ZombisVivos = 0;

        // Permitir que el jugador continue
        ActivarLimites(false);

        Debug.Log("Todas las hordas completadas");
    }

    private bool ComprobarConfiguracion()
    {
        if (PrefabsZombis == null ||
            PrefabsZombis.Length == 0 ||
            SpawnIzquierdo == null ||
            SpawnDerecho == null ||
            LimiteIzquierdo == null ||
            LimiteDerecho == null ||
            CamaraJugador == null ||
            !CamaraJugador.orthographic ||
            ZombisPorHorda == null ||
            ZombisPorHorda.Length == 0)
        {
            Debug.LogError(
                "Falta configurar el SistemaHordas"
            );

            return false;
        }

        foreach (GameObject prefab in PrefabsZombis)
        {
            if (prefab == null)
            {
                Debug.LogError("Falta asignar un prefab");
                return false;
            }
        }

        return true;
    }
}

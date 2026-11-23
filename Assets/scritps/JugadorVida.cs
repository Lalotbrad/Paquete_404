
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class JugadorVida : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Interfaz de vida")]
    public TextMeshProUGUI TextoVida;

    [Header("Tiempo para reiniciar el nivel")]
    public float TiempoReinicio = 3f;

    private bool muerto = false;

    private void Start()
    {
        currentHealth = maxHealth;
        ActualizarVida();
    }

    public void TakeDamage(int amount)
    {
        if (muerto)
            return;

        currentHealth -= amount;

        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"Salud restante del jugador: {currentHealth}");

        ActualizarVida();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ActualizarVida()
    {
        if (TextoVida != null)
        {
            TextoVida.text = "Vida: " + currentHealth + " / " + maxHealth;
        }
    }

    private void Die()
    {
        if (muerto)
            return;

        muerto = true;

        Debug.Log("El jugador ha muerto");
        // Insertar reinicio de nivel o animación de muerte aquí

        // Desactivar los controles del jugador
        Jugador jugador = GetComponent<Jugador>();
        if (jugador != null)
            jugador.enabled = false;

        Disparo disparo = GetComponent<Disparo>();
        if (disparo != null)
            disparo.enabled = false;

        LanzarGranada granadas = GetComponent<LanzarGranada>();
        if (granadas != null)
            granadas.enabled = false;

        // Detener el movimiento del jugador
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        StartCoroutine(ReiniciarNivel());
    }

    private IEnumerator ReiniciarNivel()
    {
        yield return new WaitForSecondsRealtime(TiempoReinicio);

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}

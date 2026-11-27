using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instancia;

    private void Awake()
    {
        // Evitar que existan varios MusicManager
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;

        // Mantener la musica al cambiar de escena
        DontDestroyOnLoad(gameObject);
    }
}

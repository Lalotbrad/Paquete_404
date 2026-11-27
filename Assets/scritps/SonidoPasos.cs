using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SonidoPasos : MonoBehaviour
{
    [Header("Audios de caminata")]
    public AudioClip PisadaNormal;
    public AudioClip PisadaNahual;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ReproducirPisada()
    {
        // Reproducir el sonido de la pisada normal
        if (PisadaNormal != null)
        {
            audioSource.PlayOneShot(PisadaNormal);
        }
    }

    public void ReproducirPisadaNahual()
    {
        // Reproducir el sonido de la pisada del Nahual
        if (PisadaNahual != null)
        {
            audioSource.PlayOneShot(PisadaNahual);
        }
        else
        {
            ReproducirPisada();
        }
    }
}

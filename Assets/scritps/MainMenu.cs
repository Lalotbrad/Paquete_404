using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Nivel1");
    }
    public void Salir()
    {
        Debug.Log("Salir");
        Application.Quit();

    }

}

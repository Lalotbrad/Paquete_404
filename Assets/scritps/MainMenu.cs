using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void Salir()
    {
        Debug.Log("Salir");
        Application.Quit();

    }

}

using UnityEngine;
using UnityEngine.InputSystem; // Importante agregar esta línea

public class MenuCrontrol : MonoBehaviour
{
    public GameObject menuCanvas;

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        // Forma equivalente con el nuevo Input System:
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);

        }
    }
}

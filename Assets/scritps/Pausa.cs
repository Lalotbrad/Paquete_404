using UnityEngine;
using UnityEngine.InputSystem;

public class Pausa : MonoBehaviour
{
    public bool pausa = false;


    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame && pausa == false)
        {
            Time.timeScale = 0f;
            pausa = true;

        }
        else if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame && pausa == true)
        {
            Time.timeScale = 1f;
            pausa = false;
        }
    }
}

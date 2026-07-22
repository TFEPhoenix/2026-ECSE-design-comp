using UnityEngine;
using UnityEngine.InputSystem;

public class CloseBuildOnEscape : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Quit Attempted");
            Application.Quit();
        }
    }
}

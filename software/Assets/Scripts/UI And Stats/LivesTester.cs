using UnityEngine;
using UnityEngine.InputSystem;

public class LivesTester : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }


        // Press 1 = Add P1 life
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            LivesManager.Instance.AddPlayer1Life();

            Debug.Log(
                "P1 gained a life. P1 Lives: " +
                LivesManager.Instance.player1Lives
            );
        }


        // Press 2 = Add P2 life
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            LivesManager.Instance.AddPlayer2Life();

            Debug.Log(
                "P2 gained a life. P2 Lives: " +
                LivesManager.Instance.player2Lives
            );
        }


        // Press 3 = Damage P1
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            LivesManager.Instance.LosePlayer1Life();

            Debug.Log(
                "P1 lost a life. P1 Lives: " +
                LivesManager.Instance.player1Lives
            );
        }


        // Press 4 = Damage P2
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            LivesManager.Instance.LosePlayer2Life();

            Debug.Log(
                "P2 lost a life. P2 Lives: " +
                LivesManager.Instance.player2Lives
            );
        }
    }
}
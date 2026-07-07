using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreTester : MonoBehaviour
{
    void Update()
    {
        // Left mouse button = Player 1
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            int randomScore = Random.Range(1, 101);

            ScoreManager.Instance.AddPlayer1Score(randomScore);

            Debug.Log("Player 1 gained: " + randomScore);
        }


        // Right mouse button = Player 2
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            int randomScore = Random.Range(1, 101);

            ScoreManager.Instance.AddPlayer2Score(randomScore);

            Debug.Log("Player 2 gained: " + randomScore);
        }
    }
}
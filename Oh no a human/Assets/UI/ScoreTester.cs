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

            // Add to Player 1 score
            ScoreManager.Instance.AddPlayer1Score(randomScore);

            // Add to total points
            TotalPointManager.Instance.AddPoint(randomScore);


            Debug.Log("Player 1 gained: " + randomScore);
            Debug.Log("Player 1 Score: " + ScoreManager.Instance.player1Score);
            Debug.Log("Total Point: " + TotalPointManager.Instance.totalPoint);
        }


        // Right mouse button = Player 2
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            int randomScore = Random.Range(1, 101);

            // Add to Player 2 score
            ScoreManager.Instance.AddPlayer2Score(randomScore);

            // Add to total points
            TotalPointManager.Instance.AddPoint(randomScore);


            Debug.Log("Player 2 gained: " + randomScore);
            Debug.Log("Player 2 Score: " + ScoreManager.Instance.player2Score);
            Debug.Log("Total Point: " + TotalPointManager.Instance.totalPoint);
        }
    }
}
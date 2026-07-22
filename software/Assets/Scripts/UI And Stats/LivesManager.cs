using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance;

    public int player1Lives = 3;
    public int player2Lives = 3;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void AddPlayer1Life()
    {
        player1Lives++;
    }


    public void AddPlayer2Life()
    {
        player2Lives++;
    }


    public void LosePlayer1Life()
    {
        if (player1Lives > 0)
        {
            player1Lives--;
        }
    }


    public void LosePlayer2Life()
    {
        if (player2Lives > 0)
        {
            player2Lives--;
        }
    }


    public void ResetLives()
    {
        player1Lives = 3;
        player2Lives = 3;
    }
}
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance;

    public int player1Lives = 3;
    public int player2Lives = 3;

    private ContinueManager continueManager;
    private bool player1Dead = false;


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


    void Start()
    {
        continueManager = FindFirstObjectByType<ContinueManager>();

        if (continueManager == null)
        {
            Debug.LogError("ContinueManager not found!");
        }
    }


    public void LosePlayer1Life()
    {
        if (player1Lives <= 0)
        {
            return;
        }

        player1Lives--;

        Debug.Log("Player 1 Lives: " + player1Lives);


        if (player1Lives == 0 && !player1Dead)
        {
            player1Dead = true;

            if (continueManager != null)
            {
                continueManager.ShowContinueScreen();
            }
        }
    }


    public void LosePlayer2Life()
    {
        if (player2Lives > 0)
        {
            player2Lives--;
        }
    }


    public void AddPlayer1Life()
    {
        player1Lives++;
        player1Dead = false;
    }


    public void AddPlayer2Life()
    {
        player2Lives++;
    }


    public void ResetLives()
    {
        player1Lives = 3;
        player2Lives = 3;
        player1Dead = false;
    }
}
using UnityEngine;
using System.Collections;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance;

    public int player1Lives = 3;
    public int player2Lives = 3;

    // Invincibility flags
    public bool player1Invincible = false;
    public bool player2Invincible = false;

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
        // Ignore damage while invincible
        if (player1Invincible)
        {
            return;
        }

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
        // Ignore damage while invincible
        if (player2Invincible)
        {
            return;
        }

        if (player2Lives <= 0)
        {
            return;
        }

        player2Lives--;

        Debug.Log("Player 2 Lives: " + player2Lives);
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

        player1Invincible = false;
        player2Invincible = false;
    }

    // ===========================
    // INVINCIBILITY
    // ===========================

    public void StartPlayer1Invincibility()
    {
        StartCoroutine(Player1Invincibility());
    }

    IEnumerator Player1Invincibility()
    {
        player1Invincible = true;

        Debug.Log("Player 1 Invincible");

        yield return new WaitForSeconds(5);

        player1Invincible = false;

        Debug.Log("Player 1 Invincibility Ended");
    }

    public void StartPlayer2Invincibility()
    {
        StartCoroutine(Player2Invincibility());
    }

    IEnumerator Player2Invincibility()
    {
        player2Invincible = true;

        Debug.Log("Player 2 Invincible");

        yield return new WaitForSeconds(5);

        player2Invincible = false;

        Debug.Log("Player 2 Invincibility Ended");
    }
}
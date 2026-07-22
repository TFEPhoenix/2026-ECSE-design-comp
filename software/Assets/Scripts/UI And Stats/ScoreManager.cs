using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int player1Score = 0;
    public int player2Score = 0;

    public event Action OnScoreChanged;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void AddPlayer1Score(int amount)
    {
        player1Score += amount;
        OnScoreChanged?.Invoke();
    }


    public void AddPlayer2Score(int amount)
    {
        player2Score += amount;
        OnScoreChanged?.Invoke();
    }
}
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int player1Score = 0;
    public int player2Score = 0;
    float curScoreMult = 1;
    float scoreWaitCount = 0;


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


    void Update()
    {
        scoreWaitCount += Time.deltaTime;
        if (scoreWaitCount >= 4)
        {
            curScoreMult = 1;
        }
    }
    public void AddPlayer1Score(int amount)
    {
        player1Score += (int)(amount * curScoreMult);
        scoreWaitCount = 0;
        OnScoreChanged?.Invoke();
    }


    public void AddPlayer2Score(int amount)
    {
        player2Score += amount;
        OnScoreChanged?.Invoke();
    }

/** <summary>Returns time since last score increase</summary>
    */
    public float GetScorePause()
    {
        
        return scoreWaitCount;
    }

/** <summary>Increases score multiplier, default +0.1x</summary>
    */
    public void IncreaseMult()
    {
        curScoreMult += 0.1f;
    }
    public void IncreaseMult(float amount)
    {
        curScoreMult += amount;
    }
}
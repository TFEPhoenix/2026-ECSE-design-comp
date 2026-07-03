using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;

    public int currentScore = 0;
    public int highScore = 0;

    private const string HIGH_SCORE_KEY = "HighScore";

    void Awake()
    {
        // Singleton (prevents duplicates)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadHighScore();
    }

    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
        }
    }

    void SaveHighScore()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
        PlayerPrefs.Save(); // forces write to disk
    }

    public void ResetCurrentScore()
    {
        currentScore = 0;
    }

    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
    }
}
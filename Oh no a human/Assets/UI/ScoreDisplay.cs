using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI player1Text;
    private TextMeshProUGUI player2Text;


    void Start()
    {
        GameObject canvas = GameObject.Find("GameCanvas");

        player1Text = CreateScore(
            canvas.transform,
            "P1Score",
            new Vector2(0.15f, 0.9f)
        );

        player2Text = CreateScore(
            canvas.transform,
            "P2Score",
            new Vector2(0.85f, 0.9f)
        );


        // Listen for score changes
        ScoreManager.Instance.OnScoreChanged += UpdateScore;

        UpdateScore();
    }


    TextMeshProUGUI CreateScore(Transform parent, string name, Vector2 anchor)
    {
        GameObject scoreObject = new GameObject(name);

        scoreObject.transform.SetParent(parent);


        TextMeshProUGUI text = scoreObject.AddComponent<TextMeshProUGUI>();

        text.text = "0000";
        text.fontSize = 60;
        text.alignment = TextAlignmentOptions.Center;


        RectTransform rect = scoreObject.GetComponent<RectTransform>();

        rect.anchorMin = anchor;
        rect.anchorMax = anchor;

        rect.anchoredPosition = Vector2.zero;

        rect.sizeDelta = new Vector2(200, 100);


        return text;
    }


    void UpdateScore()
    {
        player1Text.text = ScoreManager.Instance.player1Score.ToString("0000");
        player2Text.text = ScoreManager.Instance.player2Score.ToString("0000");
    }


    void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScore;
        }
    }
}
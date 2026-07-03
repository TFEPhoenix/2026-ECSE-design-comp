using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreUI : MonoBehaviour
{
    private TextMeshProUGUI highScoreText;

    void Start()
    {
        SetupUI();
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void SetupUI()
    {
        // 1. Find or create Canvas
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("GameCanvas");

            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // 2. Create High Score Text
        GameObject textObj = new GameObject("HighScoreText");
        textObj.transform.SetParent(canvas.transform, false);

        highScoreText = textObj.AddComponent<TextMeshProUGUI>();

        // 3. Style
        highScoreText.fontSize = 32;
        highScoreText.color = Color.white;
        highScoreText.text = "High Score: 000000";

        // 4. Anchor TOP LEFT
        RectTransform rect = highScoreText.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);

        rect.anchoredPosition = new Vector2(200, -200);
    }

    void UpdateUI()
    {
        if (HighScoreManager.Instance == null || highScoreText == null)
            return;

        int score = HighScoreManager.Instance.highScore;

        highScoreText.text = "High Score: " + score.ToString("D6");
    }
}
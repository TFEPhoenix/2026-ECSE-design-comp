using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

/** <summary>Bullet Count UI</summary>
*/
public class Bullets : MonoBehaviour
{
    public int maxCount = 30;
    public int currentCount;

    private TextMeshProUGUI text;

    void Start()
    {
        currentCount = maxCount;

        // Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // TEXT
        GameObject textObj = new GameObject("CounterText");
        textObj.transform.SetParent(canvasObj.transform, false);

        text = textObj.AddComponent<TextMeshProUGUI>();
        text.fontSize = 40;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

        RectTransform textRect = text.GetComponent<RectTransform>();

        textRect.anchorMin = new Vector2(0f, 0f);
        textRect.anchorMax = new Vector2(0f, 0f);
        textRect.pivot = new Vector2(0f, 0f);

        textRect.anchoredPosition = new Vector2(20f, 20f);
        textRect.sizeDelta = new Vector2(200, 100);

        UpdateText();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentCount > 0)
            {
                currentCount--;
                UpdateText();
            }
        }
    }

    void UpdateText()
    {
        text.text = currentCount + " / " + maxCount;
    }
}
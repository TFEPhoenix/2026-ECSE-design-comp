using UnityEngine;
using TMPro;

public class TotalScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI totalScoreText;


    void Start()
    {
        Debug.Log("TotalScoreDisplay Started");


        GameObject canvas = GameObject.Find("GameCanvas");


        if (canvas == null)
        {
            Debug.LogError("GameCanvas not found!");
            return;
        }


        totalScoreText = CreateScoreText(
            canvas.transform,
            "TotalScoreText"
        );


        UpdateScore();
    }



    void Update()
    {
        UpdateScore();
    }



    TextMeshProUGUI CreateScoreText(
        Transform parent,
        string name)
    {
        GameObject obj = new GameObject(name);


        obj.transform.SetParent(parent, false);


        TextMeshProUGUI text =
            obj.AddComponent<TextMeshProUGUI>();


        text.text = "0";
        text.fontSize = 60;
        text.alignment = TextAlignmentOptions.Left;


        RectTransform rect = text.rectTransform;


        // Top left corner
        rect.anchorMin = new Vector2(0.02f, 0.95f);
        rect.anchorMax = new Vector2(0.02f, 0.95f);


        rect.sizeDelta = new Vector2(300, 80);


        return text;
    }



    void UpdateScore()
    {
        if (TotalPointManager.Instance == null)
        {
            return;
        }


        totalScoreText.text =
            TotalPointManager.Instance.totalPoint.ToString();
    }
}
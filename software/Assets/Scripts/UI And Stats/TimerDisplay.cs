using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    private TextMeshProUGUI timerText;


    void Start()
    {
        GameObject canvas = GameObject.Find("GameCanvas");


        if (canvas == null)
        {
            Debug.LogError("GameCanvas not found!");
            return;
        }


        timerText = CreateTimerText(
            canvas.transform,
            "TimerText"
        );
    }


    void Update()
    {
        UpdateTimer();
    }



    TextMeshProUGUI CreateTimerText(
        Transform parent,
        string name)
    {
        GameObject obj = new GameObject(name);


        obj.transform.SetParent(parent, false);


        TextMeshProUGUI text =
            obj.AddComponent<TextMeshProUGUI>();


        text.fontSize = 50;
        text.alignment = TextAlignmentOptions.Right;


        RectTransform rect =
            text.rectTransform;


        // Top right
        rect.anchorMin = new Vector2(0.98f, 0.96f);
        rect.anchorMax = new Vector2(0.98f, 0.96f);


        rect.pivot = new Vector2(1f, 1f);


        rect.sizeDelta =
            new Vector2(300, 80);


        return text;
    }



    void UpdateTimer()
    {
        if (GameTimer.Instance == null)
        {
            return;
        }


        float time =
            GameTimer.Instance.elapsedTime;


        int minutes =
            Mathf.FloorToInt(time / 60);


        int seconds =
            Mathf.FloorToInt(time % 60);


        int milliseconds =
            Mathf.FloorToInt((time * 1000) % 1000);



        timerText.text =
            string.Format(
                "{0:00}:{1:00}:{2:000}",
                minutes,
                seconds,
                milliseconds
            );
    }
}
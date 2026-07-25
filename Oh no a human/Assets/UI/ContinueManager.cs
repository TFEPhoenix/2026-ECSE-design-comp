using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ContinueManager : MonoBehaviour
{
    private Image fadeImage;
    private Image continueImage;
    private Image gameOverImage;

    private TextMeshProUGUI countdownText;

    private Coroutine countdownRoutine;

    private bool continueActive = false;


    void Start()
    {
        GameObject canvas = GameObject.Find("GameCanvas");

        if (canvas == null)
        {
            Debug.LogError("GameCanvas not found!");
            return;
        }


        // =========================
        // BLACK OVERLAY
        // =========================

        GameObject panel = new GameObject("ContinueFade");
        panel.transform.SetParent(canvas.transform, false);

        fadeImage = panel.AddComponent<Image>();

        RectTransform fadeRect = panel.GetComponent<RectTransform>();

        fadeRect.anchorMin = Vector2.zero;
        fadeRect.anchorMax = Vector2.one;
        fadeRect.offsetMin = Vector2.zero;
        fadeRect.offsetMax = Vector2.zero;

        fadeImage.color = new Color(0f, 0f, 0f, 0f);



        // =========================
        // CONTINUE IMAGE
        // =========================

        GameObject continueObject = new GameObject("ContinueImage");
        continueObject.transform.SetParent(canvas.transform, false);

        continueImage = continueObject.AddComponent<Image>();

        RectTransform continueRect = continueObject.GetComponent<RectTransform>();

        continueRect.anchorMin = new Vector2(0.5f, 0.5f);
        continueRect.anchorMax = new Vector2(0.5f, 0.5f);
        continueRect.anchoredPosition = new Vector2(0, 100);

        continueRect.sizeDelta = new Vector2(600, 300);


        Sprite continueSprite = Resources.Load<Sprite>("HorriblyDrawnPixilart/Continue");

        if (continueSprite != null)
        {
            continueImage.sprite = continueSprite;
        }
        else
        {
            Debug.LogError("Continue.png not found!");
        }

        continueImage.enabled = false;



        // =========================
        // TIMER TEXT
        // =========================

        GameObject textObject = new GameObject("ContinueTimer");
        textObject.transform.SetParent(canvas.transform, false);

        countdownText = textObject.AddComponent<TextMeshProUGUI>();

        RectTransform textRect = textObject.GetComponent<RectTransform>();

        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);

        textRect.anchoredPosition = new Vector2(0, -150);

        textRect.sizeDelta = new Vector2(400, 100);


        countdownText.alignment = TextAlignmentOptions.Center;
        countdownText.fontSize = 60;
        countdownText.color = Color.black;
        countdownText.text = "";



        // =========================
        // GAME OVER IMAGE
        // =========================

        GameObject gameOverObject = new GameObject("GameOverImage");
        gameOverObject.transform.SetParent(canvas.transform, false);

        gameOverImage = gameOverObject.AddComponent<Image>();

        RectTransform gameOverRect = gameOverObject.GetComponent<RectTransform>();

        gameOverRect.anchorMin = new Vector2(0.5f, 0.5f);
        gameOverRect.anchorMax = new Vector2(0.5f, 0.5f);

        gameOverRect.anchoredPosition = Vector2.zero;

        gameOverRect.sizeDelta = new Vector2(600, 300);


        Sprite gameOverSprite = Resources.Load<Sprite>("HorriblyDrawnPixilart/GameOver");

        if (gameOverSprite != null)
        {
            gameOverImage.sprite = gameOverSprite;
        }
        else
        {
            Debug.LogError("GameOver.png not found!");
        }

        gameOverImage.enabled = false;
    }



    void Update()
    {
        // Press SPACE to continue
        if (continueActive && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            UseContinue();
        }
    }



    public void ShowContinueScreen()
    {
        fadeImage.color = new Color(0f, 0f, 0f, 0.5f);

        continueImage.enabled = true;

        continueActive = true;


        if (countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
        }


        countdownRoutine = StartCoroutine(StartCountdown());
    }



    IEnumerator StartCountdown()
    {
        int time = 20;


        while (time > 0)
        {
            countdownText.text = time.ToString();

            yield return new WaitForSeconds(1);

            time--;
        }


        // =========================
        // GAME OVER
        // =========================

        continueActive = false;

        countdownText.text = "";

        continueImage.enabled = false;

        gameOverImage.enabled = true;


        Debug.Log("Game Over");


        yield return new WaitForSeconds(5);


        SceneManager.LoadScene("Menu");
    }



    void UseContinue()
    {
        Debug.Log("Continue Used");


        // Add 3 lives using LivesManager
        if (LivesManager.Instance != null)
        {
            LivesManager.Instance.AddPlayer1Life();
            LivesManager.Instance.AddPlayer1Life();
            LivesManager.Instance.AddPlayer1Life();
        }


        // Hide continue screen
        fadeImage.color = new Color(0f, 0f, 0f, 0f);

        continueImage.enabled = false;

        countdownText.text = "";


        // Stop countdown
        if (countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
        }


        continueActive = false;
    }
}
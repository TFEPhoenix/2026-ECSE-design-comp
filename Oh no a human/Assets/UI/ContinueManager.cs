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
    private Image continueWhiteImage;
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
        // BLACK FADE SCREEN
        // =========================

        GameObject panel = new GameObject("ContinueFade");
        panel.transform.SetParent(canvas.transform, false);

        fadeImage = panel.AddComponent<Image>();

        RectTransform fadeRect = panel.GetComponent<RectTransform>();

        fadeRect.anchorMin = Vector2.zero;
        fadeRect.anchorMax = Vector2.one;
        fadeRect.offsetMin = Vector2.zero;
        fadeRect.offsetMax = Vector2.zero;

        fadeImage.color = new Color(0, 0, 0, 0);



        // =========================
        // BLACK CONTINUE IMAGE
        // =========================

        continueImage = CreateImage(
            "ContinueImage",
            canvas.transform,
            "HorriblyDrawnPixilart/Continue"
        );

        continueImage.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(0, 100);

        continueImage.enabled = false;



        // =========================
        // WHITE CONTINUE IMAGE
        // =========================

        continueWhiteImage = CreateImage(
            "ContinueWhiteImage",
            canvas.transform,
            "HorriblyDrawnPixilart/Continue_white"
        );

        continueWhiteImage.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(0, 100);


        continueWhiteImage.color =
            new Color(1, 1, 1, 0);

        continueWhiteImage.enabled = false;



        // =========================
        // COUNTDOWN TEXT
        // =========================

        GameObject textObject = new GameObject("ContinueTimer");
        textObject.transform.SetParent(canvas.transform, false);

        countdownText = textObject.AddComponent<TextMeshProUGUI>();

        RectTransform textRect =
            textObject.GetComponent<RectTransform>();

        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);

        textRect.anchoredPosition = new Vector2(0, -150);
        textRect.sizeDelta = new Vector2(400, 100);


        countdownText.alignment = TextAlignmentOptions.Center;
        countdownText.fontSize = 60;
        countdownText.color = Color.white;
        countdownText.text = "";



        // =========================
        // GAME OVER IMAGE
        // =========================

        gameOverImage = CreateImage(
            "GameOverImage",
            canvas.transform,
            "HorriblyDrawnPixilart/GameOver"
        );

        gameOverImage.GetComponent<RectTransform>().anchoredPosition =
            Vector2.zero;

        gameOverImage.enabled = false;
    }



    Image CreateImage(string name, Transform parent, string path)
    {
        GameObject obj = new GameObject(name);

        obj.transform.SetParent(parent, false);

        Image image = obj.AddComponent<Image>();

        RectTransform rect = obj.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);

        rect.sizeDelta = new Vector2(600, 300);


        Sprite sprite = Resources.Load<Sprite>(path);

        if(sprite != null)
        {
            image.sprite = sprite;
        }
        else
        {
            Debug.LogError(path + " not found!");
        }


        // Put image above previous UI
        obj.transform.SetAsLastSibling();


        return image;
    }



    void Update()
    {
        if (continueActive &&
            Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            UseContinue();
        }
    }



    public void ShowContinueScreen()
    {
        continueActive = true;


        continueImage.enabled = true;

        if(countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
        }


        countdownRoutine =
            StartCoroutine(FadeToBlack());


        countdownRoutine =
            StartCoroutine(StartCountdown());
    }



    IEnumerator FadeToBlack()
    {
        float timer = 0;
        float duration = 1f;


        continueWhiteImage.enabled = true;


        while(timer < duration)
        {
            timer += Time.deltaTime;


            float alpha =
                Mathf.Lerp(0, 1, timer / duration);


            fadeImage.color =
                new Color(0,0,0,alpha);


            continueWhiteImage.color =
                new Color(1,1,1,alpha);


            yield return null;
        }


        fadeImage.color =
            new Color(0,0,0,1);


        continueWhiteImage.color =
            new Color(1,1,1,1);
    }



    IEnumerator StartCountdown()
    {
        int time = 20;


        while(time > 0)
        {
            countdownText.text =
                time.ToString();


            yield return new WaitForSeconds(1);


            time--;
        }



        // GAME OVER

        continueActive = false;

        countdownText.text = "";

        continueImage.enabled = false;
        continueWhiteImage.enabled = false;


        gameOverImage.enabled = true;


        Debug.Log("Game Over");


        yield return new WaitForSeconds(5);


        SceneManager.LoadScene("Menu");
    }



    void UseContinue()
    {
        Debug.Log("Continue Used");


        if(LivesManager.Instance != null)
        {
            LivesManager.Instance.AddPlayer1Life();
            LivesManager.Instance.AddPlayer1Life();
            LivesManager.Instance.AddPlayer1Life();


            LivesManager.Instance.StartPlayer1Invincibility();
            LivesManager.Instance.StartPlayer2Invincibility();
        }



        if(countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
        }


        fadeImage.color =
            new Color(0,0,0,0);


        continueImage.enabled = false;
        continueWhiteImage.enabled = false;
        gameOverImage.enabled = false;


        countdownText.text = "";


        continueActive = false;
    }
}
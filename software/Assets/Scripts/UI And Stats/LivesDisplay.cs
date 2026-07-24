using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LivesDisplay : MonoBehaviour
{
    private Image[] player1Hearts = new Image[3];
    private Image[] player2Hearts = new Image[3];


    private TextMeshProUGUI player1ExtraText;
    private TextMeshProUGUI player2ExtraText;


    private Sprite heartSprite;
    private Sprite brokenHeartSprite;



    void Start()
    {
        Debug.Log("LivesDisplay Started");


        GameObject canvas = GameObject.Find("GameCanvas");


        if (canvas == null)
        {
            Debug.LogError("GameCanvas not found!");
            return;
        }


        // Load sprites
        heartSprite = Resources.Load<Sprite>(
            "HorriblyDrawnPixilart/Heart"
        );


        brokenHeartSprite = Resources.Load<Sprite>(
            "HorriblyDrawnPixilart/BrokenHeart"
        );


        if (heartSprite == null)
        {
            Debug.LogError("Heart sprite missing!");
        }


        if (brokenHeartSprite == null)
        {
            Debug.LogError("BrokenHeart sprite missing!");
        }



        // Player 1 lives above ammo
        CreateLives(
            canvas.transform,
            player1Hearts,
            "P1Heart",
            "P1ExtraLives",
            new Vector2(0.4f, 0.16f),
            out player1ExtraText
        );



        // Player 2 lives above ammo
        CreateLives(
            canvas.transform,
            player2Hearts,
            "P2Heart",
            "P2ExtraLives",
            new Vector2(0.6f, 0.16f),
            out player2ExtraText
        );


        UpdateLives();
    }



    void Update()
    {
        UpdateLives();
    }



    void CreateLives(
        Transform parent,
        Image[] hearts,
        string heartName,
        string extraName,
        Vector2 anchor,
        out TextMeshProUGUI extraText)
    {

        // Create 3 heart sprites
        for (int i = 0; i < 3; i++)
        {
            GameObject obj =
                new GameObject(heartName + i);


            obj.transform.SetParent(parent, false);


            Image image =
                obj.AddComponent<Image>();


            image.sprite = heartSprite;
            image.preserveAspect = true;


            RectTransform rect =
                image.rectTransform;


            rect.anchorMin = anchor;
            rect.anchorMax = anchor;


            // Spread hearts horizontally
            rect.anchoredPosition =
                new Vector2((i - 1) * 75, 0);

            rect.sizeDelta =
                new Vector2(70, 70);


            hearts[i] = image;
        }



        // Extra life text (+X)
        GameObject extraObj =
            new GameObject(extraName);


        extraObj.transform.SetParent(parent, false);


        extraText =
            extraObj.AddComponent<TextMeshProUGUI>();


        extraText.text = "";


        extraText.fontSize = 40;
        extraText.alignment =
            TextAlignmentOptions.Left;


        RectTransform extraRect =
            extraText.rectTransform;


        extraRect.anchorMin = anchor;
        extraRect.anchorMax = anchor;


        // Put +X after the 3 hearts
        extraRect.anchoredPosition =
            new Vector2(90, 0);


        extraRect.sizeDelta =
            new Vector2(100, 50);
    }



    void UpdateLives()
    {
        if (LivesManager.Instance == null)
        {
            return;
        }


        UpdatePlayerLives(
            player1Hearts,
            player1ExtraText,
            LivesManager.Instance.player1Lives
        );


        UpdatePlayerLives(
            player2Hearts,
            player2ExtraText,
            LivesManager.Instance.player2Lives
        );
    }



    void UpdatePlayerLives(
        Image[] hearts,
        TextMeshProUGUI extraText,
        int lives)
    {

        int visibleHearts =
            Mathf.Min(lives, 3);


        int extraLives =
            Mathf.Max(lives - 3, 0);



        // Update heart sprites
        for (int i = 0; i < 3; i++)
        {
            if (i < visibleHearts)
            {
                hearts[i].sprite = heartSprite;
            }
            else
            {
                hearts[i].sprite = brokenHeartSprite;
            }
        }



        // Update extra lives text
        if (extraLives > 0)
        {
            extraText.text =
                "+" + extraLives;
        }
        else
        {
            extraText.text = "";
        }
    }
}
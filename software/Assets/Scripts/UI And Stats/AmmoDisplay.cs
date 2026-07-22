using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    private TextMeshProUGUI player1AmmoText;
    private TextMeshProUGUI player2AmmoText;

    void Start()
    {
        Debug.Log("AmmoDisplay Started");


        // Find Canvas
        GameObject canvas = GameObject.Find("GameCanvas");

        if (canvas == null)
        {
            Debug.LogError("GameCanvas not found!");
            return;
        }

        Debug.Log("GameCanvas found");


        // Load bullet sprite
        Sprite bulletSprite = Resources.Load<Sprite>(
            "HorriblyDrawnPixilart/RedTippedBullet"
        );

        if (bulletSprite == null)
        {
            Debug.LogError("FAILED TO LOAD BULLET SPRITE");
        }
        else
        {
            Debug.Log("LOADED: " + bulletSprite.name);
        }


        if (bulletSprite == null)
        {
            Debug.LogError("Bullet sprite failed to load!");
        }
        else
        {
            Debug.Log("Bullet sprite loaded: " + bulletSprite.name);
        }



        // Create Player 1 Ammo
        player1AmmoText = CreateAmmoText(
            canvas.transform,
            "P1Ammo",
            new Vector2(0.4f, 0.1f)
        );


        CreateBulletIcon(
            canvas.transform,
            "P1BulletIcon",
            new Vector2(0.46f, 0.1f),
            bulletSprite
        );



        // Create Player 2 Ammo
        player2AmmoText = CreateAmmoText(
            canvas.transform,
            "P2Ammo",
            new Vector2(0.6f, 0.1f)
        );


        CreateBulletIcon(
            canvas.transform,
            "P2BulletIcon",
            new Vector2(0.54f, 0.1f),
            bulletSprite
        );


        UpdateAmmo();
    }



    TextMeshProUGUI CreateAmmoText(
        Transform parent,
        string name,
        Vector2 anchor)
    {
        GameObject obj = new GameObject(name);

        obj.transform.SetParent(parent, false);


        TextMeshProUGUI text =
            obj.AddComponent<TextMeshProUGUI>();

        text.text = "30/30";
        text.fontSize = 50;
        text.alignment = TextAlignmentOptions.Center;


        RectTransform rect = text.rectTransform;

        rect.anchorMin = anchor;
        rect.anchorMax = anchor;

        rect.sizeDelta = new Vector2(150, 80);


        Debug.Log(name + " created");

        return text;
    }



    void CreateBulletIcon(
        Transform parent,
        string name,
        Vector2 anchor,
        Sprite sprite)
    {
        GameObject obj = new GameObject(name);

        obj.transform.SetParent(parent, false);


        Image image = obj.AddComponent<Image>();

        if (sprite != null)
        {
            image.sprite = sprite;
            Debug.Log(name + " received sprite");
        }
        else
        {
            Debug.LogError(name + " has no sprite!");
        }


        image.preserveAspect = true;


        RectTransform rect = image.rectTransform;

        rect.anchorMin = anchor;
        rect.anchorMax = anchor;

        rect.sizeDelta = new Vector2(40, 40);
    }

    void Update()
    {
        UpdateAmmo();
    }

    void UpdateAmmo()
    {
        if (AmmoManager.Instance == null)
        {
            Debug.LogError("AmmoManager missing!");
            return;
        }


        player1AmmoText.text =
            AmmoManager.Instance.player1Ammo +
            "/" +
            AmmoManager.Instance.player1MaxAmmo;


        player2AmmoText.text =
            AmmoManager.Instance.player2Ammo +
            "/" +
            AmmoManager.Instance.player2MaxAmmo;
    }


}
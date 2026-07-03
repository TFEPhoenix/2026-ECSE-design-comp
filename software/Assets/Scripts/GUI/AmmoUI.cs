using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public int maxAmmo = 30;
    public int currentAmmo = 30;

    private TextMeshProUGUI ammoText;

    void Start()
    {
        SetupUI();
        UpdateAmmo();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentAmmo > 0)
            {
                currentAmmo--;
                UpdateAmmo();
            }
        }
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

            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // 2. Create Ammo Text
        GameObject textObj = new GameObject("AmmoText");
        textObj.transform.SetParent(canvas.transform, false);

        ammoText = textObj.AddComponent<TextMeshProUGUI>();

        // 3. Style it
        ammoText.fontSize = 32;
        ammoText.color = Color.white;
        ammoText.alignment = TextAlignmentOptions.BottomLeft;

        RectTransform rect = ammoText.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(0, 0);
        rect.anchoredPosition = new Vector2(200, 100);
    }

    public void UpdateAmmo()
    {
        ammoText.text = currentAmmo + "/" + maxAmmo;
    }
}
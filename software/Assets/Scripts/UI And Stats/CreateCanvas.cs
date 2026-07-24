using UnityEngine;
using UnityEngine.UI;

public class CreateCanvas : MonoBehaviour
{
    void Start()
    {
        // Create Canvas GameObject
        GameObject canvasObject = new GameObject("GameCanvas");

        // Add Canvas component
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Add CanvasScaler
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Add GraphicRaycaster (for buttons/interactions)
        canvasObject.AddComponent<GraphicRaycaster>();
    }
}
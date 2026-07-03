using UnityEngine;
using UnityEngine.UI;

public class GameCanvasManager : MonoBehaviour
{
    public static GameObject CanvasObj;

    void Awake()
    {
        if (CanvasObj != null) return;
        
        CanvasObj = new GameObject("GameCanvas");

        Canvas canvas = CanvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = CanvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        CanvasObj.AddComponent<GraphicRaycaster>();
    }
}
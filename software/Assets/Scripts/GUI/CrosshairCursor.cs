using UnityEngine;

public class CrosshairCursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    
    

    void Start()
    {
        // The clickable point of the cursor (e.g., top-left corner is Vector2.zero)
        Vector2 hotSpot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);

        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
}

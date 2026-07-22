using UnityEngine;

public class HitFlashSingle : MonoBehaviour
{
    float flash_length = 0.14f;
    float cur_time = 0;
    Renderer renderer;
    Color originalColor;


    public void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }
    public void StartFlash()
    {   
        
        renderer.material.color = Color.blue;
        cur_time = flash_length;
    }

    public void EndFlash()
    {
        renderer.material.color = originalColor;
    }
    public void Update()
    {
        if (cur_time > 0)
        {
            cur_time -= Time.deltaTime;
            if (cur_time <= 0)
            {
                cur_time = -1;
                EndFlash();
            }
        }
    }
}
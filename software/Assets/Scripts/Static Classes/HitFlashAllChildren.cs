using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HitFlashAllChildren : MonoBehaviour
{
    float flash_length = 0.14f;
    float cur_time = 0;
    List<Renderer> renderers;
    MaterialPropertyBlock block;
    Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

    public void Start()
    {
        renderers = GetComponentsInChildren<Renderer>().ToList();
        block = new MaterialPropertyBlock();

        // Store original colours for Object & children
        foreach (Renderer r in renderers)
        {
            originalColors[r] = r.sharedMaterial.GetColor("_BaseColor");
        }
    }
    public void StartFlash()
    {   
        renderers.RemoveAll(r => r == null);
        foreach (Renderer r in renderers)
        {
            
            r.GetPropertyBlock(block);
            block.SetColor("_BaseColor", Color.red);
            r.SetPropertyBlock(block);
        }
        cur_time = flash_length;
    }
    
    // Resets to original color
    public void EndFlash()
    {
        renderers.RemoveAll(r => r == null);
        // Sets object & children to the stored original colours
        foreach (Renderer r in renderers)
        {
        
        r.GetPropertyBlock(block);
        block.SetColor("_BaseColor", originalColors[r]);
        r.SetPropertyBlock(block);
        }
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
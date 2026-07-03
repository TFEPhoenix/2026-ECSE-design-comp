using UnityEngine;
using UnityEngine.Rendering;

public class CubeEnemy : MonoBehaviour, Enemy
{
    public int maxHealth;
    int curHealth;
    HitFlashAllChildren HitFlashLoader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curHealth = maxHealth;
        HitFlashLoader = gameObject.AddComponent<HitFlashAllChildren>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHit()
    {
        HitFlashLoader.StartFlash();
        curHealth -= 1;
        if (curHealth <= 0)
        {
            OnDeath();
        }
        
    }
    public void OnDeath()
    {
        Destroy(gameObject);
    }
}

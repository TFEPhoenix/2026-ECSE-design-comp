using Unity.VisualScripting;
using UnityEngine;

public class HumanEnemy : MonoBehaviour, Enemy
{
    public int scoreForKill;
    public int maxHealth;
    int curHealth;
    HitFlashAllChildren HitFlashLoader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int powerUpChance; // in percent
    Animator animationControl;
    void Start()
    {
        curHealth = maxHealth;
        HitFlashLoader = gameObject.AddComponent<HitFlashAllChildren>();
        animationControl = gameObject.GetComponent<Animator>();
        if (gameObject.GetComponentInChildren<Shield>() != null)
        {
            animationControl.SetBool("Shield", true);
        }
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
        if (Random.Range(0, 101) <= powerUpChance)
        {
            GameObject powerUp = Instantiate(PrefabRef.Instance.GetPowerUp());
            powerUp.transform.position = transform.position;
        }
        ScoreManager.Instance.AddPlayer1Score(scoreForKill);
        GameObject deathExplosion = Instantiate(PrefabRef.Instance.GetExplosion()); // Purely Visual
        deathExplosion.transform.position = transform.position;
        Destroy(gameObject);
    }
}

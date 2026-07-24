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
        // Check if has Shield
        if (gameObject.GetComponentInChildren<Shield>() != null)
        {
            animationControl.SetBool("Shield", true);
        }
        if (gameObject.GetComponentInChildren<EnemyShoot>().gameObject.name == "LobberGun")
        {
            animationControl.SetBool("Cannon", true);
            animationControl.SetBool("Pistol", false);
        }
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
        // Spawn a powerup on chance
        if (Random.Range(0, 101) <= powerUpChance)
        {
            GameObject powerUp = Instantiate(PrefabRef.Instance.GetPowerUp());
            powerUp.transform.position = transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03").position;
        }
        // Give Player score and increase multiplier
        ScoreManager.Instance.AddPlayer1Score(scoreForKill);
        if (ScoreManager.Instance.GetScorePause() <= 2)
        {
            ScoreManager.Instance.IncreaseMult();
        }

        // Spawn purely Visual explosion
        GameObject deathExplosion = Instantiate(PrefabRef.Instance.GetExplosion());
        deathExplosion.transform.position = transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03").position;
        // Destroy the Enemy
        Destroy(gameObject);
    }
}

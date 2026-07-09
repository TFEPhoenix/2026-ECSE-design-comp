using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance;


    // Player ammo
    public int player1MaxAmmo = 30;
    public int player2MaxAmmo = 30;

    public int player1Ammo;
    public int player2Ammo;


    // Bullet type
    public BulletType player1BulletType = BulletType.Normal;
    public BulletType player2BulletType = BulletType.Normal;



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        // Start with full ammo
        player1Ammo = player1MaxAmmo;
        player2Ammo = player2MaxAmmo;
    }
}
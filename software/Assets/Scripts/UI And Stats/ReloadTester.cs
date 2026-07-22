using UnityEngine;

public class ReloadTester : MonoBehaviour
{
    public int reloadLength = 2;
    float curTimer;

    void Start()
    {
        curTimer = reloadLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (AmmoManager.Instance.player1Ammo == 0)
        {
            if (curTimer <= 0)
            {
                Debug.Log("Reloaded");
                AmmoManager.Instance.player1Ammo = AmmoManager.Instance.player1MaxAmmo;
                curTimer = reloadLength;
            }
            else
            {
                curTimer -= Time.deltaTime;
            }
        }
    }
}

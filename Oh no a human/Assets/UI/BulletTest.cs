using UnityEngine;
using UnityEngine.InputSystem;

public class BulletTest : MonoBehaviour
{
    void Update()
    {
        // Left mouse click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }


    void Shoot()
    {
        if (AmmoManager.Instance == null)
        {
            Debug.LogError("AmmoManager missing!");
            return;
        }


        // Check ammo
        if (AmmoManager.Instance.player1Ammo > 0)
        {
            AmmoManager.Instance.player1Ammo--;

            Debug.Log(
                "P1 Ammo: " +
                AmmoManager.Instance.player1Ammo +
                "/" +
                AmmoManager.Instance.player1MaxAmmo
            );
        }
        else
        {
            Debug.Log("Out of ammo!");
        }
    }
}
using UnityEngine;

public class DestructableObject : MonoBehaviour, BulletHit
{
    public int health = 1;

    public virtual void OnHit()
    {
        health-=1;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

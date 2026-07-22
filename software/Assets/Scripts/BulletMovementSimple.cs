using UnityEngine;

public class BulletMovementSimple : EnemyBullet
{
    bool active = true;
    public int speed = 8;
    float lifeTime = 0; // Time since creation (s)
    public float lifeSpan; // Constant, maximum time alive (s)
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        CheckHit();
        // Moves foward and destroys itself after lifeSpan
        if(active){
            transform.position += transform.forward * speed * Time.deltaTime;
            if (lifeTime < lifeSpan)
            {
                lifeTime += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

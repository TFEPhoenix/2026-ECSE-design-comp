using UnityEngine;

public class PowerUp : MonoBehaviour, BulletHit
{

    public BulletType type;
    public float gravity;
    public float posUp;
    public float speedInit;
    Vector3 velocity;
    Vector3 goalPos;
    bool reachedGoal = false;
    public float lifeTime = 5;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = lifeTime;
        // Randomise type if not set (ie prefab)
        // Equal weight
        if (type == BulletType.Normal)
        {
            type = (BulletType)Random.Range(1,System.Enum.GetValues(typeof(BulletType)).Length);
        }
        goalPos = transform.position + (posUp * transform.up);
        velocity = speedInit * transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (!reachedGoal)
        {
            transform.position = Vector3.MoveTowards(transform.position, goalPos, speedInit*Time.deltaTime);
            if (transform.position == goalPos)
            {
                reachedGoal = true;
            }
        }
        else
        {
            transform.position += velocity * Time.deltaTime;
            velocity -= gravity*Time.deltaTime * transform.up;
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnHit()
    {
        AmmoManager.Instance.LoadAmmoType(type);
        Destroy(gameObject);
    }
}

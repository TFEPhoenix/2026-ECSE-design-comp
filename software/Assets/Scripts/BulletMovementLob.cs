using UnityEngine;

public class BulletMovementLob : EnemyBullet
{

    public float gravity = 1;
    public float horizontalSpeed = 5;
    float initialVert;
    Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Uses Position of player's hurtpoint
        Vector3 vectorToHurt = Camera.main.transform.parent.Find("HurtPoint").position - transform.position;
        Vector2 horizontalvectorToHurt = new Vector2(vectorToHurt.x, vectorToHurt.z);
        float horizontalDistToCamera = horizontalvectorToHurt.magnitude;
        float timeToReachCam = horizontalDistToCamera / horizontalSpeed;
        float verticalDistToCamera = vectorToHurt.y;
        initialVert = (verticalDistToCamera/timeToReachCam) + (0.5f*gravity*timeToReachCam);
        Vector2 horizontalVelocity = horizontalvectorToHurt.normalized * horizontalSpeed;
        velocity = new Vector3(horizontalVelocity.x, initialVert, horizontalVelocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        CheckHit();
        velocity.y -= gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }
}

using System.Diagnostics;
using UnityEngine;

public class BulletMovementLob : MonoBehaviour
{

    public float gravity = 1;
    public float horizontalSpeed = 5;
    float initialVert;
    Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 vectorToCamera = Camera.main.transform.position - transform.position;
        Vector2 horizontalVectorToCamera = new Vector2(vectorToCamera.x, vectorToCamera.z);
        float horizontalDistToCamera = horizontalVectorToCamera.magnitude;
        float timeToReachCam = horizontalDistToCamera / horizontalSpeed;
        float verticalDistToCamera = vectorToCamera.y;
        initialVert = (verticalDistToCamera/timeToReachCam) + (0.5f*gravity*timeToReachCam);
        Vector2 horizontalVelocity = horizontalVectorToCamera.normalized * horizontalSpeed;
        velocity = new Vector3(horizontalVelocity.x, initialVert, horizontalVelocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        velocity.y -= gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float turnSpeed = 360;
    public float maxShotCooldown; // Uses deltaTime (seconds)
    float curShotCooldown;
    public bool canShoot = false; // Used to prevent shooting while moving, not related to cooldown

    void Start()
    {
        curShotCooldown = maxShotCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot)
        {
            Vector3 vectorToCamera = Camera.main.transform.position - transform.position;
            // Find the rotation Quaternion towards the camera, with no up/down rotation
            Quaternion lookToCamera = Quaternion.LookRotation(new Vector3(vectorToCamera.x, 0, vectorToCamera.z));
            // Turn object's parent (Enemy) towards the camera
            transform.parent.rotation = Quaternion.RotateTowards(
            transform.parent.rotation,
            lookToCamera,
            turnSpeed * Time.deltaTime);
            if (curShotCooldown <= 0)
            {
                onShoot();
                curShotCooldown = maxShotCooldown;
            }
            else
            {
                curShotCooldown -= Time.deltaTime;
            }
            
        }
        
    }

    void onShoot()
    {
        Quaternion aimAngle = Quaternion.LookRotation(Camera.main.transform.position - gameObject.transform.position);
        GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, aimAngle);
    }
}

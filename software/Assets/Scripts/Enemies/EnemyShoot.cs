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
        canShoot = CheckCanShoot();
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
    /** <summary>Returns false if any checked components or children are preventing shooting</summary>
    */
    public bool CheckCanShoot()
    {
        EnemyWalk walkScript = gameObject.GetComponentInParent<EnemyWalk>();
        if (walkScript != null)
        {
            if (walkScript.GetPreventShoot())
            {
                return false;
            }
        }
        Transform shieldHolder = transform.parent.Find("ShieldHolder");
        if (shieldHolder != null){
            Transform shield = shieldHolder.Find("Shield");
            if (shield!=null){
                shield.TryGetComponent<Shield>(out var shieldScript);
                if (shieldScript != null){
                    if (shieldScript.GetPreventShoot())
                    {
                        return false;
                    }
                }
            }
            else
            {
                // Destroy the shieldHolder if the shield has been broken
                // Should likely be checked elsewhere
                Destroy(shieldHolder);
            }
        }

        

        return true;
    }
    void onShoot()
    {
        Quaternion aimAngle = Quaternion.LookRotation(Camera.main.transform.parent.Find("HurtPoint").position - gameObject.transform.position);
        GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, aimAngle);
    }
}

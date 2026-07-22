using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float turnSpeed = 360;
    public float maxShotCooldown; // Uses deltaTime (seconds)
    float curShotCooldown;
    public bool canShoot = false; // Used to prevent shooting while moving, not related to cooldown
    Animator animationControl;
    GameObject root;

    void Start()
    {
        curShotCooldown = maxShotCooldown;
        animationControl = gameObject.GetComponentInParent<Animator>();
        root = animationControl.gameObject;
        if (root == null)
        {
            root = transform.parent.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Process turning whenever not walking
        if (!checkIsWalking())
        {
            
            Vector3 vectorToCamera = Camera.main.transform.position - transform.position;
            // Find the rotation Quaternion towards the camera, with no up/down rotation
            Quaternion lookToCamera = Quaternion.LookRotation(new Vector3(vectorToCamera.x, 0, vectorToCamera.z));
            // Turn object's parent (Enemy) towards the camera
            root.transform.rotation = Quaternion.RotateTowards(
            root.transform.rotation,
            lookToCamera,
            turnSpeed * Time.deltaTime);
        }
        else if (animationControl != null)
        {
            animationControl.SetBool("Aiming", false);
        }
        if (CheckCanShoot())
        {
            if (animationControl != null)
            {
                animationControl.SetBool("Aiming", true);
            }
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
        else
        {
            if (animationControl != null)
            {
                animationControl.SetBool("Aiming", false);
            }
        }
        
    }
    /** <summary>Returns false if any checked components or children are preventing shooting</summary>
    */
    public bool CheckCanShoot()
    {
        EnemyWalk walkScript = root.GetComponent<EnemyWalk>();
        if (walkScript != null)
        {
            if (walkScript.GetPreventShoot())
            {
                return false;
            }
        }
        Shield shieldScript = root.GetComponentInChildren<Shield>();
        if (shieldScript != null){
            if (shieldScript.GetPreventShoot())
            {
                return false;
            }
        }

        

        return true;
    }
    bool checkIsWalking()
    {
        EnemyWalk walkScript = gameObject.GetComponentInParent<EnemyWalk>();
        if (walkScript != null)
        {
            return walkScript.GetIsWalking();
        }
        return false;
    }
    void onShoot()
    {
        Quaternion aimAngle = Quaternion.LookRotation(Camera.main.transform.parent.Find("HurtPoint").position - gameObject.transform.position);
        GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, aimAngle);
    }
}

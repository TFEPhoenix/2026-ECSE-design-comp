using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class ClickShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public LayerMask layerToIgnore;
    GameObject newObject; 
    Ray ray;
    Vector3 mousePos;
    Vector3 targetDirection;
    Camera camera;
    Transform endTransform;
    bool hasAmmo;
    void Start()
    {
        camera = Camera.main;
        hasAmmo = AmmoManager.Instance.player1Ammo > 0;
        // Inverts the mask so that it functions as intended
        layerToIgnore = ~layerToIgnore;
    }

    void Update()
    {
        hasAmmo = AmmoManager.Instance.player1Ammo > 0;
        if(hasAmmo && Mouse.current.leftButton.wasPressedThisFrame){
            // Lower Ammo Count by 1
            AmmoManager.Instance.player1Ammo --;
            // Get the transform of the object directly in the path of bullet
            // Should only detect those in selected targetLayer (Bullet target), within a distance of 200
            ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            targetDirection = ray.GetPoint(100f) - ray.origin;
            targetDirection.Normalize();
            // Create the bullet (purely visual)
            Create();
            // Transform of object that the bullet would hit - For Hitscanning
            Transform hitObjectTransform = null;
            Vector3 hitPoint = new Vector3(0,0,0);
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, layerMask: layerToIgnore))
            {
                hitObjectTransform = hit.transform;
                hitPoint = hit.point;
            }
            // If raycast hits an applicable object
            if ( !hit.transform.IsUnityNull())
            {
                ProcessHit(hitObjectTransform, hitPoint, ray);
            }
        }
    }
    /** <summary>Creates a purely visual bullet</summary>
    */
    void Create()
    {
        mousePos = Mouse.current.position.ReadValue();
        newObject = Instantiate(bulletPrefab, ray.origin, Quaternion.LookRotation(targetDirection));
    }

    void ProcessHit(Transform hitObjectTransform, Vector3 hitPoint, Ray ray)
    {
        BulletType curBulletType = AmmoManager.Instance.player1BulletType;
        BulletHit hitScript; 
        // Regular Hit is always processed
        if (hitObjectTransform.gameObject.TryGetComponent(out hitScript))
        {
            hitScript.OnHit();
        }

        // Special Bullet types
        if (curBulletType.Equals(BulletType.Explosive))
        {
            Collider[] colliders = Physics.OverlapSphere( hitPoint, AmmoManager.Instance.explosionSize);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out hitScript) && collider.transform != hitObjectTransform && (collider.GetComponent<PowerUp>() == null))
                {
                    hitScript.OnHit();
                }
            }
        } else if (curBulletType.Equals(BulletType.Piercing))
        {
            RaycastHit[] hits = Physics.RaycastAll(hitPoint, ray.direction, 200f, layerToIgnore);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.TryGetComponent(out hitScript) && hit.transform != hitObjectTransform)
                {
                    hitScript.OnHit();
                }
            }
        }
        
    }

}
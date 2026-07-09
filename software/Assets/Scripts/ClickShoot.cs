using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.InputSystem;
using System.Collections;
public class ClickShoot : MonoBehaviour
{
    public GameObject prefab;
    public GameObject countScript;
    public LayerMask targetLayer;
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
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, targetLayer))
            {
                hitObjectTransform = hit.transform;
            }
            // If raycast hits an applicable object
            if ( !hit.transform.IsUnityNull())
            {
                // Proccess hit on target
                if (hitObjectTransform.gameObject.TryGetComponent<BulletHit>(out var HitScript))
                {
                    HitScript.OnHit();
                    
                }
            }
        }
    }
    /** <summary>Creates a purely visual bullet</summary>
    */
    void Create()
    {
        mousePos = Mouse.current.position.ReadValue();
        newObject = Instantiate(prefab, ray.origin, Quaternion.LookRotation(targetDirection));
    }

}
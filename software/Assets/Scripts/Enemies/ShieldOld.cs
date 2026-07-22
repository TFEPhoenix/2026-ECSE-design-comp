using UnityEngine;

public class ShieldOld : DestructableObject
{
    Transform raisedRef;
    Transform loweredRef;
    public float speedMove;
    public float speedTurn;
    bool raising = false;
    bool lowering = false;
    bool finishedRaising = false;
    bool finishedLowering = true; // Assumes shield starts ~ in lowered position
    bool preventShoot = false;
    float timer = 5;
    bool forceShieldUp = false;
    
    void Start()
    {
        raisedRef = transform.parent.Find("RaiseRef");
        loweredRef = transform.parent.Find("LowRef");
    }

    void Update()
    {
        
        checkShieldForceUp();
        if (raising)
        {
            transform.position = Vector3.MoveTowards(transform.position, raisedRef.position, speedMove * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, raisedRef.rotation, speedTurn * Time.deltaTime);
            // Check if finished raising shield
            if (transform.position == raisedRef.position && transform.rotation == raisedRef.rotation)
            {
                raising = false;
                finishedRaising = true;
                finishedLowering = false;
            }
        } else if (lowering)
        {
            transform.position = Vector3.MoveTowards(transform.position, loweredRef.position, speedMove * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, loweredRef.rotation, speedTurn * Time.deltaTime);
            // Check if finished lowering shield
            if (transform.position == loweredRef.position && transform.rotation == loweredRef.rotation)
            {
                raising = false;
                finishedRaising = false;
                finishedLowering = true;
                preventShoot = false;
            }
        }
        if (!forceShieldUp){
            if (timer <= 0)
            {
                if (finishedRaising)
                {
                    LowerShield();
                    timer = 5;
                } else if (finishedLowering)
                {
                    RaiseShield();
                    timer = 5;
                }
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }
   void RaiseShield()
    {
        raising = true;
        lowering = false;
        finishedRaising = false;
        finishedLowering = false;
        preventShoot = true;
    }
   void LowerShield()
    {
        lowering = true;
        raising = false;
        finishedRaising = false;
        finishedLowering = false;
    }
    /** <summary>Returns true if Enemy should be unable to shoot</summary>
    */
    public bool GetPreventShoot()
    {
        return preventShoot;
    }

    public override void OnHit()
    {
        health-=1;
        if (health <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    void checkShieldForceUp()
    {
        if (transform.parent.parent.TryGetComponent<EnemyWalk>(out var walkScript))
        {
            if (walkScript.GetForceShieldUp())
            {
                forceShieldUp = true;
                if (!finishedRaising)
                {
                    raising = true;
                    lowering = false;
                    finishedRaising = false;
                    finishedLowering = false;
                    preventShoot = true;
                }
            }
            else
            {
                if (forceShieldUp)
                {
                    forceShieldUp = false;
                }
            }
        }
    }
}

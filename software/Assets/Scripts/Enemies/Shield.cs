using UnityEngine;

public class Shield : DestructableObject
{


    bool preventShoot = false;
    float timer = 5;
    Animator animationControl;
    GameObject root;
    bool isUp = false;
    bool isDown = true;
    
    void Start()
    {
        animationControl = gameObject.GetComponentInParent<Animator>();
        root = animationControl.gameObject;
        if (root == null)
        {
            root = transform.parent.gameObject;
        }
    }

    void Update()
    {
        if (!checkShieldForceUp())
        {
            if (timer <= 0)
            {
                isUp = !isUp;
                isDown = !isDown;
                preventShoot = !preventShoot;
                timer = 5;
                Debug.Log(preventShoot);
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else
        {
            Debug.Log("ShieldForced");
            preventShoot = true;
            isUp = true;
            isDown = false;
            timer = 5;
        }

        /*
        checkShieldForceUp();

        if (!forceShieldUp){
            
        }
        */
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

    bool checkShieldForceUp()
    {
        EnemyWalk walkScript = gameObject.GetComponentInParent<EnemyWalk>();
        if (walkScript!= null)
        {
            if (walkScript.GetForceShieldUp())
            {
                return true;
            }
        }
        return false;
    }
}

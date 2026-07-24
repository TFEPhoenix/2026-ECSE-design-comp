using UnityEngine;

public class Shield : DestructableObject
{


    bool preventShoot = false;
    public float raiseSwitchTimer = 5;
    float timer;
    Animator animationControl;
    GameObject root;
    bool isUp = false;
    bool isDown = true;
    
    void Start()
    {
        timer = raiseSwitchTimer;
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
                timer = raiseSwitchTimer;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else
        {
            preventShoot = true;
            isUp = true;
            isDown = false;
            timer = 5;
        }

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
            animationControl.SetBool("Shield", false);
            Destroy(gameObject);
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

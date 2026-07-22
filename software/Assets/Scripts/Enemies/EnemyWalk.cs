using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    bool isWalking = false;
    public Vector3 goal; // Final position
    public float speed; // Distance per second
    Animator animationControl;


    void Start()
    {
        animationControl = gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        if (isWalking){
            if (animationControl != null)
            {
                animationControl.SetBool("Moving", true);
            }
            Vector3 direction = goal - transform.position;
            float curSpeed = speed;
            // If close to the goal, move onto it and stop walking
            if (direction.magnitude < (curSpeed * Time.deltaTime))
            {
                transform.position = goal;
                StoppedWalking();
            }else{
                // Move towards the goal
                transform.position += direction.normalized * curSpeed * Time.deltaTime;
            }


        }
        else if (animationControl != null)
        {
            animationControl.SetBool("Moving", false);
        }
    }
    // Called when the goal is reached
    void StoppedWalking()
    {
        isWalking = false;
    }
    // Begins movement towards a new goal
    public void StartWalking(Vector3 newGoal)
    {
        goal = newGoal;
        isWalking = true;
        Vector3 vectorToGoal = goal - transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(vectorToGoal.x, 0, vectorToGoal.z));
    }
    /** <summary>Returns true if Enemy should be unable to shoot</summary>
    */
    public bool GetPreventShoot()
    {
        return isWalking;
    }

    public bool GetForceShieldUp()
    {
        return isWalking;
    }
    public bool GetIsWalking()
    {
        return isWalking;
    }
}

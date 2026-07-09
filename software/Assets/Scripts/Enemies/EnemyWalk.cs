using UnityEditor.XR;
using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    bool isWalking = false;
    public Vector3 goal; // Final position
    public float speed; // Distance per second
    bool preventShoot = false;
 
    void Update()
    {
        if (isWalking){
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
    }
    // Called when the goal is reached
    void StoppedWalking()
    {
        isWalking = false;
        preventShoot = false;
    }
    // Begins movement towards a new goal
    public void StartWalking(Vector3 newGoal)
    {
        goal = newGoal;
        isWalking = true;
        preventShoot = true;
        Vector3 vectorToGoal = goal - transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(vectorToGoal.x, 0, vectorToGoal.z));
    }
    /** <summary>Returns true if Enemy should be unable to shoot</summary>
    */
    public bool GetPreventShoot()
    {
        return preventShoot;
    }
}

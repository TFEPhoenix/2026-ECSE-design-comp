using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    bool isWalking = false;
    public Vector3 goal; // Final position
    public float speed; // Distance per second
    EnemyShoot shootingScript;
    void Start()
    {
        // Find the part of the enemy that shoots, for the purpose of preventing shooting while moving
        shootingScript = gameObject.GetComponentInChildren<EnemyShoot>();
    }
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
        shootingScript.canShoot = true;
    }
    // Begins movement towards a new goal
    public void StartWalking(Vector3 newGoal)
    {
        goal = newGoal;
        isWalking = true;
        if (shootingScript == null)
        {
            shootingScript = gameObject.GetComponentInChildren<EnemyShoot>();
        }
        shootingScript.canShoot = false;
        Vector3 vectorToGoal = goal - transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(vectorToGoal.x, 0, vectorToGoal.z));
    }
}

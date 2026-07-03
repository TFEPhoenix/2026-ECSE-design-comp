using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class EnemySpawning : MonoBehaviour
{
    
    public GameObject enemyType;
    int curIndex = 0;
    public List<float> timeDelays = new List<float>{1,2,3,4,0.1f, 0.1f}; // In seconds
    public List<Vector3> goalPositions = new List<Vector3>
    {
        new Vector3(-6,2,-14),
        new Vector3(-10,6,-14),
        new Vector3(-18,2,-14),
        new Vector3(-21,2,-14),
    };
    float curCountDown = 0;

    // Update is called once per frame
    void Update()
    {
        // If there are either no more timeDelays stored, or goalPositions, Destroys itself
        if (curIndex >= math.min(timeDelays.Count, goalPositions.Count))
        {
            Destroy(gameObject);
            Destroy(this);
        }


        // Either spawns an enemy or counts down the timer
        if (curCountDown <= 0f){
            SpawnEnemy();
            curCountDown = timeDelays[curIndex];
            curIndex ++;
        }
        else
        {
            curCountDown -= Time.deltaTime;
        }
    }
    /**<summary>Spawns an enemy with a goalPosition, at spawner location</summary>
    */
    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyType, transform.position, Quaternion.LookRotation(new Vector3(0,0,0)));
        // Set the position the enemy is walking to within the enemy
        if (newEnemy.TryGetComponent<EnemyWalk>(out var enemyScript))
        {
            enemyScript.StartWalking(goalPositions[curIndex]);
        }
    }
}

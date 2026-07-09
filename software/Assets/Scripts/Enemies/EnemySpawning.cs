using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class EnemySpawning : MonoBehaviour
{
    
    public GameObject enemyType;
    int curIndex = 0;
    public List<float> timeDelays = new List<float>{1,2,3,4,0.1f, 0.1f}; // In seconds
    List<Vector3> goalPositions = new List<Vector3>();
    float curCountDown = 0;


    void Start()
    {
        if (timeDelays.Count == 0 || transform.childCount == 0)
        {
            Destroy(gameObject);
        }
        // Get goals positions from children
        for (int i = 0; i < transform.childCount; i++)
        {
            goalPositions.Add(transform.GetChild(i).transform.position);
        }
    }
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
        GameObject newEnemy = Instantiate(enemyType, transform.position, Quaternion.LookRotation(new Vector3(0,1,0)));
        // Set the position the enemy is walking to within the enemy
        if (newEnemy.TryGetComponent<EnemyWalk>(out var enemyScript))
        {
            enemyScript.StartWalking(goalPositions[curIndex]);
        }
    }
}

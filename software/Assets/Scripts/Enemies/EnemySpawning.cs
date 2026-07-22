using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;

public class EnemySpawning : MonoBehaviour
{
    
    public GameObject enemyType;
    int curIndex = 0;
    public List<float> timeDelays = new List<float>{1,2,3,4,0.1f, 0.1f}; // In seconds
    List<Vector3> goalPositions = new List<Vector3>();
    float curCountDown = 0;
    bool ignorePosCount = false;
    Transform defaultPos; // After set goalPositions run out, new Spawns goal is defaultPos


    void Start()
    {
        defaultPos = transform.Find("PosDefault");
        if (defaultPos != null)
        {
            ignorePosCount = true;
        }
        if (timeDelays.Count == 0 || transform.childCount == 0)
        {
            Destroy(gameObject);
        }
        // Get goals positions from children
        for (int i = 0; i < transform.childCount; i++)
        {
            goalPositions.Add(transform.GetChild(i).transform.position);
        }
        curCountDown = timeDelays[curIndex];
    }
    // Update is called once per frame
    void Update()
    {

        // Either spawns an enemy or counts down the timer
        if (curCountDown <= 0f){
            SpawnEnemy();
            curIndex ++;
            if (checkOutOfEnemies())
            {
                return;
            }
            curCountDown = timeDelays[curIndex];
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
        GameObject newEnemy = Instantiate(enemyType, transform.parent);
        newEnemy.transform.position = transform.position;
        newEnemy.transform.rotation = Quaternion.LookRotation(new Vector3(0,1,0));
        // Set the position the enemy is walking to within the enemy
        if (newEnemy.TryGetComponent<EnemyWalk>(out var enemyScript))
        {
            if (ignorePosCount && curIndex >= goalPositions.Count)
            {
                enemyScript.StartWalking(defaultPos.position);
            }
            else
            {
                enemyScript.StartWalking(goalPositions[curIndex]);
            }
            
        }
    }
    bool checkOutOfEnemies()
    {
        if (curIndex >= timeDelays.Count || (!ignorePosCount && (curIndex >= goalPositions.Count)))
        {
            Destroy(gameObject);
            Destroy(this);
            return true;
        }
        return false;
    }
}

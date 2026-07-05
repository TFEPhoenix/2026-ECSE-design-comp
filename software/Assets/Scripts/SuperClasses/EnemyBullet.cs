using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    protected void CheckHit()
    {
        if ((Camera.main.transform.parent.GetChild(2).position - transform.position).magnitude <= 0.5)
        {
            Debug.Log(Camera.main.transform.parent.GetChild(2).name);
            Debug.Log("Your in trouble");
        }
    }
}

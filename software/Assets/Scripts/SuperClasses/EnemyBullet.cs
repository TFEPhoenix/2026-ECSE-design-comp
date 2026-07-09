using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    protected void CheckHit()
    {
        if ((Camera.main.transform.parent.Find("HurtPoint").position - transform.position).magnitude <= 0.5)
        {
            LivesManager.Instance.LosePlayer1Life();
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class GenericTargetHitScript : MonoBehaviour, BulletHit
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void OnHit()
    {
        // Should be changed for whatever it is attatched to
        Destroy(gameObject);
    }
    // Update is called once per frame

}

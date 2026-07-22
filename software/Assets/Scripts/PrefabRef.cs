using UnityEngine;

public class PrefabRef : MonoBehaviour
{
    public static PrefabRef Instance;
    public GameObject explosionPrefab;
    public GameObject powerUpPrefab;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public GameObject GetExplosion()
    {
        return explosionPrefab;
    }
    public GameObject GetPowerUp()
    {
        return powerUpPrefab;
    }

    
    

}

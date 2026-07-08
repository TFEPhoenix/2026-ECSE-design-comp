using UnityEngine;

public class TotalPointManager : MonoBehaviour
{
    public static TotalPointManager Instance;

    public int totalPoint = 0;


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


    public void AddPoint(int amount)
    {
        totalPoint += amount;
    }


    public void ResetPoint()
    {
        totalPoint = 0;
    }
}
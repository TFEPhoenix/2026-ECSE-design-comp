using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;


    public float elapsedTime = 0f;


    public bool timerRunning = true;


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


    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }


    public void ResetTimer()
    {
        elapsedTime = 0f;
    }


    public void StopTimer()
    {
        timerRunning = false;
    }


    public void StartTimer()
    {
        timerRunning = true;
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class StageLoadTest : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            transform.Find("P1").Find("P1Fight").gameObject.SetActive(true);
            transform.Find("P2").Find("P2Fight").gameObject.SetActive(false);
        }
        if (Keyboard.current.numpad2Key.wasPressedThisFrame)
        {
            transform.Find("P2").Find("P2Fight").gameObject.SetActive(true);
            transform.Find("P1").Find("P1Fight").gameObject.SetActive(false);
        }
        if (Keyboard.current.numpad5Key.wasPressedThisFrame)
        {
            Camera.main.transform.parent.position = transform.Find("P2").Find("P2Position").transform.position;
        }

        // Check if fights are complete
        // P1
        if (transform.Find("P1/P1Fight").childCount == 0)
        {
            Debug.Log("Fight 1 over");
            transform.Find("P2").Find("P2Fight").gameObject.SetActive(true);
            transform.Find("P1").Find("P1Fight").gameObject.SetActive(false);
            Camera.main.transform.parent.position = transform.Find("P2").Find("P2Position").transform.position;
        }
    }
}

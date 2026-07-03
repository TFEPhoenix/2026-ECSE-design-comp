using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    int speed = 8;
    int turnSpeed = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /**<summary>Low Quality movement of the camera</summary>
    */
    void Move()
    {
        // Horizontal
        if (Keyboard.current.wKey.isPressed)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }

        // Vertical
        if (Keyboard.current.spaceKey.isPressed)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
        if (Keyboard.current.shiftKey.isPressed)
        {
            transform.position -= transform.up * speed * Time.deltaTime;
        }

        // Turn left/Right
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            transform.Rotate(0f, -turnSpeed * Time.deltaTime, 0f);
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            transform.Rotate(0f, turnSpeed * Time.deltaTime, 0f);
        }

        // Look up/down
        if (Keyboard.current.upArrowKey.isPressed)
        {
            transform.Rotate(-turnSpeed * Time.deltaTime, 0f, 0f);
        }

        if (Keyboard.current.downArrowKey.isPressed)
        {
            transform.Rotate(turnSpeed * Time.deltaTime, 0f, 0f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public float timeScale = 1;
    [SerializeField] private float speed = 4;
    [SerializeField] private float sensitivity = 1;
    // Start is called before the first frame update
    void Start()
    {
        timeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ChangeDirection();
        FastForward();
        SlowDown();
    }

    void Move()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            dir += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir += -transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir += -transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir += transform.right;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            dir += transform.up;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            dir += -transform.up;
        }
        transform.position += dir.normalized * (speed * Time.deltaTime);
    }

    void ChangeDirection()
    {
        // Change direction by mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        transform.eulerAngles += new Vector3(-mouseY * sensitivity, mouseX * sensitivity, 0);
    }
    
    void FastForward()
    {
        if (!Input.GetKeyUp(KeyCode.RightArrow)) return;
        Time.timeScale += 0.5f;
        timeScale = Time.timeScale;
    }
    
    void SlowDown()
    {
        if (!Input.GetKeyUp(KeyCode.LeftArrow)) return;
        Time.timeScale -= 0.5f;
        timeScale = Time.timeScale;
    }
}

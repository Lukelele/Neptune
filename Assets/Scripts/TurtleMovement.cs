using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The TurtleMovement class is a MonoBehaviour that controls the movement of a turtle.
/// </summary>
public class TurtleMovement : MonoBehaviour
{
    public float speed = 1.0f;
    private Rigidbody rb;

    Quaternion targetRotation;


    float time = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position - transform.forward * speed * Time.deltaTime);

        time += Time.deltaTime;
        if (time > 5)
        {
            targetRotation = Quaternion.Euler(0.0f, Random.Range(-90.0f, 90.0f), 0.0f);
            time = 0;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
    }
}

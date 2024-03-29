using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : MonoBehaviour
{
    [Range(0.1f, 3.0f)]
    public float moveSpeed = 0.7f;
    public float movementAreaSize = 10f;
    private Vector3 targetPosition;

    void Start()
    {
       targetPosition = new Vector3(0, 10, 0);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            //targetPosition = new Vector3(0, 10, 0);
        }
    }

    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(-movementAreaSize / 2f, movementAreaSize / 2f);
        float randomY = Random.Range(-movementAreaSize / 2f, movementAreaSize / 2f);
        float randomZ = Random.Range(-movementAreaSize / 2f, movementAreaSize / 2f);
        Vector3 center = transform.parent != null ? transform.parent.position : Vector3.zero; 
        return center + new Vector3(randomX, randomY, randomZ);
    }
}
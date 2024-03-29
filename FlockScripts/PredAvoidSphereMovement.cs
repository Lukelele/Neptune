using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredAvoidSphereMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float movementAreaSize = 5f;
    public float avoidanceDistance = 2f; 
    public LayerMask Obstacles;

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = GetRandomPosition();
    }

    void Update()
    {
        AvoidPredators();
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = GetRandomPosition();
        }
    }

    void AvoidPredators()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, avoidanceDistance, Obstacles))
        {
            // If a predator is detected, calculate avoidance direction
            Vector3 avoidanceDirection = transform.position - hit.transform.position;

            // Calculate steering force by blending the avoidance direction with the direction towards the target
            Vector3 desiredDirection = (targetPosition - transform.position).normalized;
            Vector3 steerDirection = (desiredDirection + avoidanceDirection.normalized).normalized;

            // Set the target position to move in the steering direction
            targetPosition = transform.position + steerDirection * avoidanceDistance;
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

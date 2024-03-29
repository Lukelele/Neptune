using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FollowMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Vector3 targetPosition;
    public GameObject goalPrefab;

    void Start()
    {
        targetPosition = Vector3.zero;
        
}

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        targetPosition = goalPrefab.transform.position;
    }


  
}

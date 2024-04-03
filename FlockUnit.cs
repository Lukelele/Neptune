using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockUnit : MonoBehaviour
{

    [SerializeField] private float FOVAngle;
    [SerializeField] private float smoothTime;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Vector3[] directionsToCheckWhenAvoidingObstacles;
    
    private List<FlockUnit> cohesionNeighbours = new List<FlockUnit>();
    private List<FlockUnit> avoidanceNeighbours = new List<FlockUnit>();
    private List<FlockUnit> alignmentNeighbours = new List<FlockUnit>();
    private Flock assignedFlock;
    private Vector3 velocity;
    private float speed;
    
    
    public void AssignFlock(Flock flock)
    {
        assignedFlock = flock;
    }
    
    public void InitializeUnit(float speed)
    {
        this.speed = speed;
    }
    
    public void MoveUnit()
    {
        FindCohesionNeighbours();
        CalculateSpeed();
        Vector3 cohesionVector = CalculateCohesionVector() * assignedFlock.CohesionWeight;
        Vector3 avoidanceVector = CalculateAvoidanceVector() * assignedFlock.AvoidanceWeight;
        Vector3 alignmentVector = CalculateAlignmentVector() * assignedFlock.AlignmentWeight;
        Vector3 boundsVector = CalculateBoundsVector() * assignedFlock.BoundsWeight;
        Vector3 obstacleAvoidanceVector = CalculateObstacleAvoidanceVector() * assignedFlock.ObstacleAvoidanceWeight;
        
        Vector3 moveVector = cohesionVector + avoidanceVector + alignmentVector + boundsVector + obstacleAvoidanceVector;
        moveVector = Vector3.SmoothDamp(transform.forward, moveVector, ref velocity, smoothTime);
        moveVector *= speed;
        transform.forward = moveVector;
        transform.position += moveVector * Time.deltaTime;
    }

    void CalculateSpeed()
    {
        if (cohesionNeighbours.Count == 0) return;
        speed = 0;
        for (int i = 0; i < cohesionNeighbours.Count; i++)
        {
            speed += cohesionNeighbours[i].speed;
        }
        speed /= cohesionNeighbours.Count;
        speed = Mathf.Clamp(speed, assignedFlock.MinSpeed, assignedFlock.MaxSpeed);
    }
    
    private void FindCohesionNeighbours()
    {
        cohesionNeighbours.Clear();
        avoidanceNeighbours.Clear();
        alignmentNeighbours.Clear();
        FlockUnit[] allUnits = assignedFlock.AllUnits;
        for (int i = 0; i < allUnits.Length; i++)
        {
            FlockUnit unit = allUnits[i];
            if (unit == this) continue;
            float distanceSqr = Vector3.SqrMagnitude(unit.transform.position - transform.position);
            if (distanceSqr < assignedFlock.CohesionDistance * assignedFlock.CohesionDistance) cohesionNeighbours.Add(unit);
            if (distanceSqr < assignedFlock.AvoidanceDistance * assignedFlock.AvoidanceDistance) avoidanceNeighbours.Add(unit);
            if (distanceSqr < assignedFlock.AlignmentDistance * assignedFlock.AlignmentDistance) alignmentNeighbours.Add(unit);
        }
    }

    private Vector3 CalculateCohesionVector()
    {
        Vector3 cohesionVector = Vector3.zero;
        if (cohesionNeighbours.Count == 0) return cohesionVector;
        
        int neighboursInFOV = 0;
        for (int i = 0; i < cohesionNeighbours.Count; i++)
        {
            FlockUnit unit = cohesionNeighbours[i];
            if (IsInFOV(unit.transform.position))
            {
                cohesionVector += unit.transform.position;
                neighboursInFOV++;
            }
        }
        
        if (neighboursInFOV == 0) return cohesionVector;
        
        cohesionVector /= neighboursInFOV;
        cohesionVector -= transform.position;
        return cohesionVector.normalized;
    }
    
    private bool IsInFOV(Vector3 neighbourPosition)
    {
        return Vector3.Angle(transform.forward, neighbourPosition - transform.position) <= FOVAngle;
    }
    
    private Vector3 CalculateAvoidanceVector()
    {
        Vector3 avoidanceVector = Vector3.zero;
        if (avoidanceNeighbours.Count == 0) return avoidanceVector;
        
        int neighboursInFOV = 0;
        for (int i = 0; i < avoidanceNeighbours.Count; i++)
        {
            FlockUnit unit = avoidanceNeighbours[i];
            if (IsInFOV(unit.transform.position))
            {
                avoidanceVector += (transform.position - unit.transform.position);
                neighboursInFOV++;
            }
        }
        
        if (neighboursInFOV == 0) return avoidanceVector;
        
        avoidanceVector /= neighboursInFOV;
        return avoidanceVector.normalized;
    }
    
    private Vector3 CalculateAlignmentVector()
    {
        Vector3 alignmentVector = transform.forward;
        if (cohesionNeighbours.Count == 0) return alignmentVector;
        
        int neighboursInFOV = 0;
        for (int i = 0; i < alignmentNeighbours.Count; i++)
        {
            FlockUnit unit = alignmentNeighbours[i];
            if (IsInFOV(unit.transform.position))
            {
                alignmentVector += unit.transform.forward;
                neighboursInFOV++;
            }
        }
        
        if (neighboursInFOV == 0) return alignmentVector;
        
        alignmentVector /= neighboursInFOV;
        return alignmentVector.normalized;
    }
    
    private Vector3 CalculateBoundsVector()
    {
        Vector3 offsetToCenter = assignedFlock.transform.position - transform.position;
        bool isNearBound = offsetToCenter.magnitude >= assignedFlock.BoundsDistance * 0.9f;
        return isNearBound ? offsetToCenter.normalized : Vector3.zero;
    }
    
    private Vector3 CalculateObstacleAvoidanceVector()
    {
        Vector3 obstacleAvoidanceVector = Vector3.zero;

        if (Physics.Raycast(transform.position, transform.forward, out _, assignedFlock.ObstacleAvoidanceDistance, obstacleMask))
        {
            obstacleAvoidanceVector = FindBestDirectionToAvoidObstacle();
        }
        
        return obstacleAvoidanceVector;
    }
    
    private Vector3 FindBestDirectionToAvoidObstacle()
    {
        float maxDistance = 0;
        Vector3 bestDirection = Vector3.zero;
        
        for (int i = 0; i < directionsToCheckWhenAvoidingObstacles.Length; i++)
        {
            Vector3 direction = transform.TransformDirection(directionsToCheckWhenAvoidingObstacles[i]);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, assignedFlock.ObstacleAvoidanceDistance, obstacleMask))
            {
                float distance = Vector3.SqrMagnitude(hit.point - transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    bestDirection = direction;
                }
            }
            else
            {
                return direction.normalized;
            }
        }

        return bestDirection.normalized;
    }
}

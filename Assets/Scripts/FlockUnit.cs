using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Flock Unit represents a single unit in a flock, inherits from Animal.
/// </summary>
public class FlockUnit : Animal
{
    [Header("Flock Unit Settings")]
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

    private void FixedUpdate()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Reproduce();
        // }
    }

    /// <summary>
    /// Assigns the flock to the unit.
    /// </summary>
    public void AssignFlock(Flock flock)
    {
        assignedFlock = flock;
    }
    
    /// <summary>
    /// Initializes the unit with the given speed.
    /// </summary>
    public void InitializeUnit(float speed)
    {
        this.speed = speed;
    }
    
    /// <summary>
    /// Moves the unit based on the flocking rules.
    /// </summary>
    public void MoveUnit()
    {
        FindCohesionNeighbours();
        CalculateSpeed();
        // Calculate the vectors for cohesion, avoidance, alignment, bounds, and obstacle avoidance
        Vector3 cohesionVector = CalculateCohesionVector() * assignedFlock.CohesionWeight;
        Vector3 avoidanceVector = CalculateAvoidanceVector() * assignedFlock.AvoidanceWeight;
        Vector3 alignmentVector = CalculateAlignmentVector() * assignedFlock.AlignmentWeight;
        Vector3 boundsVector = CalculateBoundsVector() * assignedFlock.BoundsWeight;
        Vector3 obstacleAvoidanceVector = CalculateObstacleAvoidanceVector() * assignedFlock.ObstacleAvoidanceWeight;
        
        // Combine the vectors and move the unit
        Vector3 moveVector = cohesionVector + avoidanceVector + alignmentVector + boundsVector + obstacleAvoidanceVector;
        moveVector = Vector3.SmoothDamp(transform.forward, moveVector, ref velocity, smoothTime);
        moveVector *= speed;
        transform.forward = moveVector;
        transform.position += moveVector * Time.deltaTime;
    }

    /// <summary>
    /// Calculates the speed of the unit based on the speed of its neighbours.
    /// </summary>
    void CalculateSpeed()
    {
        if (cohesionNeighbours.Count == 0) return;
        speed = 0;
        for (int i = 0; i < cohesionNeighbours.Count; i++)
        {
            speed += cohesionNeighbours[i].speed;
        }
        speed /= cohesionNeighbours.Count;
        speed = Mathf.Clamp(speed, assignedFlock.MinFlockUnitSpeed, assignedFlock.MaxFlockUnitSpeed);
    }
    
    /// <summary>
    /// Finds the neighbours of the unit for cohesion, avoidance, and alignment.
    /// </summary>
    private void FindCohesionNeighbours()
    {
        cohesionNeighbours.Clear();
        avoidanceNeighbours.Clear();
        alignmentNeighbours.Clear();
        List<FlockUnit> allUnits = assignedFlock.AllUnits;

        // Find the neighbours of the unit
        for (int i = 0; i < allUnits.Count; i++)
        {
            FlockUnit unit = allUnits[i];
            if (unit == this) continue;
            float distanceSqr = Vector3.SqrMagnitude(unit.transform.position - transform.position);
            if (distanceSqr < assignedFlock.CohesionDistance * assignedFlock.CohesionDistance) cohesionNeighbours.Add(unit);
            if (distanceSqr < assignedFlock.AvoidanceDistance * assignedFlock.AvoidanceDistance) avoidanceNeighbours.Add(unit);
            if (distanceSqr < assignedFlock.AlignmentDistance * assignedFlock.AlignmentDistance) alignmentNeighbours.Add(unit);
        }
    }

    /// <summary>
    /// Calculates the cohesion vector for the unit.
    /// </summary>
    /// <returns>The cohesion vector.</returns>
    private Vector3 CalculateCohesionVector()
    {
        Vector3 cohesionVector = Vector3.zero;
        if (cohesionNeighbours.Count == 0) return cohesionVector;
        
        int neighboursInFOV = 0;
        // Calculate the average position of the neighbours in the unit's field of view
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
    
    /// <summary>
    /// Checks if the given position is in the unit's field of view.
    /// </summary>
    /// <returns>True if the position is in the field of view, false otherwise.</returns>
    private bool IsInFOV(Vector3 neighbourPosition)
    {
        return Vector3.Angle(transform.forward, neighbourPosition - transform.position) <= FOVAngle;
    }
    
    /// <summary>
    /// Calculates the avoidance vector for the unit.
    /// </summary>
    /// <returns>The avoidance vector.</returns>
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
    
    /// <summary>
    /// Calculates the alignment vector for the unit.
    /// </summary>
    /// <returns>The alignment vector.</returns>
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
    
    /// <summary>
    /// Calculates the bounds vector for the unit.
    /// </summary>
    /// <returns>The bounds vector.</returns>
    private Vector3 CalculateBoundsVector()
    {
        Vector3 offsetToCenter = assignedFlock.transform.position - transform.position;
        bool isNearBound = offsetToCenter.magnitude >= assignedFlock.BoundsDistance * 0.9f;
        return isNearBound ? offsetToCenter.normalized : Vector3.zero;
    }
    
    /// <summary>
    /// Calculates the obstacle avoidance vector for the unit.
    /// </summary>
    /// <returns>The obstacle avoidance vector.</returns>
    private Vector3 CalculateObstacleAvoidanceVector()
    {
        Vector3 obstacleAvoidanceVector = Vector3.zero;

        if (Physics.Raycast(transform.position, transform.forward, out _, assignedFlock.ObstacleAvoidanceDistance, obstacleMask))
        {
            obstacleAvoidanceVector = FindBestDirectionToAvoidObstacle();
        }
        
        return obstacleAvoidanceVector;
    }
    

    /// <summary>
    /// Finds the best direction to avoid an obstacle.
    /// </summary>
    /// <returns>The best direction to avoid the obstacle.</returns>
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
    

    /// <summary>
    /// Eats the given plant and gains energy.
    /// </summary>
    public override void GainEnergy(float amount)
    {
        List<FlockUnit> allUnits = assignedFlock.AllUnits;
        float averageEnergy = amount / allUnits.Count;
        for (int i = 0; i < allUnits.Count; i++)
        {
            FlockUnit unit = allUnits[i];
            unit.energy += averageEnergy;
        }
    }
    
    /// <summary>
    /// Reproduces the unit and adds the child to the flock.
    /// </summary>
    /// <returns>The child GameObject.</returns>
    protected override GameObject Reproduce()
    {
        //if (GameObject.FindGameObjectsWithTag("Fish").Length >= 30) return null;
        GameObject child = base.Reproduce();
        FlockUnit childScript = child.GetComponent<FlockUnit>();
        childScript.generation = generation;
        assignedFlock.AddUnit(childScript);
        return child;
    }
}

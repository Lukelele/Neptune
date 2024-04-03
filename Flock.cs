using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    
    [Header("Spawn Setup")]
    [SerializeField] private FlockUnit flockUnitPrefab;
    [SerializeField] private int flockSize;
    [SerializeField] private Vector3 spawnBounds;
    
    [Header("Speed Setup")]
    [Range(0, 10)]
    [SerializeField] private float minSpeed;
    public float MinSpeed => minSpeed;
    [Range(0, 10)]
    [SerializeField] private float maxSpeed;
    public float MaxSpeed => maxSpeed;
    
    [Header("Detection Distances")]
    [Range(0, 10)]
    [SerializeField] private float cohesionDistance;
    public float CohesionDistance => cohesionDistance;
    
    [Range(0, 10)]
    [SerializeField] private float avoidanceDistance;
    public float AvoidanceDistance => avoidanceDistance;
    
    [Range(0, 10)]
    [SerializeField] private float alignmentDistance;
    public float AlignmentDistance => alignmentDistance;
    
    [Range(0, 100)]
    [SerializeField] private float boundsDistance;
    public float BoundsDistance => boundsDistance;
    
    [Range(0, 100)]
    [SerializeField] private float obstacleAvoidanceDistance;
    public float ObstacleAvoidanceDistance => obstacleAvoidanceDistance;
    
    
    [Header("Weights")]
    [Range(0, 10)]
    [SerializeField] private float cohesionWeight;
    public float CohesionWeight => cohesionWeight;
    
    [Range(0, 10)]
    [SerializeField] private float avoidanceWeight;
    public float AvoidanceWeight => avoidanceWeight;
    
    [Range(0, 10)]
    [SerializeField] private float alignmentWeight;
    public float AlignmentWeight => alignmentWeight;
    
    [Range(0, 10)]
    [SerializeField] private float boundsWeight;
    public float BoundsWeight => boundsWeight;
    
    [Range(0, 100)]
    [SerializeField] private float obstacleAvoidanceWeight;
    public float ObstacleAvoidanceWeight => obstacleAvoidanceWeight;
    
    public FlockUnit[] AllUnits { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateUnits();
    }
    
    void Update()
    {
        for (var i = 0; i < AllUnits.Length; i++)
        {
            AllUnits[i].MoveUnit();
        }
    }

    private void GenerateUnits()
    {
        AllUnits = new FlockUnit[flockSize];
        for (int i = 0; i < flockSize; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-spawnBounds.x, spawnBounds.x), 
                Random.Range(-spawnBounds.y, spawnBounds.y),
                Random.Range(-spawnBounds.z, spawnBounds.z));
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            AllUnits[i] = Instantiate(flockUnitPrefab, pos, rot);
            AllUnits[i].AssignFlock(this);
            AllUnits[i].InitializeUnit(Random.Range(minSpeed, maxSpeed));
        }
    }
}

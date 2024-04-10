using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : Creature
{
    [Header("Flock Settings")]
    public int maxFlockSize;

    [Header("Spawn Setup")]
    [SerializeField] private FlockUnit flockUnitPrefab;
    [SerializeField] private VisionCone vision;
    [SerializeField] private int flockSize;
    [SerializeField] private Vector3 spawnBounds;
    
    [Header("Speed Setup")]
    [Range(0, 10)]
    [SerializeField] private float minSpeed = 1;
    public float MinSpeed => minSpeed;
    [Range(0, 10)]
    [SerializeField] private float maxSpeed = 2;
    public float MaxSpeed => maxSpeed;
    
    [Header("Detection Distances")]
    [Range(0, 10)]
    [SerializeField] private float cohesionDistance = 5;
    public float CohesionDistance => cohesionDistance;
    
    [Range(0, 10)]
    [SerializeField] private float avoidanceDistance = 2;
    public float AvoidanceDistance => avoidanceDistance;
    
    [Range(0, 10)]
    [SerializeField] private float alignmentDistance = 2;
    public float AlignmentDistance => alignmentDistance;
    
    [Range(0, 100)]
    [SerializeField] private float boundsDistance = 1;
    public float BoundsDistance => boundsDistance;
    
    [Range(0, 100)]
    [SerializeField] private float obstacleAvoidanceDistance = 20;
    public float ObstacleAvoidanceDistance => obstacleAvoidanceDistance;
    
    
    [Header("Weights")]
    [Range(0, 10)]
    [SerializeField] private float cohesionWeight = 1;
    public float CohesionWeight => cohesionWeight;
    
    [Range(0, 10)]
    [SerializeField] private float avoidanceWeight = 10;
    public float AvoidanceWeight => avoidanceWeight;
    
    [Range(0, 10)]
    [SerializeField] private float alignmentWeight = 1;
    public float AlignmentWeight => alignmentWeight;
    
    [Range(0, 10)]
    [SerializeField] private float boundsWeight = 10;
    public float BoundsWeight => boundsWeight;
    
    [Range(0, 100)]
    [SerializeField] private float obstacleAvoidanceWeight = 100;
    public float ObstacleAvoidanceWeight => obstacleAvoidanceWeight;
    
    public List<FlockUnit> AllUnits { get; set; }

    public Net brain;
    private Transform _target;
    public bool drawVision;
    public Dimension motionDimension;


    void Awake()
    {
        brain = new Net (new[] { 2, 2 });
        AllUnits = new List<FlockUnit>();
        drawVision = true;
        age = 0;
    }

    
    void Update()
    {
        age += Time.deltaTime;

        if (drawVision)
        {
            Debug.DrawRay(transform.position, vision.closetFoodDir, Color.green);
            Debug.DrawRay(transform.position, vision.closetTerrainDir, Color.red);
        }

        Quaternion foodLookRotation = Quaternion.LookRotation(vision.closetFoodDir);
        Vector3 foodRotationDifference = foodLookRotation.eulerAngles - transform.rotation.eulerAngles;
        
        foodRotationDifference /= 360;
        
        
        float[] inputs = { foodRotationDifference.x, foodRotationDifference.y };
        
        brain.FeedForward(inputs);
        float[] outputs = brain.GetOutput();
        Vector3 angleToRotate = new Vector3(outputs[0], outputs[1], 0);
        angleToRotate *= 360;

        switch (motionDimension)
        {
            case Dimension.XY:
                transform.Rotate(0, angleToRotate.y*Time.timeScale*Time.deltaTime, 0);
                break;
            case Dimension.XYZ:
                Quaternion final = Quaternion.Euler(transform.eulerAngles + angleToRotate);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, final, Time.deltaTime*Time.timeScale);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                break;
        }
        
        transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));

        ManageFlock();
    }

    public void GenerateUnits(int numUnits = 0)
    {
        for (int i = 0; i < numUnits; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-spawnBounds.x, spawnBounds.x), 
                Random.Range(-spawnBounds.y, spawnBounds.y),
                Random.Range(-spawnBounds.z, spawnBounds.z));
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            AllUnits.Add(Instantiate(flockUnitPrefab, pos, rot));
            AllUnits[i].AssignFlock(this);
            AllUnits[i].InitializeUnit(Random.Range(minSpeed, maxSpeed));
        }
    }


    private void ManageFlock() {
        if (AllUnits.Count == 0) {
            Destroy(gameObject);
        }

        if (AllUnits.Count > maxFlockSize) {
            Flock newFlock = Instantiate(this, transform.position, Quaternion.identity);
            // mutate the new flock
            newFlock.brain = new Net(brain.CopyLayers());
            newFlock.Mutate();

            for (int i = maxFlockSize; i < AllUnits.Count; i++) {
                FlockUnit unit = AllUnits[i];
                AllUnits.RemoveAt(i);
                newFlock.AddUnit(unit);
            }
        }
    }

    public void AddUnit(FlockUnit unit)
    {
        AllUnits.Add(unit);
        AllUnits[AllUnits.Count - 1].AssignFlock(this);
        AllUnits[AllUnits.Count - 1].InitializeUnit(Random.Range(minSpeed, maxSpeed));
    }

    public void CreateUnit() {
        AllUnits.Add(Instantiate(flockUnitPrefab, transform.position, Quaternion.identity));
        AllUnits[AllUnits.Count - 1].AssignFlock(this);
        AllUnits[AllUnits.Count - 1].InitializeUnit(Random.Range(minSpeed, maxSpeed));
    }

    private void Mutate()
    {
        float mutationProbability = Random.Range(0f, 1.0f);
        float mutationMagnitude = Random.Range(0f, 1.0f);

        brain.MutateNet(mutationProbability, mutationMagnitude);
    }

}
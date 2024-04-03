using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlockLeader : Creature
{
    public GameObject fishPrefab;

    public Net brain;

    public VisionCone vision;
    private float[] outputs;
    public int foodCount;


    [Header("Flock Settings")]
    [SerializeField] private List<string> seekingTags;
    [SerializeField] private Dimension motionDimension;
    [SerializeField] private bool drawVision;
    [SerializeField] public float turningRate;

    int numFish = 25;
    int spawnarea = 5;
    public GameObject[] fishList;

    [Range(0.1f, 3.0f)]
    public float cohesionWeight = 1.0f; // Weight for cohesion behavior
    [Range(0.1f, 3.0f)]
    public float alignmentWeight = 1.0f; // Weight for alignment behavior
    [Range(0.1f, 3.0f)]
    public float avoidanceWeight = 1.0f; // Weight for avoidance behavior

    [Range(0.5f, 3.0f)]
    public float flockSpeed = 1.0f;


    void Start()
    {
        drawVision = true;
        brain = new Net(new[] { 2, 2 });
        age = 0;
        turningRate = 3;


        fishList = new GameObject[numFish];

        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-spawnarea, spawnarea),
                                                           Random.Range(-spawnarea, spawnarea),
                                                           Random.Range(-spawnarea, spawnarea));
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            fishList[i] = (GameObject)Instantiate(fishPrefab, pos, rot);
        }
    }


    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockLeader : MonoBehaviour
{
    public GameObject fishPrefab;

    int numFish = 25;
    int spawnarea = 5;
    public GameObject[] allFish;
    public int tankSize = 20;


    [Range(0.1f, 3.0f)]
    public float cohesionWeight = 1.0f; // Weight for cohesion behavior
    [Range(0.1f, 3.0f)]
    public float alignmentWeight = 1.0f; // Weight for alignment behavior
    [Range(0.1f, 3.0f)]
    public float avoidanceWeight = 1.0f; // Weight for avoidance behavior

    [Range(0.5f, 3.0f)]
    public float speed = 1.0f;


    void Start()
    {
        allFish = new GameObject[numFish];

        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-spawnarea, spawnarea),
                                                           Random.Range(-spawnarea, spawnarea),
                                                           Random.Range(-spawnarea, spawnarea));
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, rot);
        }
    }


    void Update()
    {
        
    }
}

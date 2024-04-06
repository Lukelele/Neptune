using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject flockPrefab;
    [SerializeField] private int flockSize;
    [SerializeField] private int numFlocks;
    [SerializeField] private Vector3 spawnBounds;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numFlocks; i++) {
            GameObject flock = Instantiate(flockPrefab, transform.position + new Vector3(Random.Range(-spawnBounds.x, spawnBounds.x), Random.Range(-spawnBounds.y, spawnBounds.y), Random.Range(-spawnBounds.z, spawnBounds.z)), Quaternion.identity);
            flock.GetComponent<Flock>().GenerateUnits(flockSize);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

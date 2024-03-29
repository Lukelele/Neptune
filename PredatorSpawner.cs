using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject predatorPrefab;
    [SerializeField] private float spawnRange = 20;
    [SerializeField] private int threshold = 3;
    [SerializeField] private Dimension spawnDimension;
    private void FixedUpdate()
    {
        GameObject[] predators = GameObject.FindGameObjectsWithTag("Predator");
        if (predators.Length < threshold)
        {
            SpawnPredator();
        }
    }
    
    void SpawnPredator()
    {
        Vector3 spawnPos = Vector3.zero;
        if (spawnDimension == Dimension.XYZ)
            spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));
        else if (spawnDimension == Dimension.XY)
            spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0.5f, Random.Range(-spawnRange, spawnRange));
        Instantiate(predatorPrefab, spawnPos, Quaternion.identity);
    }
}

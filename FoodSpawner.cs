using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private float initialFood = 50;
    //[SerializeField] private float spawnRate = 5;
    [SerializeField] private float spawnRange = 20;
    [SerializeField] private float spawnCap = 1500;
    private float _spawnRate = 0;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private Dimension spawnDimension;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < initialFood; i++)
        {
            SpawnFood();
        }
    }


    void FixedUpdate() {
        if (GameObject.FindGameObjectsWithTag("Food").Length < spawnCap) SpawnFood();
    }
    
    void SpawnFood()
    {
        Vector3 spawnPos = Vector3.zero;
        if (spawnDimension == Dimension.XYZ) 
            spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));
        else if (spawnDimension == Dimension.XY) 
            spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0.5f, Random.Range(-spawnRange, spawnRange));
        Instantiate(foodPrefab, spawnPos, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flockSpawner : MonoBehaviour
{
    public int spawnarea = 20;
    public int tankSize = 20;
    public int numLeaders = 1;
    public GameObject leaderPrefab;
    

    void Start()
    {
        for(int i = 0; i < numLeaders; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-spawnarea, spawnarea),
                                      Random.Range(-spawnarea, spawnarea),
                                      Random.Range(-spawnarea, spawnarea));
            Instantiate(leaderPrefab, pos, Quaternion.identity);
        }
        
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(tankSize, tankSize, tankSize));
    }
}
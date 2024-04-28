using UnityEngine;

public class FlockSpawner : Spawner
{
    [SerializeField] private int unitsPerFlock;
    
    protected override void Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            GameObject flock = Instantiate(prefab, randomPos, Quaternion.identity);
            Flock flockScript = flock.GetComponent<Flock>();
            for (int j = 0; j < unitsPerFlock; j++)
            {
                flockScript.CreateUnit();
            }
        }
    }
}
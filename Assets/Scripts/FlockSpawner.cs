using UnityEngine;


/// <summary>
/// The FlockSpawner class inherits from the Spawner class. It represents a spawner that spawns flocks of units.
/// </summary>
public class FlockSpawner : Spawner
{
    [SerializeField] private int unitsPerFlock;
    
    protected override void Spawn(int count)
    {
        // spawn flocks based on the count
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
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected Vector2 spawnXRange;
    [SerializeField] protected Vector2 spawnYRange;
    [SerializeField] protected Vector2 spawnZRange;
    
    [SerializeField] protected GameObject[] prefabs;
    [SerializeField][TagSelector] protected string prefabTag;
    [SerializeField] protected int minCount;
    
    [SerializeField] public bool enable;
    
    protected Vector3 randomPos => new(
        Random.Range(spawnXRange.x, spawnXRange.y),
        Random.Range(spawnYRange.x, spawnYRange.y),
        Random.Range(spawnZRange.x, spawnZRange.y)
    );


    private void FixedUpdate()
    {
        int count = GameObject.FindGameObjectsWithTag(prefabTag).Length;
        if (count < minCount)
        {
            Spawn(minCount - count);
        }
    }
    
    protected virtual void Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            Instantiate(prefab, randomPos, Quaternion.identity);
        }
    }
    
    public void SetMinCount(int count)
    {
        minCount = count;
    }
    
    public void SetMinCount(float count)
    {
        minCount = (int) count;
    }
    
    public void ToggleEnable()
    {
        enable = !enable;
    }
}

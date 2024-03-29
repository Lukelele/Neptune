using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantSpawner : MonoBehaviour
{
    // 100% definitely my own work adn not from a tutorial. Works almost perfectly, just make sure using layers
    // and setting them in the unity editor. Probably can change to using tags tbf but this made selection 
    // of layers easier. Also spawns in a square but not really noticeable.
    [Header("Spawn Settings")]
    public GameObject plantPrefab;
    public float spawnChance;

    [Header("Raycast Settings")]
    public float distanceBetweenChecks;
    public float heightOfCheck = 10f, rangeOfCheck=30f;
    public LayerMask layerMaskCheck;
    public Vector2 positivePosition, negativePosition;

    [SerializeField] LayerMask foodSpawnerPlantLayerMask;
    private void Start() {
        SpawnPlant();
    }
    void SpawnPlant() {

        for (float x=negativePosition.x; x<positivePosition.x; x+=distanceBetweenChecks) {
            for (float z=negativePosition.y; z<positivePosition.y; z+=distanceBetweenChecks){
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightOfCheck, z), Vector3.down, out hit, rangeOfCheck, layerMaskCheck)) {
                    if (spawnChance > Random.Range(0,101)) {
                        GameObject child = Instantiate(plantPrefab, hit.point, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
                        child.name = "FoodSpawnerPlant";
                        if (foodSpawnerPlantLayerMask >= 0) {  // If layerMask is too large, turns negative.
                            // Dont even ask, layers are weird, but this fixes an issue for them.
                            child.layer = (int)Mathf.Log(foodSpawnerPlantLayerMask.value, 2);
                        } else {
                            Debug.LogError("LayerMask too large!");

                        }
                    }    
                }
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject plantPrefab;
    [SerializeField] private float spawnChance = 5;
    private Vector3 plantSize;
    private GameObject child;

    [Header("Raycast Settings")]
    [SerializeField] private float distanceBetweenChecks = 2;
    private float heightOfCheck = 10f, rangeOfCheck=30f;
    [SerializeField] private LayerMask layerMaskCheck;
    [SerializeField] private Vector2 positivePosition, negativePosition;

    [SerializeField] private LayerMask foodSpawnerPlantLayerMask;
    private void Start() {
        SpawnPlant();
    }
    void SpawnPlant() {

        for (float x=negativePosition.x; x<positivePosition.x; x+=distanceBetweenChecks) {
            for (float z=negativePosition.y; z<positivePosition.y; z+=distanceBetweenChecks){
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightOfCheck, z), Vector3.down, out hit, rangeOfCheck, layerMaskCheck)) {
                    if (spawnChance > Random.Range(0,101)) {
                        
                        child = Instantiate(plantPrefab, hit.point, Quaternion.Euler(new Vector3(0, Random.Range(0, 30), 0)), transform);
                        child.name = "FoodSpawnerPlant";
                        
                        plantSize = child.transform.localScale;
                        plantSize[1] = plantSize[1] * Random.Range(0.8f,1.25f);;

                        child.transform.localScale = plantSize;
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


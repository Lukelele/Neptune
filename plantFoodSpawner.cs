using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour
{
    [SerializeField] private int maxFood;
    public GameObject foodPrefab;
    private float time = 0f;
    private float plantHeight = 2f;
    private float surfaceDepth = 16f; // Height of surface
    private float plantY = 0f;
    private float plantDepth = 0f;
    private double growRate = 0f;
    private Vector3 position;
    private Quaternion rotation;
    [SerializeField] private int stackSize = 1;

    void Start() {
        maxFood = 16;
        plantY = transform.position.y;
        plantDepth = surfaceDepth - plantY;
        growRate = 0 + 2.5*(plantDepth/surfaceDepth); // Calculates a growth rate for the plant based on its distance from surface
    }

    public void Update() { // Timer
        time += Time.deltaTime;

        if ((time > growRate) & (stackSize < maxFood)) { // Timer up, add food object as child
            position = transform.position + Vector3.up*(stackSize * plantHeight/10);
            rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)); // random rotation
            GameObject childGameObject = Instantiate(foodPrefab, position, rotation);
            childGameObject.transform.SetParent(transform.gameObject.transform);
            stackSize += 1;
            time = 0; // reset timer
        }
    }
}

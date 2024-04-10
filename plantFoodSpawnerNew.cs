using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlantScriptNew : MonoBehaviour
{
    [SerializeField] private int maxFood;
    private float time = 0f;
    private float surfaceDepth = 18f; // Height of surface
    private float plantY = 0f;
    private float plantDepth = 0f;
    private double growRate = 0f;
    private Vector3 position;
    private Quaternion rotation;
    [SerializeField] private int stackSize = 0;

    [SerializeField]private List<GameObject> childrenLeaves = new List<GameObject>();

    void Start() {

        plantY = transform.position.y;
        plantDepth = surfaceDepth - plantY;
        growRate = 0 + (plantDepth/surfaceDepth); // Calculates a growth rate for the plant based on its distance from surface
    
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Food")
            {
                childrenLeaves.Add(child.gameObject);
            }
        }

        maxFood = childrenLeaves.Count;

    }

    public void Update() { // Timer
        time += Time.deltaTime;

        if ((time > growRate) & (stackSize < maxFood)) { // Timer up, add food object as child

            childrenLeaves[stackSize].transform.gameObject.SetActive(true);

            stackSize += 1;
            time = 0; // reset timer
        }
    }
}

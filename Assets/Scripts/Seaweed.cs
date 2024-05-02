using System;
using UnityEngine;


/// <summary>
/// The Seaweed class is a Plant that grows in the ocean.
/// </summary>
public class Seaweed : Plant
{
    [Header("Seaweed Settings")]
    [SerializeField] private float maxSizeEnergy = 60f;
    
    private void Awake()
    {
        // Set the seaweed on the terrain
        transform.localScale = Vector3.zero;
        SetOnTerrain(transform);
    }

    private void FixedUpdate()
    {
        // Grow the seaweed based on its energy level
        float height = Mathf.Clamp(energy/maxSizeEnergy * 1.5f, 0, 1.5f);
        float width = Mathf.Clamp(energy/maxSizeEnergy * 1.1f, 0.5f, 1.1f);
        transform.localScale = new Vector3(width, height, width);
    }
}
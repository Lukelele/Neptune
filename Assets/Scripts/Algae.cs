using UnityEngine;
using System;


/// <summary>
/// The Algae class inherits from the Plant class. It represents an algae object in the game.
/// </summary>
public class Algae : Plant
{
    [Header("Algae Settings")]
    [SerializeField] private float maxSizeEnergy = 60f;

    [SerializeField] private GameObject[] leaves;
    

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// It binds the child GameObjects to the leaves array and sets the algae on the terrain.
    /// </summary>
    private void Awake()
    {
        BindChild();
        SetOnTerrain(transform, 1.65f);
    }
    

    /// <summary>
    /// FixedUpdate is called every fixed framerate frame.
    /// It sets each leaf active or inactive based on the current energy level of the algae.
    /// </summary>
    private void FixedUpdate()
    {
        for (int i = 0; i < leaves.Length; i++)
        {
            leaves[i].SetActive(i < energy / maxSizeEnergy * leaves.Length);
        }
    }
    

    /// <summary>
    /// BindChild method initializes the leaves array with the child GameObjects of the algae.
    /// </summary>
    public void BindChild()
    {
        leaves = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            leaves[i] = transform.GetChild(i).gameObject;
        }
    }
}
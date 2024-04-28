using UnityEngine;
using System;

public class Algae : Plant
{
    [Header("Algae Settings")]
    [SerializeField] private float maxSizeEnergy = 60f;

    [SerializeField] private GameObject[] leaves;
    
    private void Awake()
    {
        BindChild();
        SetOnTerrain(transform, 1.65f);
    }
    
    private void FixedUpdate()
    {
        for (int i = 0; i < leaves.Length; i++)
        {
            leaves[i].SetActive(i < energy / maxSizeEnergy * leaves.Length);
        }
    }
    
    public void BindChild()
    {
        leaves = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            leaves[i] = transform.GetChild(i).gameObject;
        }
    }
}
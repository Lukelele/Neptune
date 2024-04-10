using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCubeSpawnerScipt : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    void Start()
    {
        Instantiate(cubePrefab, new Vector3(0,20,0), Quaternion.Euler(new Vector3(0, 0, 0)), transform);
        Instantiate(cubePrefab, new Vector3(0,30,0), Quaternion.Euler(new Vector3(0, 0, 0)), transform);
    }
}

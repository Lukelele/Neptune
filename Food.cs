using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] public float energy = 5;
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Prey":
                FlockUnit flockUnit = other.GetComponent<FlockUnit>();
                flockUnit.energy += energy;
                Destroy(gameObject);
                break;
        }
    }
    
}

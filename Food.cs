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
            case "Predator":
                Predator predator = other.GetComponent<Predator>();
                predator.energy += energy;
                predator.foodCount++;
                //Debug.Log("Eat" + predator.foodCount);
                Destroy(gameObject);
                break;
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodNew : MonoBehaviour
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
                
                gameObject.SetActive(false);
                break;
        }
    }
    
}

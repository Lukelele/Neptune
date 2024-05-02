using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The Animal class is a subclass of the Organism class. It represents an animal in the game.
/// </summary>
public class Animal : Organism
{
    [Header("Animal Settings")]
    [SerializeField] protected float energyConsume =  1;
    [SerializeField] protected float energyConsumeInterval = 1;
    private float _energyConsumeTimer = 0;
    [SerializeField] private GameObject deathEffect;
    
    [SerializeField] protected float reproductionEnergyCost = 20;


    /// <summary>
    /// Manages the energy of the animal. This includes consuming energy at regular intervals, dying when energy is depleted, and reproducing when energy is sufficient.
    /// </summary>
    protected override void ManageEnergy()
    {
        base.ManageEnergy();
        if (!isAlive) return;
        _energyConsumeTimer -= Time.deltaTime;
        if (_energyConsumeTimer <= 0)
        {
            energy -= energyConsume;
            _energyConsumeTimer = energyConsumeInterval;
        }
        
        if (energy <= 0)
        {
            OnDeath();
            Destroy(gameObject);
            isAlive = false;
        }

        if (energy >= reproductionEnergyCost)
        {
            Reproduce();
        }
    }


    /// <summary>
    /// Reproduce method creates a new animal object with half the energy of the parent animal.
    /// </summary>
    /// <returns>The new animal object.</returns>
    protected override GameObject Reproduce()
    {
        energy -= reproductionEnergyCost * 0.5f;
        return base.Reproduce();
    }

    /// <summary>
    /// OnDeath method is called when the animal dies. It instantiates the death effect at the animal's position.
    /// </summary>
    protected override void OnDeath()
    {
        base.OnDeath();
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        
    }
}

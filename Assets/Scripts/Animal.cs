using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Organism
{
    
    [Header("Animal Settings")]
    [SerializeField] protected float energyConsume =  1;
    [SerializeField] protected float energyConsumeInterval = 1;
    private float _energyConsumeTimer = 0;
    [SerializeField] private GameObject deathEffect;
    
    [SerializeField] protected float reproductionEnergyCost = 20;

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
            //transform.Rotate(-90, 0, 0);
            OnDeath();
            Destroy(gameObject);
            isAlive = false;
            
            //string path = "Assets/LifeStats.txt";
            //string content = generation + "," + age + "\n";
            //if (!System.IO.File.Exists(path)) System.IO.File.WriteAllText(path, "");
            //System.IO.File.AppendAllText(path, content);
        }

        if (energy >= reproductionEnergyCost)
        {
            Reproduce();
        }
    }

    protected override GameObject Reproduce()
    {
        energy -= reproductionEnergyCost * 0.5f;
        return base.Reproduce();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Instantiate(deathEffect, transform.position, Quaternion.identity);
    }
}

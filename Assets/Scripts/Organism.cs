using System;
using UnityEngine;


/// <summary>
/// The Organism class is a MonoBehaviour that represents an organism in the game.
/// </summary>
public class Organism : MonoBehaviour
{
    [Header("Organism Settings")]
    [SerializeField] public float energy = 10;
    [SerializeField][TagSelector] public string[] MyPredators;
    [SerializeField][TagSelector] public string[] MyPreys;
    public int generation = 1;
    public float age;
    protected bool isAlive = true;
    
    protected virtual void Update()
    {
        age += Time.deltaTime;
        ManageEnergy();
    }
    
    protected virtual void ManageEnergy() { }

    /// <summary>
    /// Reproduce method creates a new organism object.
    /// </summary>
    /// <returns></returns>
    protected virtual GameObject Reproduce()
    {
        GameObject child = Instantiate(gameObject, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        Organism childScript = child.GetComponent<Organism>();
        childScript.age = 0;
        childScript.generation = generation + 1;
        return child;
    }
    
    /// <summary>
    /// GainEnergy method increases the energy of the organism.
    /// </summary>
    /// <param name="amount"></param>
    public virtual void GainEnergy(float amount)
    {
        energy += amount;
    }

    /// <summary>
    /// LoseEnergy method decreases the energy of the organism.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnCollisionEnter(Collision other)
    {
        if (Array.Exists(MyPredators, element => other.gameObject.CompareTag(element)))
        {
            BeEaten(other);
        }
    }
    
    /// <summary>
    /// BeEaten method is called when the organism is eaten by another organism.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void BeEaten(Collision other)
    {
        other.gameObject.GetComponent<Organism>().GainEnergy(energy);
        OnDeath();
        Destroy(gameObject);
    }
    
    protected virtual void OnDeath() {}
}

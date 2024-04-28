using System;
using UnityEngine;

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

    protected virtual GameObject Reproduce()
    {
        GameObject child = Instantiate(gameObject, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        Organism childScript = child.GetComponent<Organism>();
        childScript.age = 0;
        childScript.generation = generation + 1;
        return child;
    }
    
    public virtual void GainEnergy(float amount)
    {
        energy += amount;
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (Array.Exists(MyPredators, element => other.gameObject.CompareTag(element)))
        {
            BeEaten(other);
        }
    }
    
    protected virtual void BeEaten(Collision other)
    {
        other.gameObject.GetComponent<Organism>().GainEnergy(energy);
        OnDeath();
        Destroy(gameObject);
    }
    
    protected virtual void OnDeath() {}
}

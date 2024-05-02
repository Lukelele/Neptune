using UnityEngine;


/// <summary>
/// The Organism class is a MonoBehaviour that represents an organism in the game.
/// </summary>
public class Plant : Organism
{
    [Header("Plant Settings")]
    [SerializeField] protected float energyGrow =  1;
    [SerializeField] protected float energyGrowInterval = 1;
    [SerializeField] protected float maxEnergy = 0;
    private float _energyGrowTimer = 0;
    
    [SerializeField] protected float reproductionEnergyThreshold = 20;
    [SerializeField] protected float reproductionEnergyCost = 20;
    public float _reproductionEnergy = 0;
    
    /// <summary>
    /// Manages the energy of the plant. This includes growing energy at regular intervals and reproducing when energy is sufficient.
    /// </summary>
    protected override void ManageEnergy()
    {
        base.ManageEnergy();
        if (!isAlive) return;
        _energyGrowTimer -= Time.deltaTime;
        if (_energyGrowTimer <= 0)
        {
            if (energy < maxEnergy) energy += energyGrow;
            _energyGrowTimer = energyGrowInterval;
            if (energy >= reproductionEnergyThreshold) _reproductionEnergy += energyGrow;
        }
        
        if (_reproductionEnergy >= reproductionEnergyCost)
        {
            Reproduce();
        }
    }
    
    /// <summary>
    /// Reproduce method creates a new plant object.
    /// </summary>
    /// <returns>The new plant object.</returns>
    protected override GameObject Reproduce()
    {
        //if (GameObject.FindGameObjectsWithTag("Food").Length >= 50) return null;
        _reproductionEnergy -= reproductionEnergyCost;
        GameObject child = base.Reproduce();
        Plant childScript = child.GetComponent<Plant>();
        childScript.energy = 0;
        child.transform.position = transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        return child;
    }
    
    /// <summary>
    /// SetOnTerrain method sets the position of the transform on the terrain.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="offset"></param>
    protected void SetOnTerrain(Transform transform, float offset=0)
    {
        Vector3 pos = transform.position;
        Terrain activeTerrain = Terrain.activeTerrain;
        pos.y = activeTerrain.SampleHeight(pos) + activeTerrain.transform.position.y + offset;
        transform.position = pos;
    }

}
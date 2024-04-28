using UnityEngine;

public class Plant : Organism
{
    [Header("Plant Settings")]
    [SerializeField] protected float energyGrow =  1;
    [SerializeField] protected float energyGrowInterval = 1;
    private float _energyGrowTimer = 0;
    
    [SerializeField] protected float reproductionEnergyThreshold = 20;
    [SerializeField] protected float reproductionEnergyCost = 20;
    public float _reproductionEnergy = 0;
    
    protected override void ManageEnergy()
    {
        base.ManageEnergy();
        if (!isAlive) return;
        _energyGrowTimer -= Time.deltaTime;
        if (_energyGrowTimer <= 0)
        {
            energy += energyGrow;
            _energyGrowTimer = energyGrowInterval;
            if (energy >= reproductionEnergyCost) _reproductionEnergy += energyGrow;
        }
        
        if (_reproductionEnergy >= reproductionEnergyThreshold)
        {
            Reproduce();
        }
    }
    
    protected override GameObject Reproduce()
    {
        _reproductionEnergy -= reproductionEnergyCost;
        GameObject child = base.Reproduce();
        Plant childScript = child.GetComponent<Plant>();
        childScript.energy = 0;
        child.transform.position = transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        return child;
    }
    
    protected void SetOnTerrain(Transform transform, float offset=0)
    {
        Vector3 pos = transform.position;
        Terrain activeTerrain = Terrain.activeTerrain;
        pos.y = activeTerrain.SampleHeight(pos) + activeTerrain.transform.position.y + offset;
        transform.position = pos;
    }

}
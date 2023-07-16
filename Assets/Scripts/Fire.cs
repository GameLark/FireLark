using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour
{
    [ShowOnly]
    public float temperature;
    [ShowOnly]
    public float thermalEnergy;

    // constants
    private float specificHeatCapacity = 40;  // J/K  - makes keeping fires lit easier at higher values TODO: base on number of logs
    private float specificThermalConductivityToAir = 75f;  // W/K  - how much heat is transferred to surrounding air per second per degree
    private float ambientTemperature = 293.15f;  // K
    public HashSet<Combustible> combustibleChildren {get; private set;} = new HashSet<Combustible>();

    void Start()
    {
        temperature = ambientTemperature;
        thermalEnergy = specificHeatCapacity * temperature;
    }

    public float TotalThermalEnergyOfChildren() {
        float total = 0;
        foreach (var child in combustibleChildren) {
            total = total + child.thermalEnergy;
        }
        return total;
    }

    void Update()
    {
        // if there are no children with a Combustible script, destroy this Fire
        HashSet<Combustible> _combustibleChildren = new HashSet<Combustible>();
        foreach (Transform child in transform) {
            var combustible = child.gameObject.GetComponent<Combustible>();
            if (combustible != null) _combustibleChildren.Add(combustible);
        }
        combustibleChildren = _combustibleChildren;
        if (combustibleChildren.Count == 0) {
            Destroy(gameObject);
        }

        // each tick, lose some energy to the surrounding air
        thermalEnergy += specificThermalConductivityToAir * (ambientTemperature - temperature) * Time.deltaTime;
    }

    void LateUpdate() {
        temperature = thermalEnergy / specificHeatCapacity;
    }
}

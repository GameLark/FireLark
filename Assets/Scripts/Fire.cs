using UnityEngine;
using System.Collections;


public class Fire : MonoBehaviour
{
    [ShowOnly]
    public float temperature = 293.15f;
    [ShowOnly]
    public float thermalEnergy;

    // constants
    private float specificHeatCapacity = 40;  // J/K  - makes keeping fires lit easier at higher values TODO: base on number of logs
    private float maximumThermalEnergy = 1_000_000; // J - cludge to stop thermal runaway - TODO: use cooling

    void Start()
    {
        thermalEnergy = specificHeatCapacity * temperature;
    }

    void Update()
    {
        // if there are no children with a Combustible script, destroy this Fire
        int combustibleChildren = 0;
        foreach (Transform child in transform) {
            if (child.gameObject.GetComponent<Combustible>() != null) combustibleChildren++;
        }
        if (combustibleChildren == 0) {
            Destroy(gameObject);
        }

        // TODO: Add radiative cooling?

    }

    void LateUpdate() {
        thermalEnergy = Mathf.Min(thermalEnergy, maximumThermalEnergy);
        temperature = thermalEnergy / specificHeatCapacity;
    }
}

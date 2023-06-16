using UnityEngine;
using System.Collections;


public class Fire : MonoBehaviour
{
    [ShowOnly]
    public float temperature = 293.15f;
    [ShowOnly]
    public float thermalEnergy;

    // constants
    private float specificHeatCapacity = 800;  // J/K
    private float maximumThermalEnergy = 1_000_000; // J

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
    }

    void LateUpdate() {
        thermalEnergy = Mathf.Min(thermalEnergy, maximumThermalEnergy);
        temperature = thermalEnergy / specificHeatCapacity;
    }
}

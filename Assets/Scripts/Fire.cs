using UnityEngine;
using System.Collections;


public class Fire : MonoBehaviour
{
    [ShowOnly]
    public float temperature;
    [ShowOnly]
    public float thermalEnergy;

    // constants
    private float specificHeatCapacity = 40;  // J/K  - makes keeping fires lit easier at higher values TODO: base on number of logs
    private float ambientTemperature = 293.15f;  // K


    void Start()
    {
        temperature = ambientTemperature;
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

        // each tick, lose some energy to the surrounding air
        thermalEnergy -= specificHeatCapacity * (ambientTemperature - temperature) * Time.deltaTime;

    }

    void LateUpdate() {
        temperature = thermalEnergy / specificHeatCapacity;
    }
}

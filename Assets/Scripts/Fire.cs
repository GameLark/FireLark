using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Fire : MonoBehaviour
{
    [ShowOnly]
    public float temperature;
    [ShowOnly]
    public float thermalEnergy;
    private MonologueManager monologue;

    // constants
    private float specificHeatCapacity = 40;  // J/K  - makes keeping fires lit easier at higher values TODO: base on number of logs
    private float specificThermalConductivityToAir = 75f;  // W/K  - how much heat is transferred to surrounding air per second per degree
    private float ambientTemperature = 293.15f;  // K
    public HashSet<Combustible> combustibleChildren {get; private set;} = new HashSet<Combustible>();

    void Start()
    {
        temperature = ambientTemperature;
        thermalEnergy = specificHeatCapacity * temperature;
        monologue = GameObject.Find("Inner Monologue").GetComponent<MonologueManager>();
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
        if (combustibleChildren.Count > 2)
        {
            monologue.ShowMessage_Smother();
        }
        var litChildren = combustibleChildren.Where(cc => cc.isLit);
        if (litChildren.Count() > 0 && litChildren.Where(lc => lc.combustibleEnergy < Combustible.initialCombustibleEnergy / 4f).Count() == litChildren.Count())
        {
            monologue.ShowMessage_GoingOut();
        }

        // each tick, lose some energy to the surrounding air
        thermalEnergy += specificThermalConductivityToAir * (ambientTemperature - temperature) * Time.deltaTime;
    }

    void LateUpdate() {
        temperature = thermalEnergy / specificHeatCapacity;
    }
}

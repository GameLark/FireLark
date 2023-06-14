using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public bool isLit;
    public new Light light;

    private new ParticleSystem particleSystem;

    public float temperature = 293.15f; // K
    private float thermalEnergy; // J
    private float heatCapacity = 2_300_000; // J/kg K
    private float combustibleVolume;  // m3
    private float conductivity = 0.12f;  // W/m.K
    private float chemicalEnergyDensity = 18_000_000;  // J/kg
    private float density = 500; // kg/m3

    // Fire parameters
    //
    // proportion of combustion to radiation (as opposed to chemical changes) 0 - 1
    //

    float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }


    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        // combustibleVolume = VolumeOfMesh(mesh);
        combustibleVolume = 1;
        Debug.Log($"Volume: {combustibleVolume}");

        // set up energy
        thermalEnergy = temperature / (heatCapacity * combustibleVolume * density);  // TODO: check!
    }

    // Update is called once per frame
    void Update()
    {
        var emission = particleSystem.emission;
        UpdateFire();

        if (isLit) {
            emission.enabled = true;
            light.enabled = true;
        } else {
            emission.enabled = false;
            light.enabled = false;
        }
    }

    void UpdateFire()
    {

        // old notes:
        // oldTemp + fuelReduction * chemicalEnergyDensity + netConductedEnergy + netRadiatedEnergy

        // TODO: add surface burning effects

        // 1. wood has existing thermalEnergy, combustableVolume
        // 2. calculate existing temperature
        temperature = thermalEnergy * heatCapacity;
        // 3. check if wood is burning based on temperature
        // 4. calculate burn rate based on variables

        // update energy
        
        // update combustible volume

        // update light
        // consider soot
        // L_light = k_light * (energyCombusted)
        // f_light ~ T
        
    }

    void OnCollisionEnter(Collision collision) {
        if (!isLit) {
            return;
        }
        var otherFire = collision.collider.GetComponent<Fire>();
        if (otherFire != null) {
            otherFire.isLit = true;
        }
    }
}

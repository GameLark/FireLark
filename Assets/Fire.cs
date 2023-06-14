using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public bool isLit;
    public new Light light;

    private new ParticleSystem particleSystem;

    private HashSet<Fire> touchingCombustibles = new HashSet<Fire>();

    // Fire parameters
    // variables
    public float thermalEnergy;  // J
    public float combustibleEnergy = 100_000_000;  // J 
    public float temperature = 293.15f;  // K

    // constants    
    public float specificHeatCapacity = 1_000;  // J/K
    public float heatingWhenLit = 1;  // W/K
    public float specificThermalConductivity = 1;  // W/K
    public float ignitionTemperature = 873.15f;  // K
    public float extinguishTemperature = 623.15f;  // K

    // // Unused variables

    // // Object constants
    // private float volume; // m3
    // private float surfaceArea;  // m2
    
    // // Material Constants
    // private float spontaneousIgnitionTemperature = 873.15f; // K
    // private float pilotedIgnitionTemperature = 623.15f; // K
    // private float heatCapacity = 2_300_000; // J/kg.K
    // private float combustibleVolume;  // m3
    // private float conductivity = 0.12f;  // W/m.K
    // private float chemicalEnergyDensity = 18_000_000;  // J/kg
    // private float heatOfCombustion = 21_700_000; // J/kg
    // private float heatOfGasification = 4_600_000; // J/kg   // 4.6 - 8.4 MJ/kg for douglas fir - https://ncfs.ucf.edu/burn_db/Thermal_Properties/material_thermal.html
    // private float density = 500; // kg/m3

    // Fire parameters
    //
    // proportion of combustion to radiation (as opposed to chemical changes) 0 - 1
    //

    // float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    // {
    //     float v321 = p3.x * p2.y * p1.z;
    //     float v231 = p2.x * p3.y * p1.z;
    //     float v312 = p3.x * p1.y * p2.z;
    //     float v132 = p1.x * p3.y * p2.z;
    //     float v213 = p2.x * p1.y * p3.z;
    //     float v123 = p1.x * p2.y * p3.z;
    //     return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    // }

    // float VolumeOfMesh(Mesh mesh)
    // {
    //     float volume = 0;
    //     Vector3[] vertices = mesh.vertices;
    //     int[] triangles = mesh.triangles;
    //     for (int i = 0; i < mesh.triangles.Length; i += 3)
    //     {
    //         Vector3 p1 = vertices[triangles[i + 0]];
    //         Vector3 p2 = vertices[triangles[i + 1]];
    //         Vector3 p3 = vertices[triangles[i + 2]];
    //         volume += SignedVolumeOfTriangle(p1, p2, p3);
    //     }
    //     return Mathf.Abs(volume);
    // }


    // Start is called before the first frame update
    void Start()
    {

        particleSystem = GetComponent<ParticleSystem>();

        // // calculate volume and surface area
        // Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        // volume = VolumeOfMesh(mesh);
        // Debug.Log($"Volume: {volume}");
        // combustibleVolume = volume; 
        // // TODO: calculate surface area from mesh
        if (isLit) {
            temperature = ignitionTemperature;
        }


        // set up initial thermal energy
        thermalEnergy = temperature * specificHeatCapacity;
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

        // 1. heat self due to burning
        var burnEnergy = 0f;
        if (isLit) {
            burnEnergy = Mathf.Min(heatingWhenLit * temperature * Time.deltaTime, combustibleEnergy);
        }
        // 2. burn some internal fuel - reduce energy here - extinguish if no energy left
        combustibleEnergy -= burnEnergy;
 
        // 3. heat/cool self due to touching other objects
        var conductionEnergy = 0f;
        foreach (var touchingCombustible in touchingCombustibles) {
            conductionEnergy += specificThermalConductivity * (touchingCombustible.temperature - temperature) * Time.deltaTime;
        }
        thermalEnergy += conductionEnergy + burnEnergy;

    }

    void LateUpdate()
    {
        UpdateTemperature();
        UpdateIsLit();
        UpdateVisuals();
    }

    void UpdateTemperature(){
        temperature = thermalEnergy / specificHeatCapacity;
    }

    void UpdateIsLit() {
        if (combustibleEnergy <= 0) {
            isLit = false;
        }
        else if (temperature >= ignitionTemperature) {
            isLit = true;
        }
        else if (temperature <= extinguishTemperature) {
            isLit = false;
        }
    }
    
    void UpdateVisuals() {
        // // update light
        // // consider soot
        // // L_light = k_light * (energyCombusted)
        // // f_light ~ T
    }

    void OnCollisionEnter(Collision collision) {
        var touchingCombustible = collision.collider.GetComponent<Fire>();
        if (touchingCombustible != null) {
            touchingCombustibles.Add(touchingCombustible);
        }
    }
    
    void OnCollisionExit(Collision collision) {
        var touchingCombustible = collision.collider.GetComponent<Fire>();
        if (touchingCombustible != null) {
            touchingCombustibles.Remove(touchingCombustible);
        }
    }

}

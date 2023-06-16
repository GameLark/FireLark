using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private bool isLit;
    private bool isCharcoal = false;
    public new Light light;
    private ParticleSystem fireParticles;
    private ParticleSystem smokeParticles;
    private MeshRenderer meshRenderer;

    private HashSet<Fire> touchingCombustibles = new HashSet<Fire>();

    [ShowOnly]
    public float lightIntensity;
    [ShowOnly]
    public Color lightColor;

    // Fire parameters
    // variables
    private float thermalEnergy;  // J
    [ShowOnly]
    public float combustibleEnergy = 750_000;  // J 
    public float temperature = 293.15f;  // K
    private float airTemperature = 293.15f; // K - TODO: have fire heat air

    // constants    
    private float specificHeatCapacity = 1_000;  // J/K
    private float heatingWhenLit = 9;  // W/K
    private float specificThermalConductivityToAir = 5;  // W/K
    private float specificThermalConductivityToWood = 50;  // W/K
    private float ignitionTemperature = 773.15f;  // K
    private float extinguishTemperature = 523.15f;  // K
    private float maximumTemperature = 1273.15f;
    private float maximumThermalEnergy;
    private float emissivity = 5e-5f;

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

        fireParticles = transform.Find("Fire").GetComponent<ParticleSystem>();
        smokeParticles = transform.Find("Smoke").GetComponent<ParticleSystem>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.EnableKeyword("_EMISSION");

        // // calculate volume and surface area
        // Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        // volume = VolumeOfMesh(mesh);
        // Debug.Log($"Volume: {volume}");
        // combustibleVolume = volume; 
        // // TODO: calculate surface area from mesh
        maximumThermalEnergy = maximumTemperature * specificHeatCapacity;
        Init(temperature);
    }

    public void Init(float temperature)
    {
        // set up initial thermal energy
        this.temperature = temperature;
        thermalEnergy = temperature * specificHeatCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFire();

    }

    void UpdateFire()
    {

        // 1. heat self due to burning
        var burnEnergy = 0f;
        var coolingEnergy = 0f;
        if (isLit)
        {
            burnEnergy = Mathf.Min(heatingWhenLit * temperature * Time.deltaTime, combustibleEnergy);
            // 2. burn some internal fuel - reduce energy here - extinguish if no energy left
            combustibleEnergy -= burnEnergy;}
        else {
            coolingEnergy = specificThermalConductivityToAir * (airTemperature - temperature);
        }

        // 3. heat/cool self due to touching other objects
        var conductionEnergy = 0f;
        foreach (var touchingCombustible in touchingCombustibles)
        {
            conductionEnergy +=specificThermalConductivityToWood * (touchingCombustible.temperature - temperature) * Time.deltaTime;
        }
        thermalEnergy += conductionEnergy + burnEnergy + coolingEnergy;
        thermalEnergy = Mathf.Min(thermalEnergy, maximumThermalEnergy); // TODO: revisit as it's just a hacky way to cap temperature
    }

    void LateUpdate()
    {
        UpdateTemperature();
        UpdateIsLit();
        UpdateVisuals();
    }

    void UpdateTemperature()
    {
        temperature = thermalEnergy / specificHeatCapacity;
    }

    void UpdateIsLit()
    {
        if (combustibleEnergy <= 0)
        {
            isLit = false;

            if (!isCharcoal)
            {
                isCharcoal = true;
                OnCombustibleEnergyGone();
            }
        }
        else if (temperature >= ignitionTemperature)
        {
            isLit = true;
        }
        else if (temperature <= extinguishTemperature)
        {
            isLit = false;
        }
    }

    void OnIgnition()
    {
        // enable point light, change particles?
    }

    void OnCombustibleEnergyGone()
    {
        // scale down a little, lower emission intensity, set to black
        var scaleScript = GetComponent<ScaleTransform>();
        var scale = transform.localScale;
        var duration = 2.5f;
        scaleScript.StartScale(transform.localScale, new Vector3(scale[0] * 0.5f, scale[1] * 0.9f, scale[2] * 0.5f), duration);
        meshRenderer.material.color = Color.black;
        meshRenderer.material.SetColor("_EmissionColor", Color.black);
    }

    void OnCooled()
    {
        // set colour to grey, scale down to very small and then remove game object

    }

    void UpdateVisuals()
    {
        // // update light
        lightIntensity = temperature * emissivity;
        lightColor = GetRGBFromTemperature((temperature - extinguishTemperature) * 3);
        light.color = lightColor;
        light.intensity = isLit ? lightIntensity : 0;
        var minLightIntensity = extinguishTemperature * emissivity;

        if (!isCharcoal)
        {
            meshRenderer.material.SetColor("_EmissionColor", light.color * Mathf.Max(lightIntensity - minLightIntensity, 0) * 10f);
        }
        else
        {
            meshRenderer.material.SetColor("_EmissionColor", light.color * 0.1f);
        }

        var fireEmission = fireParticles.emission;
        var smokeEmission = smokeParticles.emission;
        if (isLit)
        {
            fireEmission.enabled = true;
        }
        else
        {
            fireEmission.enabled = false;
        }
        smokeEmission.enabled = temperature > extinguishTemperature;
    }

    void OnCollisionEnter(Collision collision)
    {
        var touchingCombustible = collision.collider.GetComponent<Fire>();
        if (touchingCombustible != null)
        {
            touchingCombustibles.Add(touchingCombustible);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        var touchingCombustible = collision.collider.GetComponent<Fire>();
        if (touchingCombustible != null)
        {
            touchingCombustibles.Remove(touchingCombustible);
        }
    }

    public static Color GetRGBFromTemperature(double tmpKelvin)
    {
        double tmpCalc;
        float r, g, b;

        // Temperature must fall between 1000 and 40000 degrees
        if (tmpKelvin < 1000) tmpKelvin = 1000;
        if (tmpKelvin > 40000) tmpKelvin = 40000;

        // All calculations require tmpKelvin \ 100, so only do the conversion once
        tmpKelvin /= 100;

        // Calculate each color in turn

        // First: red
        if (tmpKelvin <= 66)
        {
            r = 255;
        }
        else
        {
            // Note: the R-squared value for this approximation is .988
            tmpCalc = tmpKelvin - 60;
            tmpCalc = 329.698727446 * Mathf.Pow((float)tmpCalc, (float)-0.1332047592);
            r = (int)tmpCalc;
            if (r < 0) r = 0;
            if (r > 255) r = 255;
        }

        // Second: green
        if (tmpKelvin <= 66)
        {
            // Note: the R-squared value for this approximation is .996
            tmpCalc = tmpKelvin;
            tmpCalc = 99.4708025861 * Mathf.Log((float)tmpCalc) - 161.1195681661;
            g = (int)tmpCalc;
            if (g < 0) g = 0;
            if (g > 255) g = 255;
        }
        else
        {
            // Note: the R-squared value for this approximation is .987
            tmpCalc = tmpKelvin - 60;
            tmpCalc = 288.1221695283 * Mathf.Pow((float)tmpCalc, (float)-0.0755148492);
            g = (int)tmpCalc;
            if (g < 0) g = 0;
            if (g > 255) g = 255;
        }

        // Third: blue
        if (tmpKelvin >= 66)
        {
            b = 255;
        }
        else if (tmpKelvin <= 19)
        {
            b = 0;
        }
        else
        {
            // Note: the R-squared value for this approximation is .998
            tmpCalc = tmpKelvin - 10;
            tmpCalc = 138.5177312231 * Mathf.Log((float)tmpCalc) - 305.0447927307;

            b = (int)tmpCalc;
            if (b < 0) b = 0;
            if (b > 255) b = 255;
        }

        return new Color(r / 255f, g / 255f, b / 255f, 1);
    }
}

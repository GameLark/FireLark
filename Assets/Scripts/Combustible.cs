using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Combustible : MonoBehaviour
{
    
    public bool isLit {get; private set;}
    private bool isCharcoal = false;
    private bool isIgnited = false;
    public new Light light;
    private ParticleSystem fireParticles;
    private ParticleSystem smokeParticles;
    private MeshRenderer meshRenderer;

    private HashSet<Combustible> touchingCombustibles = new HashSet<Combustible>();
    private bool removedFromFire = false;  // flag to track fire splitting

    [ShowOnly]
    public float lightIntensity;
    [ShowOnly]
    public Color lightColor;

    // Fire parameters
      // constants    
    private static float ambientTemperature = 293.15f; // K
    public static readonly float specificHeatCapacity = 1_000;  // J/K
    private float heatingWhenLit = 20;  // W/K
    private float proportionOfRadiativeHeating = 0.75f;  // ratio of self air heating to self heating
    private float specificThermalConductivityToAir = 1f;  // W/K  - pseudo physical
    private float specificThermalConductivityToWood = 50;  // W/K - pseudo physical
    private float ignitionTemperature = 773.15f;  // K
    private float extinguishTemperature = 523.15f;  // K
    private float emissivity = 7e-4f;
    private float textureEmissivity = 1.4f;
    private float lightRangeFactor = 20;

    // variables
    public float thermalEnergy {get; private set;}  // J
    [ShowOnly]
    public float combustibleEnergy = 3_000_000;  // J 
    public float temperature = ambientTemperature;  // K
    private float airTemperature; // K
    private float charcoalHitPoints = 100_000;  // HP!

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
        GameObject.Find("Player").GetComponent<GameOver>().RegisterNewLog(this);
        

        // // calculate volume and surface area
        // Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        // volume = VolumeOfMesh(mesh);
        // Debug.Log($"Volume: {volume}");
        // combustibleVolume = volume; 
        // // TODO: calculate surface area from mesh
        
        airTemperature = GetAirTemp();
        Debug.Log($"Starting Log with temp {temperature} K");
        Init(temperature);
    }

    float GetAirTemp(){
        var parent = transform.parent;
        if (parent != null) {
            var parentFire = parent.GetComponent<Fire>();
            if (parentFire != null) {
                return parentFire.temperature;    
            }
        }
        return ambientTemperature;
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
        var internalEnergy = 0f;
        if (isLit)
        {
            internalEnergy = Mathf.Min(heatingWhenLit * temperature * Time.deltaTime, combustibleEnergy);
            // 2. burn some internal fuel - reduce energy here - extinguish if no energy left
            combustibleEnergy -= internalEnergy;
        }
        var externalEnergy = specificThermalConductivityToAir * (airTemperature - temperature) * Time.deltaTime;

        // 3. heat/cool self due to touching other objects
        var conductionEnergy = 0f;
        foreach (var touchingCombustible in touchingCombustibles)
        {
            conductionEnergy += specificThermalConductivityToWood * (touchingCombustible.temperature - temperature) * Time.deltaTime;
        }
        thermalEnergy += conductionEnergy + (1 - proportionOfRadiativeHeating) * internalEnergy + externalEnergy;
        UpdateParentFire(internalEnergy);
        
    }
    
    void UpdateParentFire(float burnEnergy) {
        var energyRadiated = proportionOfRadiativeHeating * burnEnergy;
        var parent = transform.parent;
        if (parent != null) {
            var parentFire = parent.GetComponent<Fire>();
            parentFire.thermalEnergy += energyRadiated;
        }
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
        airTemperature = GetAirTemp();
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
            if (!isIgnited) {
                OnIgnition();
                isIgnited = true;
            }
        }
        else if (temperature <= extinguishTemperature)
        {
            isLit = false;
            if (isIgnited) {
                OnExtinguish();
                isIgnited = false;
            }
        }
    }

    void OnIgnition()
    {
        // enable point light, change particles?
        // flare up

        // add all touching combustibles to the parent Fire
        var ownParent = gameObject.transform.parent.gameObject;
        foreach (var touchingCombustible in touchingCombustibles) {
            touchingCombustible.gameObject.transform.SetParent(ownParent.transform);
        }
    }

    void OnExtinguish()
    {
        // pass
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
        if (isLit) {
            light.range = lightRangeFactor * Mathf.Log(transform.parent.GetComponent<Fire>().combustibleChildren.Where(c => c.isLit).Count() * Mathf.Exp(1));
        }
        var minLightIntensity = extinguishTemperature * emissivity;

        if (!isCharcoal)
        {
            meshRenderer.material.SetColor("_EmissionColor", light.color * Mathf.Max(lightIntensity - minLightIntensity, 0) * textureEmissivity);
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
        var otherObject = collision.collider.gameObject;
        var touchingCombustible = otherObject.GetComponent<Combustible>();
        // 1. check if the touching combustible is part of an existing fire
        // if it is:
        // add it's parent fire's children
        if (touchingCombustible != null)
        {
            Debug.Log($"{gameObject.name} OnCollisionEnter with combustible");

            var ownParent = transform.parent;
            var otherParent = otherObject.transform.parent;
            if (ownParent != null && ownParent.CompareTag("fire") && otherParent is null) {
                otherObject.transform.SetParent(ownParent, true);
            }
            else if (
                ownParent != null && otherParent != null && 
                ownParent != otherParent &&
                ownParent.CompareTag("fire") && otherParent.CompareTag("fire")
            ) {
                foreach (Transform otherFiresChild in otherParent) {
                    if (otherFiresChild.tag == "log") {
                        otherFiresChild.SetParent(ownParent, true);
                    }
                    else {
                        Debug.Log($"Child of other parent is tagged {otherFiresChild.tag}.");
                    }
                }
                var ownFire = ownParent.GetComponent<Fire>();
                var otherFire = otherParent.GetComponent<Fire>();
                ownFire.temperature = (ownFire.temperature + otherFire.temperature)/2;
            }
            touchingCombustibles.Add(touchingCombustible);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        var touchingCombustible = collision.collider.GetComponent<Combustible>();
        if (touchingCombustible != null)
        {
            Debug.Log($"{gameObject.name} OnCollisionExit with combustible");
            touchingCombustibles.Remove(touchingCombustible);

            // check if this log now has no touchingCombustibles
            if (touchingCombustibles.Count == 0) {

                Debug.Log($"{gameObject.name} have no touching combustibles...");
                if (isLit) {
                    Debug.Log($"{gameObject.name} is lit, so new parent required...");
                    // if so, and the log is lit: create a new fire with this log as a child
                    var newParent = Instantiate(Resources.Load<GameObject>("Fire"));
                    transform.SetParent(newParent.transform);
                }
                else {
                    Debug.Log($"{gameObject.name} is not lit, so setting parent to null...");
                    // if so, but the log is not lit: remove the log from the existing parent only
                    transform.SetParent(null);
                }
            }
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

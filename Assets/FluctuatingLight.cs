using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluctuatingLight : MonoBehaviour
{
    public float minIntensity;
    public float maxIntensity;
    public float minWait;
    public float maxWait;

    private new Light light;

    private void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(FluctuateLightIntensity());
    }

    private IEnumerator FluctuateLightIntensity()
    {
        while (true)
        {
            float randomIntensity = Random.Range(minIntensity, maxIntensity);
            light.intensity = randomIntensity;
            float randomWaitTime = Random.Range(minWait, maxWait);
            yield return new WaitForSeconds(randomWaitTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public bool isLit;
    public new Light light;

    private new ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var emission = particleSystem.emission;
        if (isLit) {
            emission.enabled = true;
            light.enabled = true;
        } else {
            emission.enabled = false;
            light.enabled = false;
        }
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

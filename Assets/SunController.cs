using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{

    private float times;

    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.times = Time.deltaTime * 1;
        UpdateSunLocation(); 
    }

    void UpdateSunLocation() {
        this.gameObject.transform.RotateAround(Vector3.zero, Vector3.back, times * 20);
    }

}

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
        this.gameObject.transform.RotateAround(Vector3.up, Vector3.back, times);
    }

    void ResetSunLocation() {
        
        this.gameObject.transform.position = new Vector3(0, 0, 0);
    
    }
}

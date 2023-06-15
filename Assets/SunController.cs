using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{

    public float speed = 1;

    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSunLocation(); 
    }

    void UpdateSunLocation() {
        gameObject.transform.RotateAround(Vector3.zero, Vector3.back, speed * Time.deltaTime);
    }

    public void SetTime(float time)
    {

        Debug.Log("Setting time to " + time);
        //reset back to 0
        gameObject.transform.localPosition = new Vector3(0, 0, 0);

        //set time
        gameObject.transform.RotateAround(Vector3.zero, Vector3.back, time * Time.deltaTime);
    }
}

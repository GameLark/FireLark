using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{

    public float speed = 1;
    private new Light light;

    
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSunLocation();
    }

    void UpdateSunLocation() {
        // gameObject.transform.RotateAround(Vector3.zero, Vector3.back, speed * Time.deltaTime);
        transform.Rotate(speed * Time.deltaTime, 0, 0);
        if (transform.localEulerAngles.x > -105 && transform.localEulerAngles.x < -75) {
            light.enabled = false;
        } else {
            light.enabled = true;
        }
    }

    public void SetTime(float time)
    {

        Debug.Log("Setting time to " + time);
        //reset back to 0
        // gameObject.transform.localPosition = new Vector3(0, 0, 0);

        //set time
        transform.rotation = Quaternion.Euler(15*time - 90, 0, 0);
        // transform.RotateAround(Vector3.zero, Vector3.back, time * Time.deltaTime);
    }
}

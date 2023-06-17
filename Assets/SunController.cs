using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{

    private float speed = 2.5f;
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
        if (transform.localEulerAngles.x > 255 && transform.localEulerAngles.x < 285) {
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

    public bool IsNightTime() {
        return transform.localEulerAngles.x > 180 && transform.localEulerAngles.x < 360;
    }
}

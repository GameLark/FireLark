using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractControl : MonoBehaviour
{
    public Camera camera;
    Ray ray;
    RaycastHit hitData;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = camera.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction*10);
        if (Input.GetKeyDown("e")) {
            Debug.Log("E pressed...");
            if (Physics.Raycast(ray, out hitData)) {
                Debug.Log(hitData);
            }
            
        }
        // else {
        //     hitData = null;
        // }

    }
}

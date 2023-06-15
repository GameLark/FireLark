using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractControl : MonoBehaviour
{
    public Camera camera;
    public double treeInteractionDistance = 2.0;
    Ray ray;
    RaycastHit hitData;

    // Update is called once per frame
    void Update()
    {
        ray = camera.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction*10);
        if (Physics.Raycast(ray, out hitData) && hitData.distance < treeInteractionDistance) {
            GameObject target = hitData.transform.gameObject;

            // e interaction
            if (Input.GetKeyDown("e")) {
                // Get the script that implements IInteractable, and call Interact()
                var tempMonoArray = hitData.transform.gameObject.GetComponents<MonoBehaviour>();
 
                foreach (var monoBehaviour in tempMonoArray)
                {
        
                    if (monoBehaviour is IInteractable tempInteractable)
                    {
                        tempInteractable.Interact(hitData);
                        break;
                    }
                }  
            }
        } 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractControl : MonoBehaviour
{
    public new Camera camera;
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
            if (Input.GetKeyDown("e") || Input.GetMouseButtonDown(0)) {
                // Get the script that implements IInteractable, and call Interact()
                var interactScripts = hitData.transform.gameObject.GetComponents<IInteractable>();
 
                foreach (var script in interactScripts)
                {
                    script.Interact(hitData);
                }  
            }
        } 
    }
}
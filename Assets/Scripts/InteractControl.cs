using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class InteractControl : MonoBehaviour
{
    public new Camera camera;
    public double treeInteractionDistance = 2.0;
    Ray ray;
    RaycastHit hitData;

    // may not be the best place to put this... 
    public bool isHoldingAxe = false;

    GameObject lastTarget = null;
    // Update is called once per frame
    void Update()
    {
        ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * 10);
        if (Physics.Raycast(ray, out hitData) && hitData.distance < treeInteractionDistance)
        {
            GameObject target = hitData.transform.gameObject;

            // we've hit something
            // if there is a pickupable script on target, enable outline
            // if this is not the same target as last frame, disable outline on last target
            if (target != lastTarget)
            {
                var pickupable = target.GetComponent<Pickupable>();

                if (pickupable != null)
                {
                    pickupable.EnableOutline();
                }

                if (lastTarget != null)
                {
                    pickupable = lastTarget.GetComponent<Pickupable>();

                    if (pickupable != null)
                    {
                        pickupable.DisableOutline();
                    }
                }

                lastTarget = target;
            }



            // e interaction
            if (Input.GetKeyDown("e") || Input.GetMouseButtonDown(0))
            {
                // Get the script that implements IInteractable, and call Interact()
                var interactScripts = hitData.transform.gameObject.GetComponents<IInteractable>();

                foreach (var script in interactScripts)
                {
                    Debug.Log("Interacting with " + script + " on " + hitData.transform.gameObject);
                    script.Interact(hitData);
                }
            }
        }
        else
        {
            // we've hit nothing
            // disable outline on last target
            if (lastTarget != null)
            {
                var pickupable = lastTarget.GetComponent<Pickupable>();

                if (pickupable != null)
                {
                    pickupable.DisableOutline();
                }
            }
        }
    }
}

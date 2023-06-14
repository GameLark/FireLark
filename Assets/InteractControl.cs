using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractControl : MonoBehaviour
{
    public Camera camera;
    public double treeInteractionDistance = 2.0;
    Ray ray;
    RaycastHit hitData;
    private GameObject currentlyOutlined;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ray = camera.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction*10);
        if (Physics.Raycast(ray, out hitData) && hitData.distance < treeInteractionDistance) {
            GameObject target = hitData.transform.gameObject;

            // outlining TODO: buggy
            if (target != currentlyOutlined) {
                if (currentlyOutlined != null) {
                    ToggleOutline(currentlyOutlined);
                }
                ToggleOutline(target);
                currentlyOutlined = target;
            }

            // e interaction
            if (Input.GetKeyDown("e")) {
                Debug.Log("E pressed...");
                if(target.tag == "tree") {
                    Debug.Log($"Distance to tree: {hitData.distance}");

                    Choppable treeChoppingScript = target.GetComponent<Choppable>();
                    if (treeChoppingScript != null) {
                        treeChoppingScript.Chop();
                    }
                  
                }
                else if(target.tag == "fire")
                {
                    Debug.Log("Interact with fire");
                    PlayerData playerData = gameObject.GetComponent<PlayerData>();

                    if(playerData.GetWood() > 0) {
                        FeedFire fireScript = target.GetComponent<FeedFire>();
                        playerData.GetWood(-1);
                        fireScript.AddWood(1);
                    }
                }
                else if (target.tag == "log")
                {
                    Debug.Log("Interact with log");
                    var fixedJoint = camera.GetComponent<FixedJoint>();
                    var targetRigidBody = target.GetComponent<Rigidbody>();
                    if (fixedJoint != null) {
                        Destroy(fixedJoint);
                    } else {
                        // connect
                        fixedJoint = camera.gameObject.AddComponent<FixedJoint>();
                        fixedJoint.connectedBody = targetRigidBody;
                        fixedJoint.connectedAnchor = target.transform.position;
                        fixedJoint.enableCollision = false;
                        fixedJoint.anchor = transform.position;
                    }
                }
            }
        } else if (currentlyOutlined != null) {
            // we are now not looking at this object
            ToggleOutline(currentlyOutlined);
        }
    }

    private void ToggleOutline(GameObject g) {
        return; // TODO: currently a bit buggy
        if (g.tag != "log") { // TODO: use some better discriminator
            return;
        }

        var outline = g.GetComponent<cakeslice.Outline>();
        if (outline != null) {
            Destroy(outline);
        } else {
            g.AddComponent<cakeslice.Outline>();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireInteraction : MonoBehaviour, IInteractable
{
    public void Interact(RaycastHit hitData) {
        // get main camera 
        Camera camera = Camera.main;
        // get the fixed joint on the camera
        FixedJoint fixedJoint = camera.GetComponent<FixedJoint>();

        // get the target rigidbody
        Rigidbody targetRigidBody = hitData.transform.gameObject.GetComponent<Rigidbody>();

        // if the camera has a fixed joint, destroy it
        if (fixedJoint != null) {
            Destroy(fixedJoint);
        } else {
            // connect
            fixedJoint = camera.gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = targetRigidBody;
            fixedJoint.connectedAnchor = targetRigidBody.transform.position;
            fixedJoint.enableCollision = false;
            fixedJoint.anchor = transform.position;
        }
    }
}

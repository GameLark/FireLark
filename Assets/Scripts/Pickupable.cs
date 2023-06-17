using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// --------------------------------------------------------------------------------
// FYI: This script needs to be on the same GameObject that a Rigidbody, and MeshCollider are on
// --------------------------------------------------------------------------------
public class Pickupable : MonoBehaviour, IInteractable
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
            targetRigidBody.GetComponent<MeshCollider>().isTrigger = false;
        } else {
            // connect
            fixedJoint = camera.gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = targetRigidBody;
            fixedJoint.connectedAnchor = targetRigidBody.transform.position;
            fixedJoint.enableCollision = false;
            fixedJoint.anchor = transform.position;

            // disable collisions on the target
            var meshCollider = targetRigidBody.GetComponent<MeshCollider>();

            meshCollider.isTrigger = true;

            // call the OnPickup function on the target if the script is not null
            IPickupable script = targetRigidBody.GetComponent<IPickupable>();
            if (script != null)
            {
                script.OnPickup();
            }

        }
    }
}


public interface IPickupable {
    void OnPickup();
}
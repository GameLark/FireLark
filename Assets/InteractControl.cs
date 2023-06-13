using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractControl : MonoBehaviour
{
    public Camera camera;
    public double treeInteractionDistance = 2.0;
    Ray ray;
    RaycastHit hitData;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ray = camera.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction*10);
        if (Input.GetKeyDown("e")) {
            Debug.Log("E pressed...");
            if (Physics.Raycast(ray, out hitData)) {
                GameObject target = hitData.transform.gameObject;

                if(target.tag == "tree") {
                    Debug.Log($"Distance to tree: {hitData.distance}");
                    if(hitData.distance < treeInteractionDistance) {
                        // call tree script 'interact' method
                        Interaction interactScript = target.GetComponent<Interaction>();
                        Debug.Log("Interact script");
                        Debug.Log(interactScript);

                        int woodGained = interactScript.Interact("e");

                        Debug.Log($"Got {woodGained} wood!");

                        PlayerData playerData = gameObject.GetComponent<PlayerData>();
                        Debug.Log($"Player Data: {playerData}");
                        playerData.GetWood(woodGained);

                        Debug.Log($"Total wood gained so far: {playerData.GetWood()}");
                    }
                }
                else if(target.tag == "fire")
                {
                    if(hitData.distance < treeInteractionDistance) {
                        Debug.Log("Interact with fire");
                        PlayerData playerData = gameObject.GetComponent<PlayerData>();

                        if(playerData.GetWood() > 0) {
                            FeedFire fireScript = target.GetComponent<FeedFire>();
                            playerData.GetWood(-1);
                            fireScript.AddWood(1);
                        }
                    }
                }
            }
        }
    }
}

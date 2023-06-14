using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choppable : MonoBehaviour
{

    public GameObject woodPrefab;
    
    public float chopTime = 2f;
    public float chopDamage = 1f;
    private float currentHealth = 5f; // TODO: make this based off of the current tree type


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void onTreeDestroy(){
        Debug.Log("Tree Destroyed");
        // spawn 3 wood on top of each other
        Vector3 spawnPosition = transform.position; // spawn position is at the center of the current position
        for (int i = 0; i < 3; i++)
        {
            Instantiate(woodPrefab, spawnPosition, Quaternion.identity);
            spawnPosition += Vector3.up; // move the spawn position up by one unit for each iteration
        }
    }

    
    public void Chop()
    {
        // make the object shake 
        // get the shake script
        ShakeTransform shakeScript = GetComponent<ShakeTransform>();
        if (shakeScript != null)
        {
            shakeScript.Begin();
        }

        // apply damage to the tree
        currentHealth -= chopDamage;
        if (currentHealth <= 0)
        {
            onTreeDestroy();

            // TODO: play a sound, drop wood, etc.
            // destroy the tree
            Destroy(gameObject);

        }
    }

   
}

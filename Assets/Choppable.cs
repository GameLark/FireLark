using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choppable : MonoBehaviour
{

    public GameObject woodPrefab;
    public GameObject saplingPrefab;

    
    public float chopTime = 2f;
    public float chopDamage = 1f;
    private float currentHealth = 5f; // TODO: make this based off of the current tree type


    // Start is called before the first frame update
    void Start()
    {

    }

    private void onTreeDestroy(){
        // get size of the tree
        int treeWoodCount = GetComponent<TreeGrowth>().GetTreeValue();

        Vector3 spawnPosition = transform.position; // spawn position is at the center of the current position
        for (int i = 0; i < treeWoodCount; i++)
        {
            Instantiate(woodPrefab, spawnPosition, Quaternion.identity);
            spawnPosition += Vector3.up; // move the spawn position up by one unit for each iteration
        }

        // pick a random point within the tree's mesh to spawn the sapling
        Mesh treeMesh = GetComponent<MeshFilter>().mesh;

        // spawn 0-2 saplings
        int saplingCount = Random.Range(0, 3);
        for (int i = 0; i < saplingCount; i++)
        {
            Vector3 randomPoint = treeMesh.vertices[Random.Range(0, treeMesh.vertices.Length)];
            Instantiate(saplingPrefab, transform.position + randomPoint, Quaternion.identity);
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

        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            onTreeDestroy();

            // TODO: play a sound, drop wood, etc.
            // destroy the tree
            Destroy(gameObject);
        }
    }

   
}
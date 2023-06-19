using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choppable : MonoBehaviour, IInteractable
{

    public GameObject woodPrefab;
    public GameObject saplingPrefab;

    private TreeSound treeSound;


    public float chopTime = 2f;
    public float handChopDamage = 1f;

    public float axeChopDamage = 5f;
    private float currentHealth = 20f;

    public void Interact(RaycastHit hitData)
    {
        Chop();
    }

    void Start()
    {
        treeSound = GetComponent<TreeSound>();
    }

    private void onTreeDestroy()
    {
        // get size of the tree
        int treeWoodCount = GetComponent<TreeGrowth>().GetTreeValue();

        // play tree fell sound
        treeSound.PlayFellSound();

        Vector3 spawnPosition = transform.position; // spawn position is at the center of the current position
        for (int i = 0; i < treeWoodCount; i++)
        {
            Instantiate(woodPrefab, spawnPosition, Quaternion.identity);
            spawnPosition += Vector3.up; // move the spawn position up by one unit for each iteration
        }

        // pick a random point within the tree's mesh to spawn the sapling
        Mesh treeMesh = GetComponent<MeshFilter>().mesh;

        // spawn saplings
        int saplingCount = Random.Range(1, 3);
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

        bool isHoldingAxe = GameObject.Find("Player").GetComponent<InteractControl>().isHoldingAxe;

        var chopDamage = isHoldingAxe ? axeChopDamage : this.handChopDamage;

        currentHealth -= chopDamage;

        if (currentHealth <= 0)
        {
            onTreeDestroy();

            Destroy(gameObject);
        }
        else
        {
            // play chop sounds
            treeSound.PlayHitSound();
        }
    }
}

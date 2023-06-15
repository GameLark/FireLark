using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowth : MonoBehaviour
{
    public bool shouldGrow = false;
    public int maxTreeSize;
    public Mesh[] treeMeshes; 
    private Mesh currentTreeMesh;
    private int currentMeshIndex = 0;
    public float timeBetweenTreeStates = 1f;

    void Awake()
    {
        // start with a random mesh
        currentMeshIndex = Random.Range(0, treeMeshes.Length); 

        Debug.Log("Starting with mesh " + currentMeshIndex);
        SetTreeMesh(treeMeshes[currentMeshIndex]);

        maxTreeSize = Random.Range(currentMeshIndex, maxTreeSize);

        StartCoroutine(ChangeMeshOverTime());
    }

    public void SetTreeSize(int size) {
        Mesh mesh = treeMeshes[size - 1];
        Debug.Log("Setting tree size to " + size);
        SetTreeMesh(mesh);
    }

    void SetTreeMesh(Mesh mesh) {
        currentTreeMesh = mesh;
        gameObject.GetComponent<MeshFilter>().mesh = currentTreeMesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = currentTreeMesh;
    }

    public int GetTreeValue() {
        return currentMeshIndex + 1;
    }

    IEnumerator ChangeMeshOverTime()
    {
        while (true && shouldGrow)
        {
            yield return new WaitForSeconds(timeBetweenTreeStates);

            if(currentMeshIndex < treeMeshes.Length) {
                SetTreeMesh(treeMeshes[currentMeshIndex++]);
            } else {
                // stop the coroutine
                StopCoroutine(ChangeMeshOverTime());
            }
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowth : MonoBehaviour
{
    public Mesh[] treeMeshes; 
    private Mesh currentTreeMesh;
    private int currentMeshIndex = 0;
    public float timeBetweenTreeStates = 1f;

    void Start()
    {
        // start with a random mesh
        currentMeshIndex = Random.Range(0, treeMeshes.Length); 
        SetTreeMesh(treeMeshes[currentMeshIndex]);
        StartCoroutine(ChangeMeshOverTime());
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
        while (true)
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

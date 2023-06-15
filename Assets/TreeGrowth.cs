using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TreeGrowth : MonoBehaviour
{
    public bool shouldGrow = false;
    private int maxTreeSize;
    public Mesh[] treeMeshes; 
    private Mesh currentTreeMesh;
    private int currentMeshIndex = 0;
    public float timeBetweenTreeStates = 1f;

    // called after Instantiate()
    void Awake()
    {
        // start with a random mesh
        currentMeshIndex = Random.Range(0, treeMeshes.Length); 

        SetTreeMesh(treeMeshes[currentMeshIndex]);
    }

    // Start is called before the first frame update
    void Start()
    {
        maxTreeSize = Random.Range(currentMeshIndex, treeMeshes.Length);
    }

    public void SetTreeSize(int size) {
        Mesh mesh = treeMeshes[size - 1];
        currentMeshIndex = size - 1;
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
    

    public void StartTreeGrowing() {
        shouldGrow = true;
        StartCoroutine(ChangeMeshOverTime());
    }

    public IEnumerator ChangeMeshOverTime()
    {
        while (true && shouldGrow)
        {
            yield return new WaitForSeconds(timeBetweenTreeStates);

            if(currentMeshIndex < this.maxTreeSize) {
                SetTreeMesh(treeMeshes[currentMeshIndex++]);
            } else {
                // stop the coroutine
                StopCoroutine(ChangeMeshOverTime());
            }
            
        }
    }
}

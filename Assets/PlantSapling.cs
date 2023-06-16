using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSapling : MonoBehaviour, IPickupable
{

    public GameObject treePrefab;
    private bool isPickedUp = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPickup()
    {
        isPickedUp = true;
    }


    // when the plant collides with the floor, it should be planted
    void OnCollisionEnter(Collision collision)
    {
        // instanciate a new tree prefab where the sapling is, destroy the sapling
        if (collision.gameObject.tag == "ground" && this.isPickedUp)
        {
            // instanciate a new tree prefab where the sapling is, destroy the sapling
            GameObject newTree = Instantiate(treePrefab, transform.position, Quaternion.identity);
            var treeGrowth = newTree.GetComponent<TreeGrowth>();
            treeGrowth.SetTreeSize(1);
            treeGrowth.StartTreeGrowing();
            // scale the tree up to its normal size over time
            var currentScale = gameObject.transform.localScale;
            newTree.GetComponent<ScaleTransform>().StartScaleUp(currentScale, new Vector3(1f, 1f, 1f), 0.5f);
            Destroy(gameObject);
        }
    }
}

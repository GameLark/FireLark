using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSapling : MonoBehaviour, IPickupable
{

    public GameObject treePrefab;
    public bool isPickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPickup() {
        isPickedUp = true;
    }

    // when the plant collides with the floor, it should be planted
     void OnCollisionEnter(Collision collision)
    {
       // instanciate a new tree prefab where the sapling is, destroy the sapling
         if (collision.gameObject.tag == "ground" && isPickedUp) {
              // instanciate a new tree prefab where the sapling is, destroy the sapling
              GameObject newTree = Instantiate(treePrefab, transform.position, Quaternion.identity);

              newTree.GetComponent<TreeGrowth>().shouldGrow = true;
              Destroy(gameObject);
         }
    }
}

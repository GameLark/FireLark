using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelBehaviour : MonoBehaviour
{
    //private Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        //collider = gameObject.GetComponent<Collider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

      //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        //check if obj has tag "acorn"
        if(other.gameObject.CompareTag("acorn"))
        {
            Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            Debug.Log("Squirrel collided with acorn");

        }

    }
}

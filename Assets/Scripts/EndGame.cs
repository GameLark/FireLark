using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        var otherObject = collision.collider.gameObject;
        var touchingCombustible = otherObject.GetComponent<Combustible>();
        // 1. check if the touching combustible is part of an existing fire
        // if it is:
        // add it's parent fire's childred
        if (touchingCombustible != null)
        {
        
        }
    }
}

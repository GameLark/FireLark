using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public int value = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Interact(string interactChar) 
    {
        Debug.Log($"Tree interact: {interactChar}");
        
        Destroy(gameObject);

        return value;
    } 
}

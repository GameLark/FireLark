using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    int collectedWood = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Getter and setter for collectedWood
    public int GetWood(int woodGot=0) {
        this.collectedWood += woodGot;
        return this.collectedWood;
    }

}

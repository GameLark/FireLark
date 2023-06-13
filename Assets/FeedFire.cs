using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedFire : MonoBehaviour
{

    int woodCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scale = 1 + woodCount * 0.1f;
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void AddWood(int woodCount) 
    {
        this.woodCount += woodCount;
    }

    
}

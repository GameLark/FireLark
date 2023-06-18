using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Credits : MonoBehaviour
{
    public static GameObject credits;
    // Start is called before the first frame update
    void Start()
    {
        credits = GameObject.FindGameObjectWithTag("credits");
        credits.SetActive(false);
                
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if(credits.activeSelf)
            {
                credits.SetActive(false);
            }
            else
            {
                credits.SetActive(true);
            }
        }
    }

}

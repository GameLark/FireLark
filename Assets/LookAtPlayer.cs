using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        TurnTowardsPlayer();
    }


    // Update is called once per frame
    void Update()
    {
        //rotate towards player
        TurnTowardsPlayer();  
    }

    private void TurnTowardsPlayer()
    {
        transform.LookAt(player.transform, Vector3.up);
        transform.Rotate(0, -90, 0);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject player;
    public float delay = 1f;
    private float timeToWait;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        timeToWait = delay;
        TurnTowardsPlayer();
    }


    // Update is called once per frame
    void Update()
    {

        //rotate towards player

        TurnTowardsPlayer();
    }

    void TurnTowardsPlayer()
    {
        //look at player but with a delay
        if (timeToWait <= 0)
        {
            //slowly turn towards player


            Vector3 dir = player.transform.position - transform.position;
            dir = new Vector3(dir.x, dir.y, dir.z);
            dir.y = 0; // keep the direction strictly horizontal
            Quaternion rot = Quaternion.LookRotation(dir) * Quaternion.Euler(0, -90, 0);
            // slerp to the desired rotation over time
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, Time.deltaTime*100);
            //if is looking at player, reset timetowait
            RaycastHit _hit = new RaycastHit();
            if (Physics.Raycast(transform.position, transform.forward, out _hit))
            {
                if (_hit.transform.gameObject.tag == "Player")
                {
                    timeToWait = delay;
                }
            }

        }
        else
        {
            timeToWait -= Time.deltaTime;
        }
    }
}

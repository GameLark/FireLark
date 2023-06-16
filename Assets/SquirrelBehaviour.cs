using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelBehaviour : MonoBehaviour
{
    public int acornsRequired = 10; //todo show acorn sign above squirrel
    public bool followingPlayer = false;
    public float speed = 0.05f;
    public float jumpIncrement = 0.03f;

    public float jumpHeight = 0.5f;
    public float minDistance = 2;
    private bool isJumpingUp = true;
    private GameObject player;
    private bool standing;
    private float waitingBeforeMoving;
    public float secondsToWaitBeforeMoving = 1f;
    private AudioSource audioSource;
    public AudioClip eatSound;
    public AudioClip petSound;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ResetWaiting();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (followingPlayer)
        {
            //move towards player but always keep 10m away
            Vector3 moveTowards = Vector3.MoveTowards(transform.position, player.transform.position, speed);
            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(moveTowards, player.transform.position) > minDistance)
            {
                if (standing)
                {
                    //reduce waiting time
                    waitingBeforeMoving -= Time.deltaTime;
                    if (waitingBeforeMoving <=0) standing = false;
                }
                else {
                    transform.position = moveTowards;
                    JumpUpAndDown();

                }
            }
            else
            {
                JumpDown();
                ResetWaiting();
            }
        }

    }

    void ResetWaiting()
    {
        standing = true;
        waitingBeforeMoving = secondsToWaitBeforeMoving;
    }
    void JumpUpAndDown()
    {
        //jump whilst walking
        if (isJumpingUp)
        {
            Debug.Log("Jumping up");
            float newY = transform.position.y + jumpIncrement;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            Debug.Log("newY: " + newY);
            if (newY >= jumpHeight) isJumpingUp = false;
        }
        else
        {
            Debug.Log("Jumping down");
            float newY = transform.position.y - jumpIncrement;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            if (newY <= 0) isJumpingUp = true;
        }

    }
    void JumpDown()
    {
        float newY = transform.position.y - jumpIncrement;
        if (newY <= 0)
        {
            isJumpingUp = true;
            return;
        }
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        //check if obj has tag "acorn"
        if (other.gameObject.CompareTag("acorn"))
        {
            //remove acorn
            Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            //eating sound
            audioSource.PlayOneShot(eatSound);
            //add to acorn count
            acornsRequired--;
            if(acornsRequired <= 0) followingPlayer = true;
        }

    }
}

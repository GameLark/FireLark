using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    private HashSet<Fire> endGameFires = new HashSet<Fire>();
    private GameObject boat;
    private float boatSpeed = 10;
    private bool isEndGame = false;

    // Start is called before the first frame update
    void Start()
    {
        boat = transform.Find("Boat").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEndGame) {
            foreach (var fire in endGameFires) {
                // TODO: use thermal energy of fire - currently that's not reliable
                if (fire.transform.GetChild(0).GetComponent<Combustible>().isLit && !isEndGame) {
                    StartCoroutine(TriggerEndGame(fire));    
                }
            }
        }
    }

    private IEnumerator TriggerEndGame(Fire fire)
    {
        Debug.Log("Game end");
        isEndGame = true;
        boat.SetActive(true);
        var fireLocation = fire.transform.GetChild(0).position; // TODO: what is the location of the fire? For now this is good enough
        var player = GameObject.Find("Player");
        while ((fireLocation - boat.transform.position).magnitude > 5) {
            boat.transform.position += (fireLocation - boat.transform.position).normalized * boatSpeed * Time.deltaTime;
            yield return null;
        }
        player.GetComponent<GameOver>().EndGame();
        var boatAudioSource = boat.GetComponent<AudioSource>();
        var holdOnSound = Resources.Load<AudioClip>("hold_on");
        boatAudioSource.PlayOneShot(holdOnSound, 10);
        yield return new WaitForSeconds(holdOnSound.length);
        
        Credits.credits.SetActive(true);
        foreach (var credit in Credits.credits.GetComponentsInChildren<TMP_Text>()) {
            credit.color = Color.clear;
        }
        yield return StartCoroutine(FadeImageToFullAlpha(3f, Credits.credits.GetComponent<Image>()));
        boatAudioSource.enabled = false;
        var audioSource = player.GetComponent<AudioSource>();
        audioSource.PlayOneShot(Resources.Load<AudioClip>("timpani_1"));
        yield return new WaitForSeconds(2.5f);
        audioSource.PlayOneShot(Resources.Load<AudioClip>("timpani_2"));
        yield return new WaitForSeconds(2.5f);
        audioSource.PlayOneShot(Resources.Load<AudioClip>("timpani_3"));
        foreach (var credit in Credits.credits.GetComponentsInChildren<TMP_Text>()) {
            credit.color = Color.black;
        }
    }

    public static IEnumerator FadeImageToFullAlpha(float t, Image i)
    {

        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        var otherObject = collision.collider.gameObject;
        var touchingCombustible = otherObject.GetComponent<Combustible>();
        if (touchingCombustible != null)
        {
            var parent = touchingCombustible.transform.parent;
            if (parent.CompareTag("fire")) {
                var fire = parent.GetComponent<Fire>();
                if (fire != null) {
                    endGameFires.Add(fire);
                }
            }
        }
    }
}

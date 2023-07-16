using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    private HashSet<Combustible> endGameLogs = new HashSet<Combustible>();
    private GameObject boat;
    private float boatSpeed = 10;
    private bool isEndGame = false;

    // > 10 logs at 1000 degrees
    private readonly float endGameFireEnergy = Combustible.specificHeatCapacity * 1000 * 10;

    // Start is called before the first frame update
    void Start()
    {
        boat = transform.Find("Boat").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEndGame) {
            foreach (var log in endGameLogs) {
                // TODO: use thermal energy of fire - currently that's not reliable
                var fire = log.transform.parent.GetComponent<Fire>();
                if (fire != null && fire.TotalThermalEnergyOfChildren() > endGameFireEnergy && !isEndGame) {
                    StartCoroutine(TriggerEndGame(log));    
                }
            }
        }
    }

    private IEnumerator TriggerEndGame(Combustible log)
    {
        Debug.Log("Game end");
        isEndGame = true;
        boat.SetActive(true);
        var fireLocation = log.transform.position; // TODO: what is the location of the fire? For now this is good enough
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
            endGameLogs.Add(touchingCombustible);
        }
    }
}

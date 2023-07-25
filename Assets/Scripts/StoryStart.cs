using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class StoryStart : MonoBehaviour
{
    public AudioSource lightning;
    public TMP_Text[] words;
    public GameObject startMusic;
    private AudioSource startMusicSource;
    private GameObject player;
    private GameObject playerCamera;
    private GameObject playerSound;
    private GameObject lightningLight;
    private MonologueManager monologue;

    // Start is called before the first frame update
    void Start()
    {
        startMusicSource = startMusic.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerSound = GameObject.Find("First Person Audio");
        lightningLight = GameObject.FindGameObjectWithTag("lightning");
        monologue = GameObject.Find("Inner Monologue").GetComponent<MonologueManager>();

        //disable player
        player.GetComponent<FirstPersonMovement>().LockMovement();
        playerCamera.GetComponent<FirstPersonLook>().LockCamera();
        playerSound.SetActive(false);


        //set it to be transparent
        foreach (TMP_Text w in words)
        {
            w.color = new Color(w.color.r, w.color.g, w.color.b, 0);
        }
        StartCoroutine(RevealText());

    }
    IEnumerator RevealText()
    {
        if (!GameManager.quickStart) {
            startMusicSource.Play();
            for (int word = 0; word < words.Length; word++)
            {
                StartCoroutine(FadeTextToFullAlpha(1f, words[word]));
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitUntil(() => startMusicSource.isPlaying == false);
            HideText();
            // pause...
            yield return new WaitForSeconds(2f);
        }
        GameObject.FindGameObjectWithTag("Sun").GetComponent<SunController>().SetTime(0);

        //disable black screen
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        lightning.Play();

        yield return FlashLightningLights();
        yield return new WaitForSeconds(1.5f);
        var fire = GameObject.Find("First Log").GetComponent<Combustible>();
        fire.Init(1000);
        yield return new WaitForSeconds(2f);
        monologue.SetEnabled(true);

        //re-enable player
        player.GetComponent<FirstPersonMovement>().UnlockMovement();
        playerCamera.GetComponent<FirstPersonLook>().UnlockCamera();
        playerSound.SetActive(true);

        // enable gameover tracker
        player.GetComponent<GameOver>().StartGame();

        yield return new WaitForSeconds(1f);
        monologue.SetVisibleTime(0.5f);
        yield return new WaitForSeconds(0.5f);
        monologue.SetEnabled(false);


        //fade out
        // Light light = lightningLight.GetComponent<Light>();
        // float i = light.spotAngle;

        // while (lightningLight.transform.localPosition.y > -50)
        // {
        //     lightningLight.transform.localPosition = new Vector3(lightningLight.transform.localPosition.x, lightningLight.transform.localPosition.y - 0.1f, lightningLight.transform.localPosition.z);
        //     yield return new WaitForSeconds(.25f);
        // }
    }


    IEnumerator FlashLightningLights()
    {

        Debug.Log("lightning is playing");

        Color red = new Color(1, 0, 0, 1);
        Color yellow = new Color(1, 1, 0, 1);
        Color white = new Color(1, 1, 1, 1);
        Light light = lightningLight.GetComponent<Light>();

        light.color = red;
        light.intensity = 100;
        yield return new WaitForSeconds(.02f);

        light.color = yellow;
        light.intensity = 100;

        yield return new WaitForSeconds(.02f);

        light.intensity = 0;

        yield return new WaitForSeconds(.35f);

        light.color = yellow;
        light.intensity = 100;

        yield return new WaitForSeconds(.02f);

        light.color = white;
        light.intensity = 100;

        yield return new WaitForSeconds(.02f);

        light.intensity = 0;

        yield return new WaitForSeconds(.2f);

        light.color = red;
        light.intensity = 100;

        yield return new WaitForSeconds(.02f);

        light.color = yellow;
        light.intensity = 100;

        yield return new WaitForSeconds(.02f);

        light.intensity = 100;
        light.color = white;

        yield return new WaitForSeconds(.02f);

        light.intensity = 0;
    }
    void HideText()
    {
        foreach (TMP_Text w in words)
        {
            w.color = new Color(w.color.r, w.color.g, w.color.b, 0);
        }
    }

    IEnumerator FadeTextToFullAlpha(float t, TMP_Text i)
    {

        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

}

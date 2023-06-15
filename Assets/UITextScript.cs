using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextScript : MonoBehaviour
{
    public TMP_Text[] words;
    public GameObject startMusic;
    private AudioSource startMusicSource;
    // Start is called before the first frame update
    void Start()
    {
        startMusicSource = startMusic.GetComponent<AudioSource>();
        //set it to be transparent
        foreach (TMP_Text w in words)
        {
            w.color = new Color(w.color.r, w.color.g, w.color.b, 0);
        }
        StartCoroutine(RevealText());
    }
    IEnumerator RevealText() {
        for (int word = 0; word < words.Length; word++) 
        {
            StartCoroutine(FadeTextToFullAlpha(1f, words[word]));
            yield return new WaitForSeconds(2f);
        }
        yield return new WaitUntil(() => startMusicSource.isPlaying == false);
        HideText();
        // pause...
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    void HideText( ) {
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

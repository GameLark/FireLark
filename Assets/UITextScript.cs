using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextScript : MonoBehaviour
{
    public TMP_Text[] words;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RevealText());
    }

    IEnumerator RevealText() {
        for (int word = 0; word < words.Length; word++) 
        {
            StartCoroutine(FadeTextToFullAlpha(1f, words[word]));
            yield return new WaitForSeconds(2f);
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

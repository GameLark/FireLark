using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUp : MonoBehaviour
{

    public void StartScaleUp(Vector3 startScale, Vector3 targetScale, float duration) {
        StartCoroutine(ScaleUpOverTime(startScale, targetScale, duration));
    }

    IEnumerator ScaleUpOverTime(Vector3 startScale, Vector3 targetScale, float duration) {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration) {
            gameObject.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime * Time.timeScale; // multiply by Time.timeScale
            yield return null;
        }

        gameObject.transform.localScale = targetScale;
    }
}

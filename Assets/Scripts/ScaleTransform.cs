using System;
using System.Collections;
using UnityEngine;

public class ScaleTransform : MonoBehaviour
{

    public void StartScale(Vector3 startScale, Vector3 targetScale, float duration, Action callback = null)
    {
        Debug.Log("Starting scale");
        StartCoroutine(ScaleOverTime(startScale, targetScale, duration, callback));
    }

    private IEnumerator ScaleOverTime(Vector3 startScale, Vector3 targetScale, float duration, Action callback)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime <= duration)
        {
            var deltaTime = Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            Debug.Log("Scale: " + gameObject.transform.localScale + "deltaTime: " + deltaTime + "elapsedTime: " + elapsedTime + "duration: " + duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (callback != null)
            callback();
    }
}

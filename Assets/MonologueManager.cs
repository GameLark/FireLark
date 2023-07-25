using UnityEngine;
using TMPro;
using System.Collections;

public class MonologueManager : MonoBehaviour
{
    private float minAlpha = 0.2f; // Minimum alpha value when text is invisible
    private float maxAlpha = 1.0f; // Maximum alpha value when text is fully visible
    private float minVisibleTime = 0.1f; // Minimum time the text should be visible
    private float maxVisibleTime = 1.0f; // Maximum time the text should be visible
    private float shakeAmount = 5; // Adjust this value to control the intensity of the shaking.
    private float shakeSpeed = 100; // Adjust this value to control the speed of the shaking.

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private bool isShivering = false;
    private TMP_Text textMeshPro;
    private bool isVisible = true;
    private float flickerTimer = 0f;
    private float currentVisibleTime = 0f;
    private bool customIsEnabled = false;

    // game state
    private bool hasPlacedSecondLog = false;
    private bool hasSmotheredOnce = false;

    private void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        SetNextVisibleTime();
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.localPosition;
    }

    public void SetEnabled(bool enable)
    {
        customIsEnabled = enable;
        SetVisibleTime(0);
    }

    public void SetVisibleTime(float duration)
    {
        isVisible = true;
        flickerTimer = 0;
        currentVisibleTime = duration;
    }

    public void SetText(string text)
    {
        textMeshPro.text = text;
    }

    public IEnumerator DisableSoon(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopShivering();
        SetEnabled(false);
    }

    public void ShowMessage_Smother()
    {
        if (!hasPlacedSecondLog)
        {
            hasPlacedSecondLog = true;
            SetText("be CAREFUL! too many log make fire cold");
            SetEnabled(true);
            StartCoroutine(DisableSoon(4));
        }
    }

    public void ShowMessage_GoingOut()
    {
        if (!hasSmotheredOnce)
        {
            hasSmotheredOnce = true;
            SetText("fire nearly burned out!!");
            SetEnabled(true);
            StartShivering();
            currentVisibleTime = 2;
            StartCoroutine(DisableSoon(4));
        }
    }

    private void Update()
    {
        if (customIsEnabled)
        {
            flickerTimer += Time.deltaTime;

            if (flickerTimer >= currentVisibleTime)
            {
                ToggleVisibility();
                SetNextVisibleTime();
            }

            // Calculate the new alpha value based on time and visibility duration
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, flickerTimer / currentVisibleTime);

            // Apply the new alpha value to the TMP_Text material
            Color color = textMeshPro.color;
            color.a = isVisible ? alpha : 0f;
            textMeshPro.color = color;
        }
        else
        {
            Color color = textMeshPro.color;
            color.a = 0;
            textMeshPro.color = color;
        }
    }

    private void ToggleVisibility()
    {
        isVisible = !isVisible;
        flickerTimer = 0f;
    }

    private void SetNextVisibleTime()
    {
        currentVisibleTime = Random.Range(minVisibleTime, maxVisibleTime);
    }

    public void StartShivering()
    {
        if (!isShivering)
        {
            isShivering = true;
            StartCoroutine(Shiver());
        }
    }

    private IEnumerator Shiver()
    {
        while (isShivering)
        {
            float xShake = shakeAmount * Mathf.Sin(Time.time * shakeSpeed);
            float yShake = shakeAmount * Mathf.Sin(Time.time * shakeSpeed); // You can adjust the multiplier for a different shaking pattern.
            Vector3 shakeOffset = new Vector3(xShake, yShake, 0f);

            rectTransform.localPosition = originalPosition + shakeOffset;

            yield return null;
        }
    }

    public void StopShivering()
    {
        isShivering = false;
        rectTransform.localPosition = originalPosition;
    }

}
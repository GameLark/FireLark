using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxePickup : MonoBehaviour, IPickupable
{
    // Start is called before the first frame update
    public Color emissionColor = Color.white;

    public MeshRenderer meshRenderer;

    public void OnPickup()
    {
        // wait for 1 second
        // animate scale to 0
        // destroy game object
        // set isHoldingAxe to true
        var interactControl = GameObject.Find("Player").GetComponent<InteractControl>();
        interactControl.isHoldingAxe = true;

        StartCoroutine(AnimatePickup());
    }

    IEnumerator AnimatePickup()
    {
        yield return new WaitForSeconds(0.5f);
        // destroy the FixedJoint on the camera
        var camera = Camera.main;
        var fixedJoint = camera.GetComponent<FixedJoint>();
        Destroy(fixedJoint);

        // remove the rigidbody from the axe
        Destroy(GetComponent<Rigidbody>());

        // get the axe's current position
        Vector3 axePosition = transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;

        // move the axe towards the camera
        float axeSpeed = 3f;
        while (Vector3.Distance(axePosition, cameraPosition) > 0.1f)
        {
            axePosition = Vector3.MoveTowards(axePosition, cameraPosition, axeSpeed * Time.deltaTime);
            transform.position = axePosition;

            //update positions
            axePosition = transform.position;
            cameraPosition = Camera.main.transform.position;

            yield return null;
        }


        // scale the axe to 0
        Vector3 axeScale = gameObject.transform.localScale;
        float axeScaleSpeed = 20f;
        while (axeScale.x > 0.1f)
        {
            axeScale = Vector3.MoveTowards(axeScale, Vector3.zero, axeScaleSpeed * Time.deltaTime);
            transform.localScale = axeScale;

            yield return null;
        }

        Destroy(gameObject);
    }


    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        // enable emission for all materials
        var materials = meshRenderer.materials;

        foreach (var material in materials)
        {
            material.EnableKeyword("_EMISSION");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // adjust emission to make it pulse
        var materials = meshRenderer.materials;

        foreach (var material in materials)
        {
            material.SetColor("_EmissionColor", emissionColor * Mathf.PingPong(Time.time * 2f, 0.5f));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAbove : MonoBehaviour
{
    public Transform leader;
    public Vector3 offset;

    void LateUpdate()
    {
        transform.position = leader.position + offset;
    }
}

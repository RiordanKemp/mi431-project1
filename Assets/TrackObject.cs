using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObject : MonoBehaviour
{
    [Header("Dynamic")]
    public Transform target;


    void FixedUpdate()
    {
        transform.position = target.position;

    }
}

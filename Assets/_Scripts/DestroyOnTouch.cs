using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTouch : MonoBehaviour
{
    public GameObject parentGO;
    public GameObject deathParticles;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player == null) return;

        GameObject GO = Instantiate<GameObject>(deathParticles);
        GO.transform.position = parentGO.transform.position;
        Destroy(parentGO);
    }
}

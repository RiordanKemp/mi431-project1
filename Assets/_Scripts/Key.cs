using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [Header("Inscribed")]
    public Door[] doors;
    public GameObject particlesGO;

    void OnTriggerEnter2D(Collider2D other){
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        foreach (Door dScr in doors){
            dScr.gameObject.SetActive(false);
        }

        GameObject newParticles = Instantiate<GameObject>(particlesGO);
        newParticles.transform.position = transform.position;

        this.gameObject.SetActive(false);
    }

}

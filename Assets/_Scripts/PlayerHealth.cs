using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Inscribed")]
    public int health = 1;
    [Header("Dynamic")]
    [SerializeField] PlayerController pcScript;
    
    // Start is called before the first frame update

    void Awake(){
        pcScript = FindObjectOfType(typeof(PlayerController)) as PlayerController;
    }
    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.CompareTag("Spike")){
            print("spike");
            //health -= 1;
            pcScript.Respawn();
        }
    }
}

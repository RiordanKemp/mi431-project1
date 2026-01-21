using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    [Header("Inscribed")]
    [Tooltip("THe highest-weighted active checkpoint will be used")]
    public int checkpointWeight = 1;
    public TMP_Text checkpointText;
    public GameObject captureParticles;
    public Vector3 respawnOffset = Vector3.zero;
    [Header("Dynamic")]
    [SerializeField] bool isActive = false;
    [SerializeField] SpriteRenderer[] sprites;
    // Start is called before the first frame update
    void Awake(){
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D other){
        print("trigger enter");
        PlayerController pcScript = other.gameObject.GetComponent<PlayerController>();
        if (pcScript == null) return;

        print("player got the checkpoint");
        if(!isActive && checkpointWeight > pcScript.currentCheckpoint.checkpointWeight){
            ActivateCheckpoint(pcScript);
        }
    }

    void ActivateCheckpoint(PlayerController playerContrScript){
        isActive = true;
        playerContrScript.currentCheckpoint = this;

        //this is redundant for now, but it allows the means to change multiple sprites differently
        for(int i = 0; i < sprites.Length; i++){
            sprites[0].color = Color.green;
        }

        GameObject particlesGO = Instantiate<GameObject>(captureParticles);
        particlesGO.transform.position = transform.position;

        checkpointText.text = $"Checkpoints: {checkpointWeight}/7";
    }        
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlock : MonoBehaviour
{
    [Header("Inscribed")]
    public Color defaultColor;
    public Color disabledColor;
    public bool matchFanToggle = true;
    [Header("Dynamic")]
    SpriteRenderer spriteRenderer;
    PlayerController pcScript;
    Collider2D collider2D;

    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        
        
    }

    public void UpdateBlockStatus(bool fansOn){
        if (fansOn && matchFanToggle || !fansOn && !matchFanToggle){
            EnableBlock();
        }
        else{
            DisableBlock();
        }

    }

    void EnableBlock(){
        spriteRenderer.color = defaultColor;
        collider2D.isTrigger = false;
    }

    void DisableBlock(){
        spriteRenderer.color = disabledColor;
        collider2D.isTrigger = true;
    }



    
}

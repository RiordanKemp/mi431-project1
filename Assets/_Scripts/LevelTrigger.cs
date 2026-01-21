using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTrigger : MonoBehaviour
{
    [Header("Inscribed")]
    public Canvas nextLevelCanvas; 
    public Image blackImage;

    void Awake(){
        nextLevelCanvas.enabled = false;

    }

    void OnTriggerEnter2D(Collider2D other){
        PlayerController pcScript = other.gameObject.GetComponent<PlayerController>();
        if (pcScript != null){
            pcScript.NextLevel(nextLevelCanvas);
            print("Level trigger entered");
            FadeToBlack();
            
        }
    }

    void FadeToBlack(){
        Color tempColor = blackImage.color;
        print("temp color a:" + tempColor.a);
        float alpha = Mathf.Lerp(tempColor.a, 1, 0.05f);
        tempColor.a = alpha;
        blackImage.color = tempColor;
        if (alpha < .99f) {
            Invoke("FadeToBlack", 0.05f);
            return;}
        tempColor.a = 1;
        blackImage.color = tempColor;
    
    }
}

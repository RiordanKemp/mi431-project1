using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextActivator : MonoBehaviour
{
    [Header("Inscribed")]
    public TextController textController;
    public int conversationInt = -1;

    void OnTriggerEnter2D(Collider2D other){
        PlayerController pcScript = other.gameObject.GetComponent<PlayerController>();
        if (pcScript != null){
            textController.RunConversation(conversationInt);
            print("Player trigger entered");
            this.gameObject.SetActive(false);
        }
    }
}

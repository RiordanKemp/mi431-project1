using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextController : MonoBehaviour
{
    [Header("Inscribed")]
    public Canvas characterCanvas;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public Image characterPortrait;
    public DialogueController[] conversations;
    bool runningConversation = false;
    int conversationInt = -1;
    int dialogueInt = -1;
        [Header("Dynamic")]
    [SerializeField] int dialogueLength = -1;
    SettingsManager settingsManager;
    // Start is called before the first frame update
    public void RunConversation(int elementInt){
        conversationInt = elementInt;
        dialogueLength = conversations[conversationInt].dialogues.Length;
        characterCanvas.enabled = true;
        runningConversation = true;

        dialogueInt = -1;
        ProgressDialogue();
    }

    void Awake(){
        EndConversation();
        //Only for testing.  comment out later
        //print(conversations[0].dialogues.Length);
        RunConversation(0);
                settingsManager = Camera.main.GetComponent<SettingsManager>();


    }

    void Update(){
        if (!runningConversation) {
            return;}

        if (Input.GetKeyDown(settingsManager.controls["nextdialogue"])){
            ProgressDialogue();
        }
    }

    void ProgressDialogue(){
        dialogueInt += 1;
        if (dialogueInt >= dialogueLength){
            print("too much");
            EndConversation();
            return;
        }
        print("dialogue int:" + dialogueInt);

        Sprite updatedImage = conversations[conversationInt].dialogues[dialogueInt].newPortrait;
        if (updatedImage != null){
            characterPortrait.sprite = updatedImage;
        }

        string dialogue = conversations[conversationInt].dialogues[dialogueInt].dialogueStr;
        dialogueText.text = dialogue;

        string updatedName = conversations[conversationInt].dialogues[dialogueInt].newNameStr;
        if (!string.IsNullOrEmpty(updatedName)){
            characterNameText.text = updatedName;
        }

    }

    void EndConversation(){
        characterCanvas.enabled = false;
        runningConversation = false;
        print("ended dialogue");
    }
}

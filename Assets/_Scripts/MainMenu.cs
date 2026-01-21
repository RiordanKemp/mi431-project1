using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Inscribed")]
    public Canvas defaultSuite;
    public Canvas settingsSuite;

    public void PlayButton(){
        SceneManager.LoadScene("_Scene_0");
    }

    void Awake(){
        settingsSuite.enabled = false;
    }

    public void OpenSettings(){
        defaultSuite.enabled = false;
        settingsSuite.enabled = true;

    }

    public void ReturnToMenu(){
        settingsSuite.enabled = false;
        defaultSuite.enabled = true;
    }



    void Update(){

            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))){
                if (Input.GetKeyDown(key)){
                    print("Key:" + key.ToString()); 
                    PlayerPrefs.SetString("BINDING", key.ToString());
                    string newStr = PlayerPrefs.GetString("BINDING");
                    print("binding key:" + newStr);
                    KeyCode thisKeyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), newStr) ;
                    print("this keycode:" + thisKeyCode);

                
            }
        }
    }
}

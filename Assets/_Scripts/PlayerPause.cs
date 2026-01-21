using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerPause : MonoBehaviour
{
    [Header("Inscribed")]
    public KeyCode pauseKey = KeyCode.Escape;
    public Canvas canvasObject;
    
    [Header("Dynamic")]
    [SerializeField] bool isPaused = false;
    PlayerController pcScript;

    void Awake(){
        canvasObject.enabled = false;
        pcScript = this.gameObject.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (pcScript.nextLevel){
            if (isPaused){
            UnpauseScene();
            }
            this.enabled = false;
        }

        if (Input.GetKeyDown(pauseKey)){
            isPaused = !isPaused;
            if (isPaused){
                PauseScene();
            }
            else{
                UnpauseScene();
            }
        }
    }

    public void PauseScene(){
        Time.timeScale = 0;
        isPaused = true;
        canvasObject.enabled = true;

    }

    public void UnpauseScene(){
        Time.timeScale = 1;
        canvasObject.enabled = false;
        isPaused = false;


    }

    public void ExitScene(){
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLevel(int level){
        if (level == -1){
            SceneManager.LoadScene("Scene0Dream");
        }
    }
}

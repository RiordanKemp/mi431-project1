using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AwakeBinding : MonoBehaviour
{
    TextMeshProUGUI tmpro;
    string settingName = "";
    void Awake(){
         tmpro = GetComponent<TextMeshProUGUI>();
        settingName = tmpro.name;
        UpdateThisKey();

    }

    public void UpdateThisKey(){
       

        tmpro.text = PlayerPrefs.GetString(settingName);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Window_Dropdown : MonoBehaviour
{
    TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeWindowMode()
    {
        
        if (dropdown.options[dropdown.value].text == "Fullscreen")
        {
            Screen.fullScreen = true;
        }
        else if(dropdown.options[dropdown.value].text == "Windowed")
        {
            Screen.fullScreen = false;
        }
        //PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
    }
}

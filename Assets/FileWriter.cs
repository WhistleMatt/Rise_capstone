using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class FileWriter : MonoBehaviour
{

    private bool _enabled = false;
    private string path = Application.dataPath + "DebugLog.txt";
    

    public void enable()
    {
        _enabled = true;
    }

    public void disable()
    {
        _enabled = false;
    }
    void CreateText()
    {
        //file path
        
        //create file
        if (!File.Exists(path)) 
        {
            File.WriteAllText(path, "Debug Info For Rise Of The Unblessed \n\n");

        }

    
  
    }


    public void writeDebug(string _string)
    {
        if (_enabled)
        File.AppendAllText(path,_string + "\n");
    }

    void fpsMonitor()
    {
        if (_enabled)
        writeDebug("FPS: " + 1.0 / Time.deltaTime);
    }

    void playerPositionMonitor()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        writeDebug("Player Position " + GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    void cameraPosition()
    {
        if (_enabled)
        {
            writeDebug("Camera Position X,Y,Z: " + GameObject.FindGameObjectWithTag("MainCamera").transform.position);
            writeDebug("Camera Rotation X,Y,Z: " + GameObject.FindGameObjectWithTag("MainCamera").transform.rotation);
        }
           
    }
    void Start()
    {
        
        CreateText();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enabled)
        {
            fpsMonitor();
            playerPositionMonitor();
            cameraPosition();
        }
    
        
    }

    private void Awake()
    {
      writeDebug("Debugger: Current Scene is: "+ SceneManager.GetActiveScene().name);
    }
}

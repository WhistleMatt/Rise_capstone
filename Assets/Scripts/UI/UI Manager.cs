//Developed by Matthew Whistle
//a factory class designed to handle the creation,
//Destruction and swapping of all U.I. Elements
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.Cinemachine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject m_playingUI;
    [SerializeField] private GameObject m_pausedUI;

    [SerializeField] private GameObject m_resolutionSelect;


    //panels for Quitting the game
    [SerializeField] private GameObject m_quitGameCanvas;
    [SerializeField] private GameObject m_confirmQuit;

   // [SerializeField] private GameObject m_debugButton;
    [SerializeField] private GameObject m_DebugCanvas;
    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.QuitStatus() == -1 && m_confirmQuit.activeInHierarchy)
        {
            m_confirmQuit.SetActive(false);
        }
    }

    public void PauseMenu()
    {
        Cursor.visible = true;
        GameObject.FindGameObjectWithTag("cinemaCam").GetComponent<CinemachineFreeLook>().enabled = false;
        m_playingUI.SetActive(false);
        m_pausedUI.SetActive(true);
        GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().getMouseSettings();
    }

    public void Unpause()
    {
        GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().SetStatistics();
        Cursor.visible = false;
        GameObject.FindGameObjectWithTag("cinemaCam").GetComponent<CinemachineFreeLook>().enabled = true;
        if (m_confirmQuit.activeInHierarchy)
        {
            GameManager.instance.CancelQuit();
        }
        m_pausedUI.SetActive(false);
        m_playingUI.SetActive(true);
        
    }

    public void QuitGameUI()
    {
        m_quitGameCanvas.SetActive(true);
    }

    public void ConfirmQuitGameUI()
    {
        m_confirmQuit.SetActive(true);
    }

    public void SwapOptions(int option)
    {
        //option 0 is for resolition settings
        if (option == 0)
        {
            if (m_confirmQuit.activeSelf)
            {
                m_confirmQuit.SetActive(false);
            }
            m_quitGameCanvas.SetActive(false);
            m_resolutionSelect.SetActive(true);
            m_DebugCanvas.SetActive(false);
        }
        //option 1 is for quit game options
        else if (option == 1)
        {
            m_resolutionSelect.SetActive(false);
            m_quitGameCanvas.SetActive(true);
            m_DebugCanvas.SetActive(false);
        }
        //option 1 is for quit game options
        else if (option == 2)
        {
            m_DebugCanvas.SetActive(true);
            m_quitGameCanvas.SetActive(false);
            m_resolutionSelect.SetActive(false);
        }
    }
}

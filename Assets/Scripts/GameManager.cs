using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int m_quitMode = -1;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = true;
        Screen.SetResolution(3849, 2160, true);
    }

    private void Update()
    {
        
    }

    public void QueueQuit(int quitHow)
    {
        m_quitMode = quitHow;
        UIManager.instance.ConfirmQuitGameUI();
    }

    public void ConfirmQuit()
    {
        if(m_quitMode == 0)
        {
            QuitToTitle();
        }
        else
        {
            QuitToDesktop();
        }
    }

    public void CancelQuit()
    {
        m_quitMode = -1;
    }

    public int QuitStatus()
    {
        return m_quitMode;
    }

    public void QuitToTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitToDesktop()
    {
        /*
        if(EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
            return;
        }
        */
        Application.Quit();
    }
}

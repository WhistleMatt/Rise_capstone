using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUIManager : MonoBehaviour
{
    private SceneManager sceneManager;
    [SerializeField] GameObject m_panel;
    [SerializeField] GameObject button;

    public void Login()
    {
        m_panel.SetActive(true);
        button.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void StartGame()
    {
       SceneManager.LoadScene("DemoLevel", LoadSceneMode.Single);
    }
}

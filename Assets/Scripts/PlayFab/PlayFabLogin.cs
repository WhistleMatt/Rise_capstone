using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
//using static Cinemachine.DocumentationSortingAttribute;

public class PlayFabLogin : MonoBehaviour
{
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string UserPassword { get; set; }

    private string defaultEmail = "@gmail.com";
    private string defaultPassword = "Password";

    public GameObject LoginPanel;
    private int levelID;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(levelID);
        if(string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "CC9EA";

        }

        if (LoadUserPrefs())
        {
            //OnClickLogin();
        }

        
    }

    public void OnClickLogin()
    {
        UserEmail = UserName + defaultEmail;
        UserPassword = defaultPassword;

        var req = new LoginWithEmailAddressRequest { Email = UserEmail, Password = UserPassword };
        PlayFabClientAPI.LoginWithEmailAddress(req, OnLoginSuccess, OnLoginFailure);
    }

    private void SaveUserPrefs()
    {
        UserEmail = UserName + defaultEmail;
        UserPassword = defaultPassword;
        PlayerPrefs.SetString("EMAIL", UserEmail);
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: User Email Saved; "+ UserEmail);
        PlayerPrefs.SetString("PASSWORD", UserPassword);
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Password Saved; " + UserPassword);
        PlayerPrefs.SetString("USERNAME", UserName);
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: User Saved; " + UserName);
        LoginPanel.SetActive(false);
    }

    private bool LoadUserPrefs()
    {
        if(PlayerPrefs.HasKey("EMAIL"))
        {
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Has Key = True");
            UserEmail = PlayerPrefs.GetString("EMAIL");
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Got Email; "+ UserEmail);
            UserPassword = PlayerPrefs.GetString("PASSWORD");
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Got Password; " + UserPassword);

            //this was set to UserPassord
            UserName = PlayerPrefs.GetString("USERNAME");
            // UserName = PlayerPrefs.GetString("USERNAME");
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Got User; " + UserName);
            return true;
        }
        return false;
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("PlayFAB API Call Success!");
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: API Call Success");
        SaveUserPrefs();

        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
            );
        //GameObject.Find("TitleScreenUI").GetComponent<TitleScreenUIManager>().StartGame();

    }

    private void OnLoginFailure(PlayFabError _error)
    {
        UserEmail = UserName + defaultEmail;
        UserPassword = defaultPassword;

        var reg = new RegisterPlayFabUserRequest { Email = UserEmail, Password = UserPassword, Username = UserName };
        PlayFabClientAPI.RegisterPlayFabUser(reg,OnRegisterSuccess, OnRegisterFailure);
        Debug.Log("PlayFAB API Call Failed!");
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: API Call Failed");
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult _result)
    {
        Debug.Log("Registered successfully!");
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: User Registered Successfully");
        SaveUserPrefs();
        SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
    }

    private void OnRegisterFailure(PlayFabError playFabError)
    {
        Debug.LogError(playFabError.GenerateErrorReport());
    }

    private void OnGetStatistics(GetPlayerStatisticsResult _result)
    {
        foreach (var stat in _result.Statistics)
        {
            switch (stat.StatisticName)
            {
                case "JustStarted":
                    {
                        if (stat.Value == 1)
                        {
                            SceneManager.LoadScene("Level1", LoadSceneMode.Single);
                        }
                        else
                        {
                            SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
                        }//SceneManager.LoadScene(stat.Value, LoadSceneMode.Single);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;


public class Direct_Join_Script : MonoBehaviour
{
    [SerializeField] private string LobbyID;
    [SerializeField] private string LobbyPassword;
    [SerializeField] private Button ConnectButton;
    [SerializeField] private Button CancelButton;

    [SerializeField] private GameObject JoinOptUI;
    [SerializeField] private GameObject WaitingRoomUI;
    [SerializeField] private GameObject PasswordBox;

    [SerializeField] private bool Toggled;
    [SerializeField] private bool Joining;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ConnectButton.onClick.AddListener(async () =>
        {
            if (Joining)
            {
                return;
            }
            Joining = true;

            if (LobbyID.Length > 0)
            {
                string name = "Foo Fighter";
                string Username = PlayerPrefs.GetString("USERNAME", name);

                JoinLobbyByIdOptions options = new JoinLobbyByIdOptions()
                {
                    Player = new Player()
                    {
                        Data = new Dictionary<string, PlayerDataObject>
                            {
                                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, Username)}
                            },
                    }
                };
                if(Toggled && (LobbyPassword.Length > 0))
                {
                    options.Password = LobbyPassword;
                }

                var result = await Multiplayer_lobby_manager.Instance.JoinPrivateLobbyBYID(LobbyID, options);
                if (result)
                {
                    WaitingRoomUI.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
        });

        CancelButton.onClick.AddListener(() =>
        {
            if (Joining)
            {
                return;
            }
            JoinOptUI.SetActive(true);
            gameObject.SetActive(false);
        });

    }

    public void ChangeToggle(bool toggleState)
    {
        Toggled = toggleState;
        if (Toggled) 
        {
            PasswordBox.SetActive(true);
        }
        else
        {
            PasswordBox.SetActive(false);
        }
    }

    public void SetLobbyID(string id)
    {
        LobbyID = id;
    }

    public void SetLobbyPassword(string password)
    {
        LobbyPassword = password;
    }

    private void OnDisable()
    {
        Toggled = false;
        LobbyID = string.Empty;
        PasswordBox.SetActive(false);
        LobbyPassword = string.Empty;
    }
}

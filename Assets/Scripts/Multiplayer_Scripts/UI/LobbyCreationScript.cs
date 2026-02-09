using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreationScript : MonoBehaviour
{
    [SerializeField]  public string LobbyName { get; set; }
    [SerializeField] public int PlayerCount { get; set; }
    [SerializeField]  public bool PrivateLobby { get; set; }
    [SerializeField] public bool RequirePassword { get; set; }
    [SerializeField]  public string Password {  get; set; }
    [SerializeField]  public string ErrorText { get; set; }

    [SerializeField] private GameObject PasswordOption;

    [SerializeField] private TextMeshProUGUI ErrorTextOption;

    [SerializeField] private Button CreateBtn;
    [SerializeField] private Button CancelBtn;
    [SerializeField] private GameObject SelectionScreen;
    [SerializeField] private GameObject m_waitLobbyCanvas;

    private void Start()
    {
        PlayerCount = 1;
        CreateBtn.onClick.AddListener(async () =>
        {
            if (LobbyName == null)
            {
                ErrorTextOption.gameObject.SetActive(true);
                ErrorTextOption.text = "Lobby Name cannot be nothing.";
                return;
            }
            var value = await Multiplayer_lobby_manager.Instance.FindLobbiesOfName(LobbyName);

            if (value != null)
            {
                string name = "Foo Fighter";
                string Username = PlayerPrefs.GetString("USERNAME", name);


                CreateLobbyOptions options = new CreateLobbyOptions()
                {
                    IsPrivate = PrivateLobby,
                    Player = new Player()
                    {
                        Data = new Dictionary<string, PlayerDataObject>
                        {
                            {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, Username)},
                            {"IsHost", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "Host") }
                        }
                    }
                };
                options.IsPrivate = PrivateLobby;

                if (RequirePassword)
                {
                    if ((Password == null) || (Password.Length < 8))
                    {
                        ErrorTextOption.gameObject.SetActive(true);
                        ErrorTextOption.text = "Password Must be at leat 8 characters";
                        return;
                    }
                    options.Password = Password;
                    options.IsLocked = true;
                }

                
                options.Data = new Dictionary<string, DataObject>()
                {
                    {
                        "PlayerName", new DataObject(visibility:DataObject.VisibilityOptions.Public,
                        value: Username,
                        index: DataObject.IndexOptions.S1)
                    },
                };

                var result = await Multiplayer_lobby_manager.Instance.CreateLobbyWithParams(LobbyName, options, PlayerCount);
                m_waitLobbyCanvas.SetActive(true);
                this.gameObject.SetActive(false);
            }
        });

        CancelBtn.onClick.AddListener(() =>
        {
            SelectionScreen.SetActive(true);
            this.gameObject.SetActive(false);
        });
    }

    public void AssignLobbyName(string name)
    {
        LobbyName = name;
    }

    public void ConfirmSettings()
    {

    }

    private void Update()
    {
        Debug.Log(PlayerCount);
        Debug.Log(PrivateLobby);
        Debug.Log(PrivateLobby);
    }

    public void OnPrivacyValueUpdated(bool _value)
    {
        PrivateLobby = _value;
    }

    public void OnLockedValueUpdated(bool _value)
    {
        RequirePassword = _value;
        if (RequirePassword)
        {
            PasswordOption.SetActive(true);
        }
        else
        {
            PasswordOption.SetActive(false);
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class Multiplayer_Lobby_Panel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI LobbynameText;
    [SerializeField] TextMeshProUGUI HostNameText;
    [SerializeField] TextMeshProUGUI PlayerCountText;

    [SerializeField] Button Join_button;

    private Lobby RepresentativeLobby;

    private string LobbyPassword;
    private bool HasPassword = false;

    private bool CanJoin = true;

    private void Start()
    {
        Join_button.onClick.AddListener(() =>
        {
            JoingLobby();
        });
    }

    public void SetText(Lobby _lobby)
    {
        RepresentativeLobby = _lobby;

        _lobby.Data.TryGetValue("PlayerName", out var sampleObj);

        LobbynameText.text = _lobby.Name;
        HostNameText.text = sampleObj.Value;
        PlayerCountText.text = "Players: " + _lobby.Players.Count.ToString() + "/" + _lobby.MaxPlayers.ToString();

        if (RepresentativeLobby.MaxPlayers == RepresentativeLobby.Players.Count)
        {
            CanJoin = false;
        }
    }

    public void setPassword(string _password)
    {
        LobbyPassword = _password;
    }

    public void SetHasPassword(bool _hasPassword)
    {
        HasPassword = _hasPassword;
    }

    public async void JoingLobby()
    {
        Debug.Log("trying to Join!");
        if (!CanJoin) return;

        string name = "Foo Fighter";
        string Username = PlayerPrefs.GetString("USERNAME", name);

        JoinLobbyByIdOptions joinOptions = new JoinLobbyByIdOptions()
        {
            Player = new Player()
            {

                Data = new Dictionary<string, PlayerDataObject>
                {
                    {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, Username)}
                },
            },
        };

        if (HasPassword)
        {
            if (LobbyPassword != null)
            {
                if (LobbyPassword.Length < 8)
                {
                    return;
                }
                joinOptions.Password = LobbyPassword;
            }
            else
            {
                return;
            }
        }

        await Multiplayer_lobby_manager.Instance.JoinLobby(RepresentativeLobby, joinOptions);
        Debug.Log("Joined!");
        GameObject.Find("LobbyListUI").GetComponent<LobbyListIUI>().OpenWaitingUI();
    }
}

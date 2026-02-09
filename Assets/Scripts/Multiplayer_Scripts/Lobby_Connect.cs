using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby_Connect : MonoBehaviour
{
    [SerializeField] private GameObject play_ui;
    [SerializeField] private GameObject lobby_ui;

    [SerializeField] private Button host_btn;
    [SerializeField] private Button join_btn;
    [SerializeField] private Button quit_btn;

    [SerializeField] private GameObject m_scrollPanel;

    [SerializeField] private Multiplayer_Lobby_Panel m_panel;
    [SerializeField] private GameObject m_createLobbyUI;
    [SerializeField] private Multiplayer_Host_start_Script m_start;

    public string LobbyCode { get; set; }

    private void Start()
    {
        this.LobbyCode = "";
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        UnityServices.InitializeAsync();

        


        host_btn.onClick.AddListener(async () =>
        {
            //string lobbyname = "Sample Lobby";
            //int maxPlayers = 2;
            //CreateLobbyOptions options = new CreateLobbyOptions();
            //options.IsPrivate = false;

            //Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyname, maxPlayers, options);

            //Multiplayer_lobby_manager.Instance.CreateLobby("SampleLobby");

            //m_start.gameObject.SetActive(true);
            m_createLobbyUI.SetActive(true);
            this.gameObject.SetActive(false);

            //var testvar = await StartHostWithRelay(2, "udp");
            //NetworkManager.Singleton.StartHost();

            try
            {
                //Lobby joinedlobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id);
                //Debug.Log(joinedlobby.Name);
            }
            catch (LobbyServiceException ex)
            {
                Debug.LogException(ex);
            }

            this.gameObject.SetActive(false);
        }
        );


        join_btn.onClick.AddListener(async () => {
            //NetworkManager.Singleton.StartClient();

            //m_panel.gameObject.SetActive(true);

            m_scrollPanel.gameObject.SetActive(true);

            //var results = await Multiplayer_lobby_manager.Instance.ListLobbies();

            this.gameObject.SetActive(false);


        });

        quit_btn.onClick.AddListener(() => {
            SceneManager.LoadScene("Level1");
        });
    }

    public async Task<string> StartHostWithRelay(int maxConnections, string connectionType)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        var allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, connectionType));
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        Multiplayer_UI_Manager.instance.SetHostID(joinCode);
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    public async Task<bool> StartClientWithRelay(string joinCode, string connectionType)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, connectionType));
        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }

    public async Task<QueryResponse> QueryLobbiesAsync()
    {

        QueryLobbiesOptions options = new QueryLobbiesOptions();
        options.Count = 25;

        // Filter for open lobbies only
        options.Filters = new List<QueryFilter>()
    {
        new QueryFilter(
            field: QueryFilter.FieldOptions.AvailableSlots,
            op: QueryFilter.OpOptions.GT,
            value: "0")
    };

        // Order by newest lobbies first
        options.Order = new List<QueryOrder>()
    {
        new QueryOrder(
            asc: false,
            field: QueryOrder.FieldOptions.Created)
    };

        return await LobbyService.Instance.QueryLobbiesAsync(options);

    }
}

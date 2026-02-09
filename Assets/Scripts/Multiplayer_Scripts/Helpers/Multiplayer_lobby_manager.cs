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
using UnityEngine;
using UnityEngine.SceneManagement;

public class Multiplayer_lobby_manager : MonoBehaviour
{
    public static Multiplayer_lobby_manager Instance;
    private Lobby HostLobby;
    [SerializeField] private Lobby JoinedLobby;
    [SerializeField] private string lobby_joinname;
    [SerializeField] private string lobbyid;
    [SerializeField] string JoinCode;

    private Player ourPlayer;

    private bool canConnect = false;

    private float timer = 2f;
    private float updateTimer = 2f;
    private float hostUpdateTimer = 0f;
    private float hostTimer = 0f;


    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

    }

    public string GetLobbyCode()
    {
        return lobbyid;
    }

    // Update is called once per frame
    async void Update()
    {
        await LobbyHeartbeat();
        await LobbyStartCheck();
    }

    public bool IsHost()
    {
        return HostLobby != null;
    }

    public async Task kickPlayer(Player player)
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(HostLobby.Id, player.Id);

            HostLobby = await LobbyService.Instance.GetLobbyAsync(HostLobby.Id);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public List<Player> GetConnectedPlayers()
    {
        
        if (JoinedLobby != null)
        {

            return JoinedLobby.Players;
        }
        else
        {
            return HostLobby.Players;
        }
    }

    async Task<bool> LobbyStartCheck()
    {
        if (JoinCode.Length != 0) return false;
        if (JoinedLobby != null)
        {
            updateTimer -= Time.deltaTime;
            if (updateTimer < 0f)
            {
                updateTimer = 5.1f;

                JoinedLobby = await LobbyService.Instance.GetLobbyAsync(JoinedLobby.Id);

                bool exists = false;
                PlayerDataObject nameData = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public);
                PlayerDataObject ourNameData = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public);

                foreach (var player in JoinedLobby.Players)
                {
                    
                    player.Data.TryGetValue("PlayerName", out nameData);
                    ourPlayer.Data.TryGetValue("PlayerName", out ourNameData);

                    if (nameData != null && ourNameData != null)
                    {
                        if (nameData.Value != ourNameData.Value)
                        {
                            continue;
                        }
                        else
                        {
                            exists = true; break;
                        }
                    }
                }

                if(!exists)
                {
                    Debug.Log("Not here");
                    SceneManager.LoadScene("Level1");
                }

                lobbyid = JoinedLobby.Id;
                JoinedLobby.Data.TryGetValue("JoinCode", out var sampleObj);


                if (sampleObj != null)
                {
                    JoinCode = sampleObj.Value;
                    await ConnectClientInLobby();
                    GameObject.Find("WaitingRoomCanvas").gameObject.SetActive(false);
                    return true;
                }
            }
        }
        return false;
    }

        private async Task<bool> LobbyHeartbeat()
    {
        if (HostLobby == null) return false;
        timer -= Time.deltaTime;
        hostTimer -= Time.deltaTime;
        if(timer < 0f)
        {
            float heartbeatTimer = 15f;
            timer = heartbeatTimer;
            try
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(HostLobby.Id);
                return true;
            }
            catch (LobbyServiceException ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }
        if (hostTimer < 0f)
        {
            float heartbeatTimer = 5.1f;
            hostTimer = heartbeatTimer;
            try
            {
                PlayerDataObject playerCheck;
                HostLobby = await LobbyService.Instance.GetLobbyAsync(HostLobby.Id);
                foreach (var kvp in HostLobby.Players)
                {
                    kvp.Data.TryGetValue("QuitLobby", out playerCheck);

                    if (playerCheck != null)
                    {
                        await LobbyService.Instance.RemovePlayerAsync(HostLobby.Id, kvp.Id);
                        HostLobby = await LobbyService.Instance.GetLobbyAsync(HostLobby.Id);
                        break;
                    }
                }
                return true;
            }
            catch (LobbyServiceException ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }
        return false;
    }

    public async void CreateLobby(string _lobbyname)
    {
        try
        {
            string name = "dirtyDan";
            string Username = PlayerPrefs.GetString("USERNAME", name);


            //var allocation = await RelayService.Instance.CreateAllocationAsync(2);
            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "udp"));
            //var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            //JoinCode = joinCode;


            CreateLobbyOptions options = new CreateLobbyOptions();
            options.IsPrivate = false;
            options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "PlayerName", new DataObject(visibility:DataObject.VisibilityOptions.Public,
                    value: Username,
                    index: DataObject.IndexOptions.S1)
                },
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(_lobbyname, 2, options);

            HostLobby = lobby;

            //GameObject.Find("LobbyCanvas").gameObject.SetActive(false);
            

            await LobbyService.Instance.JoinLobbyByIdAsync(HostLobby.Id);

            //Debug.Log(lobby.Name);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogException(ex);
        }
    }



    private async Task<bool> ConnectClientInLobby()
    {
        var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode: JoinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "udp"));
        return !string.IsNullOrEmpty(JoinCode) && NetworkManager.Singleton.StartClient();
           
    }

    public async Task<List<Lobby>> ListLobbies(QueryLobbiesOptions _options = null)
    {
        try
        {
            QueryResponse query = null;
            if (_options == null)
            {
                query = await LobbyService.Instance.QueryLobbiesAsync();
            }

            else
            {
                query = await LobbyService.Instance.QueryLobbiesAsync(_options);
            }

            Debug.Log(query.Results.Count);

            return query.Results;
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogException(ex);

            return null;
        }
    }

    public async Task<Lobby> CreateLobbyWithParams(string _lobbyName, CreateLobbyOptions options, int _maxPlayers)
    {
        try
        {
            if (options.Player != null)
            {
                //Debug.Log("ourPlayerExists");
                ourPlayer = options.Player;
            }
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(_lobbyName, _maxPlayers + 1, options);

            HostLobby = lobby;

            lobbyid = HostLobby.Id;

            GameObject.Find("LobbyCanvas").gameObject.SetActive(false);
            //GameObject.Find("WaitingRoomCanvas").gameObject.SetActive(true);

            //var lobbyResult = await LobbyService.Instance.JoinLobbyByIdAsync(HostLobby.Id);

            //Debug.Log(lobbyResult.Name);

            return lobby;
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogException(ex);
            return null;
        }
    }

    public async Task<List<Lobby>> FindLobbiesOfName(string _lobbyName)
    {
        try
        {
            QueryLobbiesOptions query = new QueryLobbiesOptions
            {
                Count = 5,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.Name, _lobbyName, QueryFilter.OpOptions.EQ)
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse response = await LobbyService.Instance.QueryLobbiesAsync(query);

            return response.Results;

        }
        catch (LobbyServiceException ex)
        {
            Debug.LogException(ex);

            return null;
        }
    }

    public async Task<bool> JoinPrivateLobbyBYID(string _lobbyid, JoinLobbyByIdOptions _options)
    {
        try
        {
            if (_options.Player != null)
            {
                ourPlayer = _options.Player;
            }

            JoinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(_lobbyid, _options);

            lobby_joinname = JoinedLobby.Name;

            lobbyid = JoinedLobby.Id;

            var canvas = GameObject.Find("LobbyCanvas");

            if (canvas != null)
            {
                canvas.SetActive(false);
            }

            return true;
            //GameObject.Find("WaitingRoomCanvas").gameObject.SetActive(true);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogException(ex);
            return false;
        }
    }

    public async Task<bool> JoinLobby(Lobby lobby, JoinLobbyByIdOptions _options)
    {
        try
        {
            

            JoinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, _options);

            lobby_joinname = JoinedLobby.Name;

            PlayerDataObject pdo;
            PlayerDataObject _ourPlayer;

            if (_options.Player != null)
            {
                foreach (Player player in JoinedLobby.Players)
                {
                    player.Data.TryGetValue("PlayerName", out pdo);
                    _options.Player.Data.TryGetValue("PlayerName", out _ourPlayer);
                    if (pdo != null)
                    {
                        if (pdo.Value == _ourPlayer.Value)
                        {
                            ourPlayer = player;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }

            lobbyid = JoinedLobby.Id;

            var canvas = GameObject.Find("LobbyCanvas");

            if (canvas != null)
            {
                canvas.SetActive(false);
            }

            return true;
            //GameObject.Find("WaitingRoomCanvas").gameObject.SetActive(true);
        }
        catch (LobbyServiceException ex) 
        {
            Debug.LogException(ex);
            return false;
        }
    }

    public async Task<bool> CloseLobby()
    {
        try
        {
            JoinedLobby = null;

            HostLobby.Players.ForEach(async p => { if (p.Id != HostLobby.HostId) { await LobbyService.Instance.RemovePlayerAsync(HostLobby.Id, p.Id); } });

            await LobbyService.Instance.DeleteLobbyAsync(HostLobby.Id);

            HostLobby = null;

            return true;
        }
        catch (LobbyServiceException ex )
        {
            Debug.LogException(ex);
            return false;
        }
    }

    public async Task<bool> LobbyScreenQuit()
    {
        try
        {
            if (HostLobby != null)
            {
                await CloseLobby();
                return true;
            }
            else
            {
                string foundPlayerID = "";

                string name = "Foo Fighter";
                string Username = PlayerPrefs.GetString("USERNAME", name);

                UpdatePlayerOptions options = new UpdatePlayerOptions()
                {
                    Data = new Dictionary<string, PlayerDataObject>()
                    {
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, Username)},
                        {"QuitLobby", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "Quit")}
                    }
                };

                var result = await LobbyService.Instance.GetLobbyAsync(JoinedLobby.Id);

                if (result == null) return false;

                PlayerDataObject pdo;
                PlayerDataObject _ourPlayer;

                //foreach (Player player in result.Players)
                //{
                //    player.Data.TryGetValue("PlayerName", out pdo);
                //    ourPlayer.Data.TryGetValue("PlayerName", out _ourPlayer);
                //    if (pdo != null)
                //    {
                //        if (pdo.Value == _ourPlayer.Value)
                //        {
                //            foundPlayerID = _ourPlayer.Value;
                //            break;
                //        }
                //        else
                //        {
                //            continue;
                //        }
                //    }
                //}

                //if (foundPlayerID == "") { return false; }

                await LobbyService.Instance.UpdatePlayerAsync(JoinedLobby.Id, ourPlayer.Id, options);
                //await LobbyService.Instance.RemovePlayerAsync(JoinedLobby.Id, ourPlayer.Id);
                return true;
            }
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogException(ex);
            return false;
        }
    }

    public async Task<string> StartSession()
    {
        if (HostLobby == null) return "";

        try
        {
            string name = "dirtyDan";
            string Username = PlayerPrefs.GetString("USERNAME", name);

            
            var allocation = await RelayService.Instance.CreateAllocationAsync(HostLobby.MaxPlayers);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "udp"));
            
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            

            UpdateLobbyOptions options = new UpdateLobbyOptions();

            options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "PlayerName", new DataObject(visibility:DataObject.VisibilityOptions.Public,
                        value: Username,
                        index:DataObject.IndexOptions.S1)
                },
                {
                    "JoinCode", new DataObject(
                    visibility: DataObject.VisibilityOptions.Public,
                    value: joinCode,
                    index: DataObject.IndexOptions.S2)
                },
        };

            //Ensure you sign-in before calling Authentication Instance
            //See IAuthenticationService interface
            string playerId = AuthenticationService.Instance.PlayerId;

            var lobby = await LobbyService.Instance.UpdateLobbyAsync(HostLobby.Id, options);

            GameObject.Find("WaitingRoomCanvas").gameObject.SetActive(false);


            return NetworkManager.Singleton.StartHost() ? JoinCode : null;

            //...
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return "";
        }
    }
}

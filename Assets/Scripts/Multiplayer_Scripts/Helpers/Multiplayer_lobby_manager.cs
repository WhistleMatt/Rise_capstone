using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
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

    public string ErrorText;

    private float timer = 2f;
    private float updateTimer = 2f;
    private float hostUpdateTimer = 0f;
    private float hostTimer = 5.1f;

    public string OurUserName;

    private LobbyEventCallbacks ourCallbacks = new LobbyEventCallbacks();


    private void Awake()
    {
        Instance = this;
        ourCallbacks.LobbyDeleted += OurCallbacks_LobbyDeleted;
        ourCallbacks.LobbyEventConnectionStateChanged += OurCallbacks_LobbyEventConnectionStateChanged;
        ourCallbacks.KickedFromLobby += OurCallbacks_KickedFromLobby;
        ourCallbacks.PlayerLeft += OurCallbacks_PlayerLeft;
        ourCallbacks.DataChanged += OurCallbacks_DataChanged;
        ourCallbacks.LobbyChanged += OurCallbacks_LobbyChanged;
    }

    private void OurCallbacks_LobbyChanged(ILobbyChanges obj)
    {

        if(JoinedLobby != null)
        {
            obj.ApplyToLobby(JoinedLobby);
        }
        if (HostLobby != null)
        {
            obj.ApplyToLobby(HostLobby);
        }

        if (obj.Data.Value != null)
        {   
            Debug.Log(obj.Data.Value);
            if (obj.Data.Value.Keys.Count != 0)
            {
                foreach(string key in obj.Data.Value.Keys)
                {
                    Debug.Log(key);
                    Debug.Log(obj.Data.Value[key].Value.Value);

                    if (key == "JoinCode")
                    {
                        //JoinCode = obj.Data.Value[key].Value.Value;
                    }

                    if (key == "RedirectLobbyID")
                    {
                        string newLobbyCode = obj.Data.Value[key].Value.Value;
                        
                    }
                }
            }
        }
    }

    private void OurCallbacks_DataChanged(Dictionary<string, ChangedOrRemovedLobbyValue<DataObject>> obj)
    {
        foreach (string key in obj.Keys)
        {
            var value = obj[key];
            Debug.Log(value.Value);
        }
        //throw new System.NotImplementedException();
    }

    private void OurCallbacks_PlayerLeft(List<int> obj)
    {
        //throw new System.NotImplementedException();
    }

    private void OurCallbacks_KickedFromLobby()
    {
        if (HostLobby == null)
        {
            SceneManager.LoadScene("Level1");
        }
    }

    private void OurCallbacks_LobbyEventConnectionStateChanged(LobbyEventConnectionState obj)
    {
        Debug.Log(obj);
    }

    private async void Start()
    {

        string name = "Foo Fighter";
        OurUserName = PlayerPrefs.GetString("USERNAME", name);

        await UnityServices.InitializeAsync();

        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(OurUserName, "SampleText1@3");
        }
        catch (AuthenticationException ex)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(OurUserName, "SampleText1@3");
            }
            catch (AuthenticationException exe)
            {
                Debug.Log(exe);
            }
            
        }

        //await AuthenticationService.Instance.SignInAnonymouslyAsync();

    }

    public string GetLobbyCode()
    {
        return lobbyid;
    }

    public string GetPlayerDataAttribute(string _attribute)
    {
        PlayerDataObject playerData = null;
        ourPlayer.Data.TryGetValue(_attribute, out playerData);
        if (playerData != null)
        {
            return playerData.Value;
        }
        return "Not Found";
    }

    public string GetHostPlayerDataAttribute(string _attribute)
    {
        PlayerDataObject playerData = null;
        foreach (var player in JoinedLobby.Players)
        {
            if (player.Id == JoinedLobby.HostId)
            {
                player.Data.TryGetValue(_attribute, out playerData);
                if (playerData != null)
                {
                    return playerData.Value;
                }
            }
        }
        return "not Found";
    }

    // Update is called once per frame
    async void Update()
    {
        await LobbyHeartbeat();
        await ClientUpdatePoll();
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

    async Task<bool> ClientUpdatePoll()
    {
        if (JoinCode.Length != 0) return false;
        if (JoinedLobby != null)
        {
            updateTimer -= Time.deltaTime;
            if (updateTimer < 0f)
            {
                updateTimer = 5.1f;

                try
                {
                    var result = await LobbyService.Instance.GetLobbyAsync(JoinedLobby.Id);

                    JoinedLobby = result;

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
                                ourPlayer = player;
                                exists = true; 
                                break;
                            }
                        }
                    }

                    if (!exists)
                    {
                        Debug.Log("Not here");
                        SceneManager.LoadScene("Level1");
                    }

                    lobbyid = JoinedLobby.Id;
                    JoinedLobby.Data.TryGetValue("JoinCode", out var sampleObj);

                    string newJoinCode = "";

                    if (sampleObj != null)
                    {
                        newJoinCode = sampleObj.Value;
                    }

                    if (newJoinCode == "")
                    {
                        if (ourPlayer.Id == JoinedLobby.HostId)
                        {
                            var transRes = await AcceptOwnership();
                            return true;
                        }
                    }

                    else
                    {
                        if (JoinCode == null || JoinCode == "")
                        {
                            JoinCode = newJoinCode;
                            await ConnectClientInLobby();
                            GameObject.Find("WaitingRoomCanvas").gameObject.SetActive(false);
                            return true;
                        }
                        else if (JoinCode != newJoinCode && JoinCode.Length != 0)
                        {
                            JoinCode = newJoinCode;
                            if (ourPlayer.Id == JoinedLobby.HostId)
                            {
                                var transRes = await AcceptOwnership();
                                return true;
                            }
                            await ConnectClientInLobby();
                            return true;
                        }
                        return true;
                    }
                }
                catch(LobbyServiceException ex)
                {
                    JoinedLobby = null;
                    await SceneManager.LoadSceneAsync("Level1");
                    return false;
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
                PlayerDataObject playerCheck = null;
                HostLobby = await LobbyService.Instance.GetLobbyAsync(HostLobby.Id);
                foreach (var kvp in HostLobby.Players)
                {
                    kvp.Data.TryGetValue("QuitLobby", out playerCheck);

                    if (playerCheck != null)
                    {
                        await LobbyService.Instance.RemovePlayerAsync(HostLobby.Id, kvp.Id);
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

    private async Task<bool> ConnectClientInLobby()
    {
        if (HostLobby != null || JoinedLobby == null) return false;
        var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode: JoinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "udp"));
        try
        {
            UpdatePlayerOptions options = new UpdatePlayerOptions()
            {
                AllocationId = allocation.AllocationId.ToString(),
                Data = ourPlayer.Data
            };
            await LobbyService.Instance.UpdatePlayerAsync(JoinedLobby.Id, ourPlayer.Id, options);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogException(ex);
            JoinCode = "";
        }

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

            await LobbyService.Instance.SubscribeToLobbyEventsAsync(HostLobby.Id, ourCallbacks);

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
            ErrorText = ex.Message;
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

            await LobbyService.Instance.SubscribeToLobbyEventsAsync(JoinedLobby.Id, ourCallbacks);

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

    public async Task<bool> JoinBackupLobby(string _lobbyID)
    {
        try
        {
            var newLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(_lobbyID);

            return true;
        }
        catch (LobbyServiceException ex)
        {
            return false;
        }
    }

    public async Task<bool> JoinLobby(Lobby lobby, JoinLobbyByIdOptions _options)
    {
        try
        {
            JoinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, _options);

            await LobbyService.Instance.SubscribeToLobbyEventsAsync(JoinedLobby.Id, ourCallbacks);

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
            ErrorText = ex.Message;
            return false;
        }
    }

    private void OurCallbacks_LobbyDeleted()
    {
        Debug.Log("this bitch deleted, yeet.");

        //throw new System.NotImplementedException();
    }

    public async Task<bool> CloseLobby()
    {
        try
        {
            //if (HostLobby.Players.Count > 1)
            //{
            //    await TransferHost();
            //    return true;
            //}

            JoinedLobby = null;

            //HostLobby.Players.ForEach(async p => { if (p.Id != HostLobby.HostId) { await LobbyService.Instance.RemovePlayerAsync(HostLobby.Id, p.Id); } });

            await LobbyService.Instance.DeleteLobbyAsync(HostLobby.Id);

            HostLobby = null;

            NetworkManager.Singleton.Shutdown();

            return true;
        }
        catch (LobbyServiceException ex )
        {
            Debug.LogException(ex);
            return false;
        }
    }

    public async Task<Lobby> TransferHost()
    {
        try
        {
            var newHost = HostLobby.Players.Find(p => (p.Id != HostLobby.HostId));

            if (newHost != null)
            {
                Debug.Log(HostLobby.HostId);
                Debug.Log(newHost.Id);
            }

            UpdateLobbyOptions lobbyOptions = new UpdateLobbyOptions()
            {
                Name = HostLobby.Name,
                IsLocked = HostLobby.IsLocked,
                IsPrivate = HostLobby.IsPrivate,
                HostId = newHost.Id,
                Data = HostLobby.Data,
                MaxPlayers = HostLobby.MaxPlayers,
            };

            var result = await LobbyService.Instance.UpdateLobbyAsync(HostLobby.Id, lobbyOptions);

            if (result != null)
            {
                await LobbyService.Instance.RemovePlayerAsync(result.Id, HostLobby.HostId);
                //SceneManager.LoadScene("Level1");
                return result;
            }

            return null;
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogException(ex);
            return null;
        }
    }

    public async void CreateTransferLobby()
    {
        try
        {
            var newLobby = await LobbyService.Instance.CreateLobbyAsync(HostLobby.Name + "1", HostLobby.MaxPlayers);

            UpdateLobbyOptions lobUpOp = new UpdateLobbyOptions()
            {
                Data = new Dictionary<string, DataObject>
                {
                    {"RedirectLobbyID", new DataObject(DataObject.VisibilityOptions.Member, newLobby.Id, DataObject.IndexOptions.S1) }
                }
            };

            await LobbyService.Instance.UpdateLobbyAsync(HostLobby.Id, lobUpOp);
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    public async Task<string> AcceptOwnership()
    {
        Debug.Log("it is an honor.");

        if (HostLobby != null && JoinedLobby == null)
        {
            return " ";
        }

        HostLobby = JoinedLobby;
        JoinedLobby = null;

        if (JoinCode == null || JoinCode == "")
        {
            return " ";
        }

        var allocation = await RelayService.Instance.CreateAllocationAsync(HostLobby.MaxPlayers);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "udp"));

        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        UpdateLobbyOptions options = new UpdateLobbyOptions();

        options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "PlayerName", new DataObject(visibility:DataObject.VisibilityOptions.Public,
                        value: OurUserName,
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
        //string playerId = AuthenticationService.Instance.PlayerId;

        var lobby = await LobbyService.Instance.UpdateLobbyAsync(HostLobby.Id, options);

        var gameObj = GameObject.Find("WaitingRoomCanvas");

        if (gameObj != null)
        {
            gameObj.gameObject.SetActive(false);
        }

        return NetworkManager.Singleton.StartHost() ? JoinCode : null;
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

                if (JoinedLobby == null)
                {
                    SceneManager.LoadScene("Level1");
                    return true;
                }

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

                PlayerDataObject pdo;
                PlayerDataObject _ourPlayer;

                await LobbyService.Instance.UpdatePlayerAsync(JoinedLobby.Id, ourPlayer.Id, options);
                await LobbyService.Instance.RemovePlayerAsync(JoinedLobby.Id, ourPlayer.Id);
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

            UpdatePlayerOptions playerOpts = new UpdatePlayerOptions()
            {
                AllocationId = allocation.AllocationId.ToString(),
                Data = ourPlayer.Data,
            };

            await LobbyService.Instance.UpdatePlayerAsync(HostLobby.Id, playerId, playerOpts);

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

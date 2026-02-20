using PlayFab.MultiplayerModels;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class Multiplayer_Start_BossWall : NetworkBehaviour
{

    [System.Serializable]
    public struct WallDataStruct : INetworkSerializable
    {
        public int _playerCount;
        public bool _p1Ready;
        public bool _p2Ready;
        public bool _p3Ready;
        public bool _p4Ready;
        public bool _activate;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _playerCount);
            serializer.SerializeValue(ref _p1Ready);
            serializer.SerializeValue(ref _p2Ready);
            serializer.SerializeValue(ref _p3Ready);
            serializer.SerializeValue(ref _p4Ready);
            serializer.SerializeValue(ref _activate);
        }

        public WallDataStruct(int pCount, bool p1 = false, bool p2 = false, bool p3 = false, bool p4 = false, bool activ = false)
        {
            _playerCount = pCount;
            _p1Ready = p1;
            _p2Ready = p2;
            _p3Ready = p3;
            _p4Ready = p4;
            _activate = activ;
        }

        public bool IsActive()
        {
            return _activate;
        }

        public void ReadyPlayerUp(int playerNum, bool isReady)
        {
            switch(playerNum)
            {
                case 0:
                    _p1Ready = isReady;
                    break;
                case 1:
                    _p2Ready = isReady;
                    break;
                case 2:
                    _p3Ready = isReady;
                    break;
                case 3:
                    _p4Ready= isReady;
                    break;
                default:
                    break;
            }
            ValidateUnlock();
        }

        public void UpdatePlayerCount(int newPlayerCount)
        {
            _playerCount = newPlayerCount;
        }

        private void ValidateUnlock()
        {
            switch(_playerCount)
            {
                case 1:
                    if (_p1Ready)
                    {
                        _activate = true;
                    }
                    break;
                case 2:
                    if (_p1Ready && _p2Ready)
                    {
                        _activate = true;
                    }
                    break;
                case 3:
                    if ((_p1Ready && _p2Ready) && _p3Ready)
                    {
                        _activate = true;
                    }
                    break;
                case 4:
                    if ((_p1Ready && _p2Ready) && (_p3Ready && _p4Ready))
                    {
                        _activate = true;
                    }
                    break;
                default:
                    break;

            }
        }
    }

    [SerializeField] private NetworkVariable<WallDataStruct> wallData = new NetworkVariable<WallDataStruct>(new WallDataStruct(1));

    [SerializeField] private NetworkVariable<bool> m_playerOne_entered = new NetworkVariable<bool>(false);
    [SerializeField] private NetworkVariable<bool> m_playerTwo_entered = new NetworkVariable<bool>(false);

    private Network_Player_Controller[] m_controllerList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject _wall;
    //[SerializeField]
    // private List<GameObject> EnemyTemplates = new List<GameObject>();
    // [SerializeField] private List<GameObject> EnemyList = new List<GameObject>();
    [SerializeField] private GameObject _boss;
    [SerializeField] private bool bossBeaten = false;
    private bool has_triggered = false;

    [SerializeField] private GameObject _hpBar;
    // Start is called before the first frame update


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if(wallData.Value.IsActive())
        {
            if(!_boss.gameObject.activeInHierarchy)
            {
                _boss.SetActive(true);
            }
        }
        m_controllerList = GameObject.FindObjectsByType<Network_Player_Controller>(FindObjectsSortMode.InstanceID);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (has_triggered == true)
        {
            return;
        }
        if (other.gameObject.tag == "Player")
        {
            if (!bossBeaten)
            {
                if (m_controllerList.Length == 1)
                {
                    spawnWall();
                    resetEnemies();
                    has_triggered = true;
                }
                else
                {
                    if (wallData.Value.IsActive())
                    {
                        spawnWall();
                        resetEnemies();
                        has_triggered = true;
                    }
                    else
                    {
                        var network_controller = other.GetComponent<Network_Player_Controller>();
                        if (network_controller != null)
                        {
                            UpdatePlayersReadyRpc(network_controller.IsHost, network_controller.OwnerClientId);
                        }
                    }
                    //this.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (has_triggered == true)
        {
            return;
        }
        if (other.gameObject.tag == "Player")
        {
            if (!bossBeaten)
            {
                if (m_controllerList.Length == 1)
                {
                    return;
                }
                else
                {
                    var network_controller = other.GetComponent<Network_Player_Controller>();
                    if (network_controller != null)
                    {
                        CancelPlayersReadyRpc(network_controller.IsHost, network_controller.OwnerClientId);
                    }
                    
                }
                //this.gameObject.SetActive(false);
            }
        }
    }


    private void spawnWall()
    {
        //_wall.gameObject.SetActive(true);

    }

    private void resetEnemies()
    {
        _boss.gameObject.SetActive(true);
        //_hpBar.SetActive(true);

        foreach (Network_Player_Controller player in m_controllerList)
        {
            player.EnableBossUI();
        }

    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void UpdatePlayersReadyRpc(bool _isOwner, ulong _playerID)
    {
        var tempData = wallData.Value;
        tempData.UpdatePlayerCount(m_controllerList.Length);
        tempData.ReadyPlayerUp((int)_playerID, true);
        wallData.Value = tempData;
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void CancelPlayersReadyRpc(bool _isOwner, ulong _playerID)
    {
        var tempData = wallData.Value;
        tempData.UpdatePlayerCount(m_controllerList.Length);
        tempData.ReadyPlayerUp((int)_playerID, false);
        wallData.Value = tempData;
    }

}

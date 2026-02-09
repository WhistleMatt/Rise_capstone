using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class Multiplayer_Start_BossWall : NetworkBehaviour
{

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
                    if (m_playerOne_entered.Value && m_playerTwo_entered.Value)
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
                            UpdatePlayersReadyRpc(network_controller.IsHost);
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
                    if (m_playerOne_entered.Value && m_playerTwo_entered.Value)
                    {
                        var network_controller = other.GetComponent<Network_Player_Controller>();
                        if (network_controller != null)
                        {
                            CancelPlayersReadyRpc(network_controller.IsHost);
                        }
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
    public void UpdatePlayersReadyRpc(bool _isOwner)
    {
        if (_isOwner)
        {
            m_playerOne_entered.Value = true;
        }
        else
        {
            m_playerTwo_entered.Value= true;
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void CancelPlayersReadyRpc(bool _isOwner)
    {
        if (_isOwner)
        {
            m_playerOne_entered.Value = false;
        }
        else
        {
            m_playerTwo_entered.Value = false;
        }
    }

}

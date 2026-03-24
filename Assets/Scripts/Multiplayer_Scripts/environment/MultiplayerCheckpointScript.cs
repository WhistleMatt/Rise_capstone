using UnityEngine;
using Unity.Netcode;
using Unity.Networking;
using System.Collections.Generic;
using FSMC.Runtime;

public class MultiplayerCheckpointScript : NetworkBehaviour
{

    [SerializeField] List<EnemyPathController> _enemies = new List<EnemyPathController>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshAll()
    {
        ResetBreakables();
        ResetEnemies();
        ResetHealsRpc();
        RestoreHP();
    }

    public void ResetBreakables()
    {
        var crates = GameObject.FindObjectsByType<BoxDestroyScript>(FindObjectsSortMode.None);

        foreach (var crate in crates)
        {
            crate.toggleFixedRpc();
        }

    }

    public void ResetEnemies()
    {
        foreach (EnemyPathController enemyPathController in _enemies)
        {
            enemyPathController.gameObject.GetComponent<Multiplayer_Enemy_Stat_Controller>().ResfreshStatsRpc();
            Debug.Log(enemyPathController.gameObject.GetComponent<FSMC_Executer>().GetCurrentState().Name);
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void ResetHealsRpc()
    {
        var PlayerHeals = GameObject.FindObjectsByType<Use_Item_Multiplayer>(FindObjectsSortMode.None);
        foreach (var heal in PlayerHeals)
        {
            heal.ResetHealsRPC();
        }
    }

    public void RestoreHP()
    {
        var stats = GameObject.FindObjectsByType<Network_Player_stats_controller>(FindObjectsSortMode.InstanceID);
        foreach (var hp in stats)
        {
            hp.HealRpc();
        }
    }

}

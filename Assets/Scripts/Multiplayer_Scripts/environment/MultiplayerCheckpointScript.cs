using UnityEngine;
using Unity.Netcode;
using Unity.Networking;
using System.Collections.Generic;
using FSMC.Runtime;

//Written by Matthew Whistle

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
        ResetBreakablesRpc();
        ResetEnemiesRpc();
        ResetHealsRpc();
        RestoreHP();
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void ResetBreakablesRpc()
    {
        var crates = GameObject.FindObjectsByType<BoxDestroyScript>(FindObjectsSortMode.None);

        foreach (var crate in crates)
        {
            crate.toggleFixedRpc();
        }

    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void ResetEnemiesRpc()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        var spawners = GameObject.FindObjectsByType<Multiplayer_Enemy_Spawner>(FindObjectsSortMode.InstanceID);
        foreach (var spawner in spawners)
        {
            spawner.SpawnRpc();
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

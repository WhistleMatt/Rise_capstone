using Unity.Netcode;
using UnityEngine;


//Written by Matthew Whistle
public class Network_Player_stats_controller : NetworkBehaviour
{
    [SerializeField] NetworkVariable<float> networkCurrentHP = new NetworkVariable<float>(0);
    [SerializeField] NetworkVariable<bool> restoreHP = new NetworkVariable<bool>(false);


    [SerializeField] PlayerStatsController playerStatsController;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        //playerStatsController = GetComponent<PlayerStatsController>();
        networkCurrentHP.Value = playerStatsController.getPHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            if (restoreHP.Value)
            {
                HealRpc(false);
                playerStatsController.setPHealth(playerStatsController.getPHealthMax());
            }
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void HealRpc(bool healtrigger = true)
    {
        restoreHP.Value = healtrigger;
    }
}

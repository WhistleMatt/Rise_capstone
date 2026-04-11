using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class Multiplayer_Enemy_Spawner : NetworkBehaviour
{
    [SerializeField] GameObject m_enemyPrefab;
    [SerializeField] Vector3 EulerRotation;

    public override void OnNetworkSpawn()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            SpawnRpc();
        }
        base.OnNetworkSpawn();
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void SpawnRpc()
    {
        var quant = quaternion.EulerXYZ(EulerRotation);
        var obj = Instantiate(m_enemyPrefab, this.transform.position, quant);
        obj.GetComponent<NetworkObject>().Spawn();
    }
}

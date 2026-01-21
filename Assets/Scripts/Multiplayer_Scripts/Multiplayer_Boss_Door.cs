using Unity.Netcode;
using UnityEngine;

public class Multiplayer_Boss_Door : NetworkBehaviour
{
    enum DoorState
    {
        Default = 0,
        Almost_Open = 1,
        Open = 2
    };

    private NetworkVariable<DoorState> m_doorState = new NetworkVariable<DoorState>(DoorState.Default);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_doorState.Value == DoorState.Open)
        {
            Destroy(this.gameObject);
        }
    }

    [ServerRpc]
    public void UpdateDoorStateServerRpc()
    {
        m_doorState.Value += 1;
    }
}

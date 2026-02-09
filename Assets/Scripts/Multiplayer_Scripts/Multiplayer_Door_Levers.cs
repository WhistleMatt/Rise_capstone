using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Multiplayer_Door_Levers : NetworkBehaviour
{
    private Multiplayer_Boss_Door m_doorScript;
    [SerializeField] private Transform _OnPos;
    [SerializeField] private float _Speed;

    [SerializeField] private NetworkVariable<bool> m_LeverPulled = new NetworkVariable<bool>(false);
    [SerializeField] private NetworkVariable<bool> m_LeverMoved = new NetworkVariable<bool>(false);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var doorObject = GameObject.FindGameObjectWithTag("LockedDoor");
        m_doorScript = doorObject.GetComponent<Multiplayer_Boss_Door>();
    }


    // Update is called once per frame
    void Update()
    {
        if (m_LeverPulled.Value == true)
        {
            if (!m_LeverMoved.Value)
            {
                RotateLever();
            }
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void UnlockDoorServerRpc()
    {
        if (m_LeverPulled.Value == true) return;
        m_doorScript.UpdateDoorStateServerRpc();
        m_LeverPulled.Value = true;

    }

    //[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void RotateLever()
    {
        this.transform.rotation = Quaternion.Slerp(transform.rotation, _OnPos.rotation, _Speed * Time.deltaTime);
        if (this.transform.rotation == _OnPos.rotation)
        {
            m_LeverMoved.Value = true;
        }
    }
}

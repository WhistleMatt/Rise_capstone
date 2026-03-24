using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Use_Item_Multiplayer : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI m_useText;
    [SerializeField] private PlayerStatsController playerStats;

    [SerializeField] NetworkVariable<int> m_healVar = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone);

    private int _heals_remaining = 3;


    public void UseHeals(InputAction.CallbackContext context)
    {
        if (!IsOwner)
        {
            return;
        }
        if (context.performed)
        {
            if (m_healVar.Value <= 0)
            {
                return;
            }
            playerStats.setPHealth(playerStats.getPHealthMax());
            HealRpc();
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void HealRpc()
    {
        _heals_remaining -= 1;
        m_healVar.Value = _heals_remaining;
        m_useText.text = _heals_remaining.ToString();
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void ResetHealsRPC()
    {
        m_healVar.Value = 3;
        _heals_remaining = 3;
        m_useText.text = _heals_remaining.ToString();
    }

    private void Update()
    {
        if (IsOwner)
        {
            m_useText.text = _heals_remaining.ToString();
        }
    }
}

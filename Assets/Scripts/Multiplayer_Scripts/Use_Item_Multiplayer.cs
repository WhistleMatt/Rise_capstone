using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Use_Item_Multiplayer : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI m_useText;
    [SerializeField] private PlayerStatsController playerStats;
    private int _heals_remaining = 3;


    public void UseHeals(InputAction.CallbackContext context)
    {
        if (!IsOwner)
        {
            return;
        }
        if (context.performed)
        {
            if (_heals_remaining <= 0)
            {
                return;
            }
            playerStats.setPHealth(playerStats.getPHealthMax());
            _heals_remaining -= 1;
            m_useText.text = _heals_remaining.ToString();
        }
    }

    public void ResetHeals()
    {
        _heals_remaining = 3;
        m_useText.text = _heals_remaining.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

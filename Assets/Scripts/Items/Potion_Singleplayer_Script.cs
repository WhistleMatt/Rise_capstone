using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Potion_Singleplayer_Script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private TextMeshProUGUI m_useText;
    [SerializeField] private PlayerStatsController playerStats;
    private int _heals_remaining = 3;

    void Start()
    {
        m_useText.text = _heals_remaining.ToString();
    }

    public void UseHeals(InputAction.CallbackContext context)
    {
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

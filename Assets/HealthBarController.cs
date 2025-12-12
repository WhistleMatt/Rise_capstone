using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Nicolas Chatziargiriou
public class HealthBarController : MonoBehaviour
{
    public Slider _slider;
    public GameObject _player;
    private PlayerStatsController m_playerStatsController;
    private CompanionProperties m_companionProperties;
    public GameObject m_companion;
    public bool m_IsStamina;

    public void SetMaxHealth(float health)
    {
        _slider.maxValue = health;
    }

    public void SetHealth(float health)
    {
        _slider.value = health;
    }

    public void SetMaxStamina(float stamina)
    {
        _slider.maxValue = stamina;
    }

    public void SetStamina(float stamina)
    {
        _slider.value = stamina;
    }

    private void Awake()
    {
        if(_player.tag == "Player")
        { 
            m_playerStatsController = _player.GetComponent<PlayerStatsController>(); 
        }
        else
        {
            m_companionProperties = _player.GetComponent<CompanionProperties>();
        }

       
    }

    private void Update()
    {
        if(_player.tag == "Player")
        {
            if(!m_IsStamina) 
            {
                SetMaxHealth(m_playerStatsController.getPHealthMax());
                SetHealth(m_playerStatsController.getPHealth());
            }
            else
            {
                SetMaxStamina(m_playerStatsController.getStaminahMax());
                SetStamina(m_playerStatsController.getPStamina());
            }
        }
        else if(_player.tag == "Companion")
        {
            if (!m_IsStamina)
            {
                SetMaxHealth(m_companionProperties.GetMaxHP());
                SetHealth(m_companionProperties.GetComponent<CompanionProperties>().GetCurrentHP());
            }
            else
            {
                SetMaxStamina(m_companionProperties.GetMaxStamina());
                SetStamina(m_companionProperties.GetCurrentStamina());
            }
        }
        
    }

}

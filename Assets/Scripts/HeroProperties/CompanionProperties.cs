using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionProperties : MonoProperties
{
    [SerializeField] private int m_maxHP;
    [SerializeField] private int m_currentHP;
    [SerializeField] private int m_maxStamina;
    [SerializeField] public int m_currentStamina;
    [SerializeField] private float m_staminaRegen;
    [SerializeField] private int m_defenseFlat;
    [SerializeField] private int m_defensePercept;
    private int m_startHP;
    // Start is called before the first frame update

    private void Start()
    {
        m_startHP = m_currentHP;
    }

    public override int GetCurrentHP()
    {
        return m_currentHP;
    }

    public override int GetMaxHP() 
    {
        return m_maxHP;
    }

    public override int GetCurrentStamina()
    {
        return m_currentStamina;
    }

    public override int GetMaxStamina()
    {
        return m_maxStamina;
    }

    public override float GetStaminaRegen() 
    {
        return m_staminaRegen;
    }

    public override void SetCurrentStamina(int _stam)
    {
        if (m_currentStamina < _stam)
        {
            m_currentStamina = 0;
            return;
        }
        m_currentStamina -= _stam;
    }

    public override int Heal(int _recovery)
    {
        int post_heal = m_currentHP + _recovery;
        if (post_heal > m_maxHP)
        {
            int difference = m_maxHP - post_heal;
            m_currentHP = m_maxHP;
            return difference;
        }
        m_currentHP = post_heal;
        return 1;
    }

    public override void Reset()
    {
        m_currentHP = m_startHP;
    }

    private void Update()
    {
        if((m_currentStamina + m_staminaRegen) <= m_maxStamina)
        {
            m_currentStamina += (int)m_staminaRegen;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Health_Potion : MonoGameItem
{
    [SerializeField] private int m_baseHeal;
    [SerializeField] private int m_level;
    [SerializeField] private int m_charges;
    private int m_startCharges;
    // Start is called before the first frame update
    void Start()
    {
        m_startCharges = m_charges;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override int GetCharges()
    {
        return m_charges;
    }

    public override int UseItem(GameObject _user)
    {
        if (m_charges > 0) 
        {
            m_charges--;
            return _user.GetComponent<MonoProperties>().Heal((m_baseHeal * m_level));
        }
        return -1;
    }

    public override void Reset()
    {
        m_charges = m_startCharges;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IITem
{
    private string m_itemID;
    private string m_name;
    private bool m_targetPlayer;
    private bool m_isVoidItem;
    private int m_uses;
    private string m_image;

    public string ItemID 
    { 
        get => m_itemID; 
        set
        {
            m_itemID = value;
        }
    }

    public string Name
    {
        get => m_name;
        set
        {
            m_name = value;
        }
    }

    public bool TargetPlayer
    {
        get => m_targetPlayer;
        set
        {
            m_targetPlayer = value;
        }
    }

    public bool IsVoidItem
    {
        get => m_isVoidItem;
        set
        {
            m_isVoidItem = value;
        }
    }

    public int Uses
    {
        get => m_uses;
        set
        {
            m_uses = value;
        }
    }

    public string Image
    {
        get => m_image;
    }

    public void SetImageString(string imageString)
    {
        m_image = imageString;
    }


    public virtual void UseItem()
    {
        return;
    }

    public virtual void UseItem(PlayerStatsController stats)
    {
        return;
    }

}

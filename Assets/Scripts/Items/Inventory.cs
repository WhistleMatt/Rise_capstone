using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Inventory : MonoBehaviour
{
    public MonoGameItem[] m_hotbarItems = new MonoGameItem[8];
    private int m_index;
    public MonoGameItem m_itemSelected;
    // Start is called before the first frame update
    private void Start()
    {
        if(m_hotbarItems.Length > 0)
        {
            m_index = 0;
            m_itemSelected = m_hotbarItems[0];
        }
    }

    public float SwapSelectedItem(int lr)
    {
        MonoGameItem gameItem;
        int tempIndex = m_index;
        bool itemSwapped = false;
        switch (lr)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {   
                    if(m_index == 7)
                    {
                        tempIndex = 0;
                        gameItem = m_hotbarItems[tempIndex];
                    }
                    else
                    {
                        tempIndex += 1;
                        gameItem = m_hotbarItems[tempIndex];
                    }
                    if(gameItem.m_itemName != "empty")
                    {
                        itemSwapped = true;
                        m_itemSelected = gameItem;
                        m_index = tempIndex;
                    }
                    break;
                }
            case 2:
                {
                    {
                        if (m_index == 0)
                        {
                            tempIndex = 7;
                            gameItem = m_hotbarItems[tempIndex];

                        }
                        else
                        {
                            tempIndex -= 1;
                            gameItem = m_hotbarItems[tempIndex];
                        }
                        if (gameItem.m_itemName != "empty")
                        {
                            itemSwapped = true;
                            m_itemSelected = gameItem;
                            m_index = tempIndex;
                        }
                        break;
                    }
                }
                default:
                {
                    break;
                }
        }
        if(itemSwapped)
        {
            return 0.02f;
        }
        else
        {
            return -0.15f;
        }
    }

    public float UseSelectedItem()
    {
        if(m_itemSelected != null)
        {
            return m_itemSelected.UseItem(this.gameObject);
        }
        return -0.5f;
    }

    public void Reset()
    {
        foreach(var item in m_hotbarItems)
        {
            item.Reset();
        }
        m_itemSelected = m_hotbarItems[0];
    }
}

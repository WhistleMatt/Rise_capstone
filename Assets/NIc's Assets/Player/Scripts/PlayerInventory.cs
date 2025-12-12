using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    PlayerStatsController m_playerStatsController;
    //HashSet<IITem> m_ItemHash;
    List<IITem> m_items;
    IITem m_selectedItem;

    public UnityEvent ItemSwapEvent;

    private int m_selectedIndex;
    // Start is called before the first frame update

    private void Awake()
    {
        m_items = new List<IITem>();
        var item = new Healing_Item();
        m_items.Add(item);
        m_items.Add(new VoidItem());
        m_selectedItem = m_items[0];
    }

    void Start()
    {
        m_playerStatsController = GetComponent<PlayerStatsController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchItemSelected(int direction)
    {
        if(m_selectedIndex + direction > m_items.Count)
        {
            m_selectedIndex = 0;
        }
        else if(m_selectedIndex + direction < m_items.Count)
        {
            m_selectedIndex = m_items.Count - 1;
        }
        else
        {
            m_selectedIndex += direction;
        }
        m_selectedItem = m_items[m_selectedIndex];
        ItemSwapEvent.Invoke();
    }

    public void UseSelectedItem()
    {
        if(m_selectedItem.TargetPlayer)
        {
            m_selectedItem.UseItem(m_playerStatsController);
            return;
        }
        m_selectedItem.UseItem();
    }

    public List<IITem> GetInventory()
    {
        return m_items;
    }

    public IITem GetCurrentItemSelected()
    {
        return m_selectedItem;
    }

    public IITem GetItemAtIndex(int index)
    {
        if(index > m_items.Count)
        {
            return m_items[0];
        }
        return m_items[index];
    }

    public int GetSelectedIndex() { return m_selectedIndex; }
}

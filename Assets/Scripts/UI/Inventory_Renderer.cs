using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Renderer : MonoBehaviour
{
    public PlayerInventory inventory;
    [SerializeField] RawImage m_frontImage;
    [SerializeField] RawImage m_backImage;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        m_frontImage.texture = Resources.Load<Texture>(inventory.GetCurrentItemSelected().Image);
        m_backImage.texture = Resources.Load<Texture>(inventory.GetItemAtIndex(inventory.GetSelectedIndex() + 1).Image);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateItemIcons()
    {
        m_frontImage.texture = Resources.Load<Texture>(inventory.GetCurrentItemSelected().Image);
        m_backImage.texture = Resources.Load<Texture>(inventory.GetItemAtIndex(inventory.GetSelectedIndex() + 1).Image);
    }
}

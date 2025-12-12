using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hover_Over_Ui : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool updateable = false;
    public bool selected;
    public int m_swapID;
    public bool m_isButton;

    [SerializeField] Image m_image;
    [SerializeField] TextMeshProUGUI m_text;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(selected)
        {
            m_image.color = Color.blue;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_image.color = Color.blue;
        if(!m_isButton)
        {
            UnselectOthers();
        }
        selected = true;
        if(!m_isButton)
        {
            UIManager.instance.SwapOptions(m_swapID);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //m_image.color = Color.red;
    }

    public void UnselectOthers()
    {
        var others = GameObject.FindGameObjectsWithTag("OptionUI");
        foreach (var other in others)
        {
            if (other != this.gameObject)
            {
                other.GetComponent<Hover_Over_Ui>().UnselectSelf();
            }
        }
    }

    public void UnselectSelf()
    {
        if(!m_isButton)
        {
            m_image.color = Color.black;
            selected = false;
        }
    }
}

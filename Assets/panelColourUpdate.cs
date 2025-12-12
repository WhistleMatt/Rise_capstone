using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class panelColourUpdate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {

        this.GetComponent<Image>().color = new Color(0, 0, 255, 255);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        this.GetComponent<Image>().color = new Color(0, 0, 0, 255);
    }
}

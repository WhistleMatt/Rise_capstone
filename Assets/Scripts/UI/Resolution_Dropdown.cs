using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Resolution_Dropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown m_dropdown;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Screen.fullScreen)
        {
            m_dropdown.interactable = false;
            GetComponent<Image>().color = Color.gray;
        }
        else
        {
            m_dropdown.interactable = true;
            GetComponent<Image>().color = Color.white;
        }
    }

    public void ChangeResolution()
    {
        if (m_dropdown.options[m_dropdown.value].text == "1920x780")
        {
            Screen.SetResolution(1920, 780, false);
        }
        else if (m_dropdown.options[m_dropdown.value].text == "1280x620")
        {
            Screen.SetResolution(1280, 620, false);
        }
    }

}

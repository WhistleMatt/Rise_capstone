using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//using Cinemachine;
using Unity.Cinemachine;

public class getTextMouse : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] CinemachineFreeLook _camera;
    [SerializeField] bool isx;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<TMP_Text>().text =_slider.value.ToString();
        if (isx)
        {
            _camera.m_XAxis.m_MaxSpeed = _slider.value / 10;
            GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().updateMouseSettingsX((int)_slider.value);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsController>()._mouseX = (int)_slider.value;
        }

        else if (!isx)
        {
            _camera.m_YAxis.m_MaxSpeed = _slider.value / 100;
           GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().updateMouseSettingsY((int)_slider.value);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsController>()._mouseY = (int)_slider.value;
        }
            
    }
}

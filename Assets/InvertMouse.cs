using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;
public class InvertMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().mouseInvert == 0)
        {
            this.GetComponent<Toggle>().isOn = false;
        }
        if (GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().mouseInvert == 1)
        {
            this.GetComponent<Toggle>().isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void invertMouse()
    {
        if (this.GetComponent<Toggle>().isOn)
        {
            GameObject.FindGameObjectWithTag("cinemaCam").GetComponent<CinemachineFreeLook>().m_YAxis.m_InvertInput = true;
            GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().updateMouseInvert(1);
        }
        else if (!this.GetComponent<Toggle>().isOn)
        {
            GameObject.FindGameObjectWithTag("cinemaCam").GetComponent<CinemachineFreeLook>().m_YAxis.m_InvertInput = false;
            GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().updateMouseInvert(0);
        }
    }
}

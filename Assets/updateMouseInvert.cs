using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class updateMouseInvert : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().mouseInvert==0)
        {
            GameObject.FindGameObjectWithTag("cinemaCam").GetComponent<CinemachineFreeLook>().m_YAxis.m_InvertInput = false;
        }
        if (GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().mouseInvert == 1)
        {
            GameObject.FindGameObjectWithTag("cinemaCam").GetComponent<CinemachineFreeLook>().m_YAxis.m_InvertInput = true;
        }

    }
}

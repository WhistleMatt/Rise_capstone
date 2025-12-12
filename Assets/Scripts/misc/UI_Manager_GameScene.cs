using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Manager_GameScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_healthDisplay;
    [SerializeField] TextMeshProUGUI m_staminaDisplay;
    [SerializeField] MonoProperties m_properties;
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_healthDisplay.text = m_properties.GetCurrentHP().ToString();
        m_staminaDisplay.text = m_properties.GetCurrentStamina().ToString();
    }
}

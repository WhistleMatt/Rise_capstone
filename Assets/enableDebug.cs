using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class enableDebug : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _debugger;

    public void toggleDebug()
    {
        if (this.GetComponent<Toggle>().isOn)
        {
            _debugger.GetComponent<AssetToggle>().enable();
            _debugger.GetComponent<FileWriter>().enable();
           // GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().updateMouseInvert(1);
        }
        else if (!this.GetComponent<Toggle>().isOn)
        {
            _debugger.GetComponent<AssetToggle>().disaable();
            _debugger.GetComponent<FileWriter>().disable();
        }
    }
}

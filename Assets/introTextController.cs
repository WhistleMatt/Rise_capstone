using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introTextController : MonoBehaviour
{
    private GameObject _soulGirl;
    // Start is called before the first frame update
    void Start()
    {
        _soulGirl = GameObject.FindGameObjectWithTag("SoulGirl");
        _soulGirl.GetComponent<DialogueController>().recieveDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

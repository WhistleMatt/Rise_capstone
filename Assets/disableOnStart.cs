using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var networkStatCont = GetComponent<Multiplayer_WizardController>();
        if (networkStatCont != null) return;
        this.gameObject.SetActive(false);
    }

    private void Awake()
    {
        //GameObject.FindGameObjectWithTag("SoulGirl").GetComponent<DialogueController>().recieveDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

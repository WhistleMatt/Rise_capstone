using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//Nicolas Chatziargiriou
public class playerEnvironmentInteraction : MonoBehaviour
{

    public bool showRay=false;
    [SerializeField]
    private float rayLength=1;
    public PlayerInPutActions playerControls;
    private GameObject interactObject;

        
    private InputAction interact;


    private void OnEnable()
    {
        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void setInteractable(GameObject _object)
    {
        interactObject = _object;
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Set: " + interactObject);
    }
    public GameObject getInteractObj()
    {
        return interactObject;
    }
    private void Awake()
    {
        playerControls = new PlayerInPutActions();
    }
    private void Interact(InputAction.CallbackContext context)
    {
       // RaycastHit hit;
       // Vector3 fwd = transform.TransformDirection(Vector3.forward);
        //if (Physics.Raycast(transform.position,fwd,out hit,rayLength ))
        if (interactObject != null)
        {
           if(interactObject.transform.gameObject.tag=="bonFire")
            {
                interactObject.transform.gameObject.GetComponent<BonfireController>().useBonfire();
                GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Call: Bon Fire");
            }
            if (interactObject.transform.gameObject.tag == "Teleporter")
            {
                interactObject.transform.gameObject.GetComponent<Teleport>().teleport();
                GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Call: Knight Statue");
            }
            if (interactObject.transform.gameObject.tag== "SoulGirl")
            {
                GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().run = true;
                interactObject.transform.gameObject.GetComponent<DialogueController>().recieveDialogue();
                GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Call: Soul Girl");
            }
            if (interactObject.transform.gameObject.tag == "Tutorial Girl")
            {
                GameObject.FindGameObjectWithTag("SoulGirl").GetComponent<SoulsGirlDialogue>().chapter = 8;
                GameObject.FindGameObjectWithTag("SoulGirl").GetComponent<DialogueController>().recieveDialogue();
                GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Call: Tutorial Girl");
            }
                if (interactObject.transform.gameObject.tag == "Lever")
            {
                interactObject.transform.gameObject.GetComponentInChildren<LeverController>().open();
                GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Call: Lever");
            }
        }
    }
    private void Update()
    {
        //Debug.Log(interactObject);
     //   if (showRay)
       // Debug.DrawRay(new Vector3(transform.position.x,transform.position.y+1,transform.position.z), transform.TransformDirection(Vector3.forward)*rayLength, Color.red);
    }
}

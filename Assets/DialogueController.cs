using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject textBackground;
    [SerializeField] private TextMeshProUGUI gui;
    [SerializeField] private float speed = 0.01f;
    

    private string gameText=null;
  
  
    private int currentLetter = 0;
    private float timer = 0.0f;
    private bool donePage = false;
    private bool lockNext = true;
 

    public PlayerInPutActions dialogueControls;
    private InputAction next;
    public PlayerInput m_playerInput;

    private void OnEnable()
    {
        next = dialogueControls.DialogueBox.Next;
        next.Enable();
        next.performed += Next;
    }

    private void OnDisable()
    {
        dialogueControls.Disable();
    }
    private void Awake()
    {
        dialogueControls = new PlayerInPutActions();
       // recieveDialogue();
    }

    private void Start()
    {
        m_playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    public void toggleLock(bool _lock)
    {
        lockNext = _lock;
    }
    private void Next(InputAction.CallbackContext context)
    {
        Debug.Log(lockNext);
        Debug.Log(donePage);
        if (!lockNext && donePage)
        {
            donePage = false;
            GameObject.FindGameObjectWithTag("U Button").GetComponent<AudioSource>().Play();
            recieveDialogue();
        }
        else if (!lockNext && !donePage)
       {
            currentLetter = gameText.Length;
        }
      
        
    }
   

    private void Update()
    {
    if (gameText!=null)
        {
            m_playerInput.SwitchCurrentActionMap("DialogueBox");
            gui.gameObject.transform.parent.gameObject.SetActive(true);
            textBackground.SetActive(true);
            GameObject.FindGameObjectWithTag("cinemaCam").GetComponent<CinemachineInputProvider>().enabled = false;
            printText();
        }
    else if (gameText==null)
        {
          //  m_playerInput.SwitchCurrentActionMap("Player");
            gui.gameObject.transform.parent.gameObject.SetActive(false);
            textBackground.SetActive(false);
            GameObject.FindGameObjectWithTag("cinemaCam").GetComponent<CinemachineInputProvider>().enabled = true;
        }
        //dialogue toggle
      /* if (gui.gameObject.transform.parent.gameObject.active==false)
        {
            GameObject.FindGameObjectWithTag("SoulGirl").GetComponent<DialogueController>().toggleLock(true);
        }*/
       
    }

    public void printText()
    {
       // Debug.Log(timer);
                timer = timer + Time.deltaTime;
                if (currentLetter > gameText.Length)
                {
                    currentLetter = 0;
                     donePage = true;

        }
                if (timer >= speed &&!donePage)
                {
                    gui.text = gameText.Substring(0, currentLetter);
            GameObject.FindGameObjectWithTag("Text Box").GetComponentInChildren<AudioSource>().Play();
                    currentLetter++;
                    timer = 0.0f;
                    
                }

     }
       
    


    public  void recieveDialogue()
    {
        //m_playerInput.SwitchCurrentActionMap("DialogueBox");
        gameText = this.gameObject.GetComponent<SoulsGirlDialogue>().getDialogue();

    }

    public void finishPage()
    {
        currentLetter = 0;
       donePage = true;
    }

    public void startPage()
    {
        donePage = false;
    }
    
    public void DisableNext()
    {
        next.Disable();
    }

    public void RenableNext()
    {
        next.Enable();
    }

    





    /*
public void showText()
{
    string oldText;
    if (gui.gameObject.active)
    {
        if (!this.gameObject.GetComponent<SoulsGirlFlags>().getFlagBDE() && !pageFinished)
        {
            recieveDialogue();
            oldText = gameText;


            timer = timer + Time.deltaTime;
            if (currentLetter > gameText.Length)
            {
                currentLetter = 0;
                recieveDialogue();
                if (oldText!=gameText)
                {

                }

            }
            if (timer >= speed)
            {
                gui.text = gameText.Substring(0, currentLetter);
                currentLetter++;
                timer = 0.0f;
            }

        }
    }



}
*/
}

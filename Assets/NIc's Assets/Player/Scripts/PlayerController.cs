using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//Nicolas Chatziargiriou

//References:
//https://www.youtube.com/watch?v=4HpC--2iowE
//https://www.youtube.com/watch?v=HmXU4dZbaMw

public class PlayerController : MonoInputController
{
    private enum PlayerState
    {
        Unpaused = 0,
        Paused = 1
    }

    public CharacterController controller;
    private PlayerStatsController playerStatsController;
    public Transform cam;
    public float moveSpeed;
    public PlayerInPutActions playerControls;
    public float gravity;
    public float jumpPower;
    public Animator animatorController;
    
    private PlayerState m_playState = PlayerState.Unpaused;
    private int m_playStateSwapped = 0;
    //ublic float mass;

    private InputAction move;
    private InputAction jump;
    public PlayerInput input;
    private bool updateStats = false;
    private bool hasSpawned = true;

    public bool isRolling = false;
    public bool isBlocking = false;
    Vector3 moveDirection = Vector3.zero;

    public float turnSmoothTime;
    float turnVelocitySmooth;
    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

      //  jump = playerControls.Player.Jump;
      //  jump.Enable();
      //  jump.performed += Jump;
    }
    private void OnDisable()
    {
        move.Disable();
      //  jump.Disable();
    }

    private void Awake()
    {
        playerControls = new PlayerInPutActions();
        input = GetComponent<PlayerInput>();
        playerStatsController = GetComponent<PlayerStatsController>();
       Cursor.lockState = CursorLockMode.Locked;
        //updateStats = true;
    }

    private void Start()
    {
       

     //  GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().getMouseSettings();
        Cursor.visible = false;
        PlayFabStats.Instance.GetStatistics();

        if (PlayFabStats.Instance.JustStarted == 0)
        {
            //transform.position = new Vector3 (6f, 0.125f, 0f);
            //PlayFabStats.Instance.UpdateStats((int)playerStatsController.getPHealthMax(), (int)playerStatsController.getPHealth(), (int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)gameObject.transform.position.z, 1, (int)playerStatsController.getManaMax(), (int)playerStatsController.getPMana(), (int)playerStatsController.getStaminahMax(), (int)playerStatsController.getPStamina(), (int)playerStatsController.getAttckMax(), (int)playerStatsController.getPAttck(), (int)playerStatsController.getPDefenseMax(), (int)playerStatsController.getPDefense(), (int)playerStatsController.getExperiancePoints());
        }

        if (transform.position != PlayFabStats.Instance.GetAsVector())
        {
            hasSpawned = false;
        }

        //Debug.Log(vec);

        //transform.position = new Vector3(x, y, z);
    }
    /* 
     private bool getBlockState()
     {
         return this.GetComponent<InputController>().checkBlock();
     }
    */

    public void UnlockMouse(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (m_playStateSwapped == 1)
            {
                m_playStateSwapped = 0;
                return;
            }
            else
            {
                m_playStateSwapped = 1;
            }
            if (m_playState == PlayerState.Unpaused)
            {
                Cursor.lockState = CursorLockMode.Confined;
                input.SwitchCurrentActionMap("UI");
                UIManager.instance.PauseMenu();
                m_playState = PlayerState.Paused;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                input.SwitchCurrentActionMap("Player");
                UIManager.instance.Unpause();
                m_playState = PlayerState.Unpaused;
            }
        }
    }

    public void LockMouse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
           Cursor.lockState = CursorLockMode.Locked;
            input.SwitchCurrentActionMap("Player");
            UIManager.instance.Unpause();
        }
           
    }

    private bool performingAction()
    {
        return this.GetComponent<Animator>().GetBool("performingAction");
    }
    /*
    private void Jump(InputAction.CallbackContext context)
    {
        if (controller.isGrounded)
        {
           
            controller.Move(new Vector3(0, jumpPower, 0) * Time.deltaTime);
        }
    }*/
    void Update()
    {
        if (updateStats==true)
        {
           // GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().run = true;
        }
        if(!hasSpawned)
        {
            transform.position = PlayFabStats.Instance.GetAsVector();
            if (transform.position == PlayFabStats.Instance.GetAsVector())
            {
                hasSpawned = true;
            }
            return;
        }

        if (input.currentActionMap.name != "DialogueBox" &&( !isRolling && !isBlocking) )
        {
            if (m_playState == PlayerState.Paused) 
            {
                moveDirection = Vector3.zero;
                return; 
            }
            moveDirection = move.ReadValue<Vector3>();
            moveDirection.Normalize();
            //if (/*!getBlockState()*/!performingAction() && moveDirection != Vector3.zero)
            /*{
                animatorController.SetBool("isWalking", true);
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocitySmooth, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirectionUpdated = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDirectionUpdated.normalized * moveSpeed * Time.deltaTime);
                updateStats = false;
            }
            else
            {
                animatorController.SetBool("isWalking", false);
            }
                //apply gravity 
                if (!controller.isGrounded)
                {
                    controller.Move(new Vector3(0, gravity, 0) * Time.deltaTime);

                }
            */
        }
            

    }

    private void FixedUpdate()
    {
        if (/*!getBlockState()*/!performingAction() && moveDirection != Vector3.zero)
        {
            animatorController.SetBool("isWalking", true);
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocitySmooth, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirectionUpdated = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirectionUpdated.normalized * moveSpeed * Time.deltaTime);
            updateStats = false;
        }
        else
        {
            animatorController.SetBool("isWalking", false);
        }
        //apply gravity 
        if (!controller.isGrounded)
        {
            controller.Move(new Vector3(0, gravity, 0) * Time.deltaTime);

        }
    }
}

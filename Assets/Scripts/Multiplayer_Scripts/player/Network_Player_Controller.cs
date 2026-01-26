using Unity.Netcode;
using Unity.Networking;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Network_Player_Controller : NetworkBehaviour
{
    public CharacterController controller;
    private PlayerStatsController playerStatsController;
    public Transform cam;
    public GameObject m_cinemachine_cam;
    public AudioListener audioListener;
    public float moveSpeed;
    public PlayerInPutActions playerControls;
    public float gravity;
    public float jumpPower;
    public Animator animatorController;
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

    public struct NetworkVectorData : INetworkSerializable
    {
        public float _x;
        public float _y;
        public float _z;
        public float _angle;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T: IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
            serializer.SerializeValue(ref _z);
            serializer.SerializeValue(ref _angle);
        }

        public NetworkVectorData(float x = 0, float y = 0, float z = 0, float angle = 0)
        {
            _x = x;
            _y = y;
            _z = z;
            _angle = angle;
        }
    }

    private enum PlayerState
    {
        Unpaused = 0,
        Paused = 1
    }

    private PlayerState m_playState = PlayerState.Unpaused;
    private int m_playStateSwapped = 0;

    [SerializeField] private GameObject m_HealItemCanvas;
    [SerializeField] private GameObject m_PlayerUICanvas;

    //private void OnEnable()
    //{
    //move = playerControls.Player.Move;
    //move.Enable();

    //  jump = playerControls.Player.Jump;
    //  jump.Enable();
    //  jump.performed += Jump;
    //}
    //private void OnDisable()
    //{
    //move.Disable();
    //  jump.Disable();
    //}

    //private void Awake()
    //{

    //updateStats = true;
    //}

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnServerStopped += Singleton_OnServerStopped;

        playerControls = new PlayerInPutActions();
        input = GetComponent<PlayerInput>();
        playerStatsController = GetComponent<PlayerStatsController>();
        move = playerControls.Player.Move;
        move.Enable();
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        m_HealItemCanvas.SetActive(true);
        m_PlayerUICanvas.SetActive(true);
        //PlayFabStats.Instance.GetStatistics();

        //if (PlayFabStats.Instance.JustStarted == 0)
        //{
            //transform.position = new Vector3 (6f, 0.125f, 0f);
            //PlayFabStats.Instance.UpdateStats((int)playerStatsController.getPHealthMax(), (int)playerStatsController.getPHealth(), (int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)gameObject.transform.position.z, 1, (int)playerStatsController.getManaMax(), (int)playerStatsController.getPMana(), (int)playerStatsController.getStaminahMax(), (int)playerStatsController.getPStamina(), (int)playerStatsController.getAttckMax(), (int)playerStatsController.getPAttck(), (int)playerStatsController.getPDefenseMax(), (int)playerStatsController.getPDefense(), (int)playerStatsController.getExperiancePoints());
        //}

        //if (transform.position != PlayFabStats.Instance.GetAsVector())
        //{
            //hasSpawned = false;
        //}

        base.OnNetworkSpawn();
    }

    private void Singleton_OnServerStopped(bool obj)
    {
        SceneManager.LoadScene("Level1");

        //throw new System.NotImplementedException();
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        Debug.Log(NetworkObjectId);

        //throw new System.NotImplementedException();
    }

    private void Singleton_OnClientDisconnectCallback(ulong obj)
    {
        if(IsHost) return;

        SceneManager.LoadScene("Level1");

        //throw new System.NotImplementedException();
    }

    private void Start()
    {


        //  GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().getMouseSettings();
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
        if (!IsOwner) return;
        if (context.performed)
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
                Cursor.visible = true;
                input.SwitchCurrentActionMap("UI");
                m_cinemachine_cam.SetActive(false);
                Multiplayer_UI_Manager.instance.PausePlayer();
                m_playState = PlayerState.Paused;
                m_HealItemCanvas.SetActive(false);
                m_PlayerUICanvas.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                input.SwitchCurrentActionMap("Player");
                m_cinemachine_cam.SetActive(true);
                Multiplayer_UI_Manager.instance.ExitCanvas();
                m_playState = PlayerState.Unpaused;
                m_HealItemCanvas.SetActive(true);
                m_PlayerUICanvas.SetActive(false);
            }

        }
    }

    public void LockMouse(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
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
        if (!IsOwner) return;

        if (!cam.gameObject.activeInHierarchy)
        {
            cam.gameObject.SetActive(true);
            m_cinemachine_cam.SetActive(true);
            audioListener.enabled = true;
        }

        if (updateStats == true)
        {
            // GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().run = true;
        }
        if (!hasSpawned)
        {
            transform.position = PlayFabStats.Instance.GetAsVector();
            if (transform.position == PlayFabStats.Instance.GetAsVector())
            {
                hasSpawned = true;
            }
            return;
        }

        if (input.currentActionMap.name != "DialogueBox" && (!isRolling && !isBlocking))
        {
            if (m_playState == PlayerState.Paused)
            {
                moveDirection = Vector3.zero;
                return;
            }
            moveDirection = move.ReadValue<Vector3>();
            if (!IsHost) return;
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

    public bool AreWeOwner()
    {
        return IsOwner;
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        

        if (/*!getBlockState()*/!performingAction() && moveDirection != Vector3.zero)
        {
            
            animatorController.SetBool("isWalking", true);
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocitySmooth, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirectionUpdated = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (!IsHost)
            {
                NetworkVectorData data = new NetworkVectorData(moveDirectionUpdated.x, moveDirectionUpdated.y, moveDirectionUpdated.z, angle);
                SetPositionServerRpc(data, OwnerClientId);
                return;
            }
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

    [ServerRpc]
    public void SetPositionServerRpc(NetworkVectorData _data, ulong playerID)
    {
        if (OwnerClientId != playerID) return;

        Vector3 newMoveVector = new Vector3(_data._x, _data._y, _data._z);
        if (/*!getBlockState()*/!performingAction() && newMoveVector != Vector3.zero)
        {
            transform.rotation = Quaternion.Euler(0f, _data._angle, 0f);

            controller.Move(newMoveVector.normalized * moveSpeed * Time.deltaTime);
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
        //NetworkVectorData data = new NetworkVectorData(moveDirection.x, moveDirection.y, moveDirection.z);
        

    }

    public void UnpauseFromUI()
    {
        m_playStateSwapped = 0;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        input.SwitchCurrentActionMap("Player");
        m_cinemachine_cam.SetActive(true);
        //Multiplayer_UI_Manager.instance.ExitCanvas();
        m_playState = PlayerState.Unpaused;
        m_HealItemCanvas.SetActive(true);
        m_PlayerUICanvas.SetActive(true);
    }

    public void QuitMulti()
    {
        if (!IsOwner) return;

        if (IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            //DisconnectFromLobbyClientRpc();
        }
        else
        {
            DisconnectFromLobbyServerRpc(OwnerClientId);
        }
    }

    [ServerRpc]
    public void DisconnectFromLobbyServerRpc(ulong _id)
    {
        NetworkManager.DisconnectClient(_id);
        //SceneManager.LoadScene("Level1");
    }
}

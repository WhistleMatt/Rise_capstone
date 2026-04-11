using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//Extended from the code of Nicolas Chatziargiriou
//extended by Matthew Whistle

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

    public NetworkVariable<bool> testBool = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private Network_Sync_Animator syncAnimator;

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

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        GameObject.Find("DebugCamera").SetActive(false);

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
        GetComponent<Use_Item_Multiplayer>().ResetHealsRPC();

        base.OnNetworkSpawn();
    }

    private void Singleton_OnServerStopped(bool obj)
    {
        //SceneManager.LoadScene("Level1");

        //throw new System.NotImplementedException();
    }

    private void OnDestroy()
    {
        if (!IsOwner) return;
        SceneManager.LoadScene("Level1");
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        Debug.Log("connected client ID: " + obj);

        //throw new System.NotImplementedException();
    }

    private void Singleton_OnClientDisconnectCallback(ulong obj)
    {
        if(IsHost) 
        {
            Debug.Log("Player Disconnected");
            return; 
        }

        //SceneManager.LoadScene("Level1");

        //throw new System.NotImplementedException();
    }

    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        if(!IsOwner) return;
        if (context.canceled)
        {
            GetComponent<Network_Sync_Animator>().Animator.SetBool("isWalking", false);
        }
        else if (context.performed)
        {
            GetComponent<Network_Sync_Animator>().Animator.SetBool("isWalking", true);
        }
    }


    public void UnlockMouse(InputAction.CallbackContext context)
    {
        if (!IsOwner)
        {
            Debug.Log(OwnerClientId);
            return;
        }
        if (context.performed)
        {
            var diedUICheck = GameObject.Find("DeathCanvas");
            if (diedUICheck != null)
            {
                return;
            }

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
                m_PlayerUICanvas.SetActive(true);
            }

        }
    }

    public void LockMouse(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        {
            var diedUICheck = GameObject.Find("DeathCanvas");
            if (diedUICheck != null)
            {
                return;
            }
            Cursor.lockState = CursorLockMode.Locked;
            input.SwitchCurrentActionMap("Player");
            UIManager.instance.Unpause();
        }

    }

    public void EnableBossUI()
    {
        if (!IsOwner) return;

        //m_PlayerUICanvas.GetComponentInChildren<WizrdBossHealthBarController>().enabled = true;
    }

    public void TestVal(bool _oldVal, bool _newVal)
    {
        if (!IsOwner) return;
        
        animatorController.SetBool("isWalking", _newVal);
        return;
    }

    private bool performingAction()
    {
        return this.animatorController.GetBool("performingAction");
    }

    void Update()
    {
        if (!IsOwner)
        {
            if (testBool.Value)
            {
                if (!animatorController.GetBool("isWalking"))
                { 
                    this.animatorController.SetBool("isWalking", true); 
                }
            }
            else
            {
                if (animatorController.GetBool("isWalking"))
                {
                    this.animatorController.SetBool("isWalking", false);
                }
            }
                return;
        }

        if (!cam.gameObject.activeInHierarchy)
        {
            cam.gameObject.SetActive(true);
            m_cinemachine_cam.SetActive(true);
            audioListener.enabled = true;
        }

        if (updateStats == true)
        {
            
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
            DisableNetworkAnimator();
            this.animatorController.SetBool("isWalking", true);
            testBool.Value = true;
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocitySmooth, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirectionUpdated = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            

            controller.Move(moveDirectionUpdated.normalized * moveSpeed * Time.deltaTime);
            updateStats = false;
        }
        else
        {

            this.animatorController.SetBool("isWalking", false);
            testBool.Value = false;
            RenableAnimator();
        }

        //apply gravity 
        if (!controller.isGrounded)
        {
            controller.Move(new Vector3(0, gravity, 0) * Time.deltaTime);
        }
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

    public async void QuitMulti()
    {
        if (!IsOwner) return;

        if (IsHost)
        {
            await Multiplayer_lobby_manager.Instance.CloseLobby();
        }
        else
        {
            DisconnectFromLobbyServerRpc(OwnerClientId);
        }
    }

    public void DisableNetworkAnimator()
    {
        if (!IsOwner) return;
        this.GetComponent<Network_Sync_Animator>().Animator = null;
    }

    public void RenableAnimator()
    {
        if (!IsOwner) return;
        this.GetComponent<Network_Sync_Animator>().Animator = animatorController;
    }

    public void EnableNetworkAnimator()
    {
        if (!IsOwner) return;
    }

    [ServerRpc]
    public void DisconnectFromLobbyServerRpc(ulong _id)
    {
        NetworkManager.DisconnectClient(_id);
        //SceneManager.LoadScene("Level1");
    }
}

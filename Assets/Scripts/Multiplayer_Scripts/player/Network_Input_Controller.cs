using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Network_Input_Controller : NetworkBehaviour
{
    public PlayerInPutActions playerControls;
    [SerializeField]
    private int maxInputs;
    [SerializeField]
    private float timer = 0;
    [SerializeField]
    private float clearTime = 0.5f;
    [SerializeField]
    private string prevIn = "Idle";
    [SerializeField]
    private string prevPrevIn = "Idle";

    [SerializeField]
    private int lightCount = 0;
    [SerializeField]
    private int heavyCount = 0;
    [SerializeField]
    private List<string> inputs = new List<string>();

    private bool isBlocking = false;


    private InputAction lAttck;
    private InputAction hAttck;
    private InputAction block;
    private InputAction blockRelease;
    private InputAction parry;
    private InputAction roll;

    [SerializeField] private PlayerInput m_inputs;

    private PlayerStatsController m_statsController;


    public struct InputControllerNetworkInputs : INetworkSerializable
    {

        public string _currentInput;
        public string _previousInput;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _currentInput);
            serializer.SerializeValue(ref _previousInput);
        }

        public InputControllerNetworkInputs(string _current = "Idle", string _previous = "Idle")
        {
            _currentInput = _current;
            _previousInput = _previous;
        }
    }


    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        playerControls = new PlayerInPutActions();
        //m_inputs = GetComponent<PlayerInput>();
        m_statsController = GetComponent<PlayerStatsController>();

        lAttck = playerControls.Player.LightAttack;
        lAttck.Enable();
        lAttck.performed += LightAttack;


        hAttck = playerControls.Player.HeavyAttack;
        hAttck.Enable();
        hAttck.performed += HeavyAttack;

        block = playerControls.Player.Block;
        blockRelease = playerControls.Player.ReleaseBlock;
        block.Enable();
        block.performed += Block;
        blockRelease.Enable();
        blockRelease.performed += BlockRelease;

        parry = playerControls.Player.Parry;
        parry.Enable();
        parry.performed += Parry;

        roll = playerControls.Player.Roll;
        roll.Enable();
        roll.performed += Roll;

        base.OnNetworkSpawn();
    }

    private void LightAttack(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        { 
            addInput("Light", this.OwnerClientId); 
        }
    }
    private void HeavyAttack(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        {
            addInput("Heavy", this.OwnerClientId);
        }

    }

    private void Block(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        {
            addInput("Block", this.OwnerClientId);
            isBlocking = true;
        }

    }
    private void BlockRelease(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        {
            isBlocking = false;
            addInput("BlockRelease", this.OwnerClientId);
        }
    }

    private void Parry(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        {
            addInput("Parry", this.OwnerClientId);
        }
    }

    private void Roll(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        {
            addInput("Roll", this.OwnerClientId);
        }
    }

    private int GetStaminaCost(string _name)
    {
        if (!IsOwner) return 100;
        if (string.Compare(_name, "Light") == 0)
        {
            return 2;
        }
        else if (string.Compare(_name, "Heavy") == 0)
        {
            return 3;
        }
        else if (string.Compare(_name, "Roll") == 0)
        {
            return 2;
        }
        return 0;
    }

    private void executeInput()
    {
        if (!IsOwner) return;
        if (inputs.Count > 0)
        {
            string input = inputs[0];
            inputs.RemoveAt(0);
            int staminacost = GetStaminaCost(input);
            float staminaAmount = m_statsController.getPStamina();
            if (staminacost <= staminaAmount)
            {
                m_statsController.setPStamina(staminaAmount - staminacost);
                timer = 0;

                //InputControllerNetworkInputs inputStruct = new InputControllerNetworkInputs(input, prevIn);

                setAnimState(input, this.OwnerClientId);
            }
        }

    }

    public void RestInputCount()
    {
        if (!IsOwner) return;
        lightCount = 0;
        heavyCount = 0;
    }

    //[ServerRpc]
    private void setAnimState(string input, ulong _playerID)
    {

        //if (OwnerClientId != _playerID) return;

        if (!IsOwner) return;

        Debug.Log("prev input: " + prevIn + "pre prev input: " + prevPrevIn);

        if (input == "Block")
        {
            this.GetComponent<Animator>().SetBool("isBlocking", true);
            return;

        }
        if (input == "BlockRelease")
        {
            this.GetComponent<Animator>().SetBool("isBlocking", false);
            inputs.Clear();
            return;
        }
        if (input == "Parry")
        {
            this.GetComponent<Animator>().SetBool("isParrying", true);
            return;
            ;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (input == "Light" && (prevIn == "Idle" || prevIn == "Walk") && (lightCount == 0))
        {
            this.GetComponent<Animator>().SetTrigger("lightOne");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Light");
            lightCount++;
            return;
        }
        if (input == "Light" && prevIn == "Light" && lightCount == 1)
        {
            this.GetComponent<Animator>().SetTrigger("lightTwo");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Light");
            lightCount++;
            return;

        }
        if (input == "Light" && prevIn == "Light" && lightCount == 2)
        {
            this.GetComponent<Animator>().SetTrigger("lightThree");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Light");
            return;

        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (input == "Heavy" && (prevIn == "Idle" || prevIn == "Walk") && (heavyCount == 0))
        {
            this.GetComponent<Animator>().SetTrigger("heavyOne");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Heavy");
            heavyCount++;
            return;
        }
        if (input == "Heavy" && prevIn == "Heavy" && heavyCount == 1)
        {
            this.GetComponent<Animator>().SetTrigger("heavyTwo");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Heavy");
            heavyCount++;
            return;

        }
        if (input == "Heavy" && prevIn == "Heavy" && heavyCount == 2)
        {
            this.GetComponent<Animator>().SetTrigger("heavyThree");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Heavy");
            return;

        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (input == "Light" && prevIn == "Heavy" && heavyCount == 1 && lightCount == 0)
        {
            this.GetComponent<Animator>().SetTrigger("lightTwo");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Light");
            lightCount++;
            return;

        }
        if (input == "Heavy" && prevIn == "Light" && heavyCount == 1 && lightCount == 1)
        {
            this.GetComponent<Animator>().SetTrigger("heavyFour");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Heavy");
            return;

        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        if (input == "Heavy" && prevIn == "Light" && heavyCount == 0 && lightCount == 2)
        {
            this.GetComponent<Animator>().SetTrigger("heavyFive");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Heavy");
            return;

        }
        /////////////////////////////////////////////////////////////////////////////
        if (input == "Roll")
        {
            this.GetComponent<Animator>().SetTrigger("roll");
            updatePrevInput("Roll");
            inputs.Clear();
            return;
        }
    }
    public void updatePrevInput(string Input)
    {
        if (!IsOwner) return;
        prevPrevIn = prevIn;
        prevIn = Input;
    }

    private void addInput(string input, ulong _playerID)
    {
        if (!IsOwner) return;
        //if (OwnerClientId != _playerID) return;
        if (m_inputs.currentActionMap.name != "Player")
        {
            return;
        }
        if (!isBlocking && inputs.Count < maxInputs)
        {
            inputs.Add(input);

        }
    }


    private void bufferClear()
    {
        if (!IsOwner) return;
        timer = timer + Time.deltaTime;
        if (timer >= clearTime)
        {
            if (inputs.Count > 0)
            {
                inputs.RemoveAt(0);
            }

            timer = 0;
        }
    }

    private void Start()
    {
        m_inputs = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!IsOwner) return;
        if (lightCount > 2 || heavyCount > 2)
        {
            RestInputCount();
        }
        if (inputs.Count > 0 && (prevIn == "Idle" || prevIn == "Walk"))

        {
            executeInput();
            bufferClear();
        }

    }

    [ServerRpc]
    private void UpdateAnimationServerRpc(string _animName, ulong _playerID)
    {

        this.GetComponent<Animator>().SetTrigger("roll");
    }

}

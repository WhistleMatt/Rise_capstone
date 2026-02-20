using System;
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

    [SerializeField] private Animator m_animator;

    private PlayerStatsController m_statsController;

    [SerializeField] public NetworkVariable<InputAnimationStates> m_animStates = new NetworkVariable<InputAnimationStates>(new InputAnimationStates(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private List<ulong> m_animated_Ids = new List<ulong>();

    private float m_animTimer = 0.5f;

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

    [System.Serializable]
    public enum AttackType
    {
        None = 0,
        Light = 1,
        Heavy = 2
    };

    [System.Serializable]
    public struct AttackData : INetworkSerializable
    {
        public AttackType type;
        public bool attack_declared;
        public int light_attack_count;
        public int heavy_attack_count;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref type);
            serializer.SerializeValue(ref attack_declared);
            serializer.SerializeValue(ref light_attack_count);
            serializer.SerializeValue(ref heavy_attack_count);
        }

        public AttackData(AttackType _option = AttackType.None, bool attacked = false, int _lattackCount = 0, int _hattackCount = 0)
        {
            type = _option;
            attack_declared = attacked;
            light_attack_count = _lattackCount;
            heavy_attack_count= _hattackCount;
        }

        public void SetLightData(bool declared = false, int _lcount = 0, int _hcount = 0)
        {
            attack_declared = declared;
            light_attack_count = _lcount;
            heavy_attack_count=_hcount;
        }
    }


    [System.Serializable]
    public struct InputAnimationStates : INetworkSerializable
    {
        public AttackData attackData;
        public bool blocking;
        public bool rolling;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref attackData);
            serializer.SerializeValue(ref blocking);
            serializer.SerializeValue(ref rolling);
        }

        public InputAnimationStates(AttackData atkData = new AttackData(), bool block = false, bool roll = false)
        {
            attackData = atkData;
            blocking = block;
            rolling = roll;
        }

        public void SetStateData(AttackData _data = new AttackData(), bool _blocking = false, bool _rolling = false)
        {
            attackData = _data;
            blocking = _blocking;
            rolling = _rolling;
        }
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;

        m_animStates.OnValueChanged += StateChange;

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

    private void StateChange(InputAnimationStates previousValue, InputAnimationStates newValue)
    {
        if (!IsOwner)
        {
            
            return;
        }

        //throw new NotImplementedException();
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

        if (!IsOwner)
        {
            return;
        }

        this.GetComponent<Animator>().SetBool("performingAction", true);

        Debug.Log("prev input: " + prevIn + "pre prev input: " + prevPrevIn);

        if (input == "Block")
        {
            var statDat = m_animStates.Value;
            statDat.attackData = new AttackData(AttackType.None, false, 0, 0);
            statDat.blocking = true;
            statDat.rolling = false;
            m_animStates.Value = statDat;
            this.GetComponent<Animator>().SetBool("isBlocking", true);
            return;

        }
        if (input == "BlockRelease")
        {
            var statDat = m_animStates.Value;
            statDat.attackData = new AttackData(AttackType.None, false, 0, 0);
            statDat.blocking = false;
            statDat.rolling = false;
            m_animStates.Value = statDat;
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
            var statDat = m_animStates.Value;
            statDat.attackData = new AttackData(AttackType.Light, false, 1, 0);
            m_animStates.Value = statDat;
            lightCount++;
            return;
        }
        if (input == "Light" && prevIn == "Light" && lightCount == 1)
        {
            this.GetComponent<Animator>().SetTrigger("lightTwo");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Light");
            var statDat = m_animStates.Value;
            statDat.attackData = new AttackData(AttackType.Light, false, 2, 0);
            m_animStates.Value = statDat;
            lightCount++;
            return;

        }
        if (input == "Light" && prevIn == "Light" && lightCount == 2)
        {
            this.GetComponent<Animator>().SetTrigger("lightThree");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Light");
            var statDat = m_animStates.Value;
            statDat.attackData = new AttackData(AttackType.Light, true, 3, 0);
            m_animStates.Value = statDat;
            return;

        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (input == "Heavy" && (prevIn == "Idle" || prevIn == "Walk") && (heavyCount == 0))
        {
            this.GetComponent<Animator>().SetTrigger("heavyOne");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Heavy");
            var statDat = m_animStates.Value;
            statDat.attackData = new AttackData(AttackType.Heavy, true, 0, 1);
            m_animStates.Value = statDat;
            heavyCount++;
            return;
        }
        if (input == "Heavy" && prevIn == "Heavy" && heavyCount == 1)
        {
            this.GetComponent<Animator>().SetTrigger("heavyTwo");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Heavy");
            var statDat = m_animStates.Value;
            statDat.attackData = new AttackData(AttackType.Heavy, true, 0, 2);
            m_animStates.Value = statDat;
            heavyCount++;
            return;

        }
        if (input == "Heavy" && prevIn == "Heavy" && heavyCount == 2)
        {
            this.GetComponent<Animator>().SetTrigger("heavyThree");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Heavy");
            var statDat = m_animStates.Value;
            statDat.attackData = new AttackData(AttackType.Heavy, true, 0, 3);
            m_animStates.Value = statDat;
            return;

        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (input == "Light" && prevIn == "Heavy" && heavyCount == 1 && lightCount == 0)
        {
            this.GetComponent<Animator>().SetTrigger("lightTwo");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Light");
            var statDat = m_animStates.Value;
            statDat.attackData = new AttackData(AttackType.Light, true, 1, 1);
            m_animStates.Value = statDat;
            lightCount++;
            return;

        }
        if (input == "Heavy" && prevIn == "Light" && heavyCount == 1 && lightCount == 1)
        {
            this.GetComponent<Animator>().SetTrigger("heavyFour");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Heavy");
            var statDat = m_animStates.Value;
            statDat.attackData = new AttackData(AttackType.Heavy, true, 1, 1);
            m_animStates.Value = statDat;
            return;

        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        if (input == "Heavy" && prevIn == "Light" && heavyCount == 0 && lightCount == 2)
        {
            this.GetComponent<Animator>().SetTrigger("heavyFive");
            GetComponentInChildren<HitBoxController>().HitBoxEnable(0);
            updatePrevInput("Heavy");
            var statDat = m_animStates.Value;
            statDat.attackData = new AttackData(AttackType.Heavy, true, 2, 1);
            m_animStates.Value = statDat;
            return;

        }
        /////////////////////////////////////////////////////////////////////////////
        if (input == "Roll")
        {
            this.GetComponent<Animator>().SetTrigger("roll");
            updatePrevInput("Roll");
            var statDat = m_animStates.Value;
            statDat.attackData = new AttackData(AttackType.None, false, 0, 0);
            statDat.blocking = false;
            statDat.rolling = true;
            m_animStates.Value = statDat;
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
            var stateReset = m_animStates.Value;
            stateReset.SetStateData();
            m_animStates.Value = stateReset;

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

    //private void LateUpdate()
    //{
    //    var query = GameObject.FindObjectsByType<Network_Input_Controller>(FindObjectsSortMode.InstanceID);
    //    foreach (var obj in query) 
    //    {
    //        if(obj.IsOwner)
    //        {
    //            continue;
    //        }
    //        else
    //        {
    //            if (m_animated_Ids.Contains(obj.OwnerClientId))
    //            {
    //                m_animated_Ids.Remove(obj.OwnerClientId);
    //                return;
    //            }
                
    //            m_animated_Ids.Add(obj.OwnerClientId);
    //            var newValue = obj.m_animStates.Value;
    //            switch (newValue.attackData.type)
    //            {
    //                case AttackType.None:
    //                    {
    //                        if (newValue.rolling)
    //                        {
    //                            obj.m_animator.SetTrigger("roll");
    //                        }
    //                        else if (newValue.blocking)
    //                        {
    //                            obj.GetComponent<Animator>().SetBool("isBlocking", true);
    //                        }
    //                        else
    //                        {
    //                            obj.GetComponent<Animator>().SetBool("isBlocking", false);
    //                        }
    //                        break;
    //                    }
    //                case AttackType.Light:
    //                    {
    //                        if (newValue.attackData.light_attack_count == 1)
    //                        {
    //                            obj.GetComponent<Animator>().SetTrigger("lightOne");
    //                        }
    //                        else if (newValue.attackData.light_attack_count == 2 || (newValue.attackData.light_attack_count == 1 && newValue.attackData.heavy_attack_count == 1))
    //                        {
    //                            obj.GetComponent<Animator>().SetTrigger("lightTwo");
    //                        }
    //                        else if (newValue.attackData.light_attack_count == 3)
    //                        {
    //                            obj.GetComponent<Animator>().SetTrigger("lightThree");
    //                        }
    //                        break;
    //                    }
    //                case AttackType.Heavy:
    //                    {
    //                        if (newValue.attackData.heavy_attack_count == 1)
    //                        {
    //                            obj.GetComponent<Animator>().SetTrigger("heavyOne");
    //                        }
    //                        else if (newValue.attackData.heavy_attack_count == 2)
    //                        {
    //                            obj.GetComponent<Animator>().SetTrigger("heavyTwo");
    //                        }
    //                        else if (newValue.attackData.heavy_attack_count == 3)
    //                        {
    //                            obj.GetComponent<Animator>().SetTrigger("heavyThree");
    //                        }
    //                        else if (newValue.attackData.heavy_attack_count == 1 && newValue.attackData.light_attack_count == 1)
    //                        {
    //                            obj.GetComponent<Animator>().SetTrigger("heavyFour");
    //                        }
    //                        else if (newValue.attackData.heavy_attack_count == 1 && newValue.attackData.light_attack_count == 2)
    //                        {
    //                            obj.GetComponent<Animator>().SetTrigger("heavyFive");
    //                        }
    //                        break;
    //                    }
    //            }
    //        }
    //    }
    //}

    public void ResetNetworkVar()
    {
        if (!IsOwner) return;
        var stateReset = m_animStates.Value;
        stateReset.SetStateData();
        m_animStates.Value = stateReset;
    }

    [ServerRpc]
    private void UpdateAnimationServerRpc(string _animName, ulong _playerID)
    {

        this.GetComponent<Animator>().SetTrigger("roll");
    }

}

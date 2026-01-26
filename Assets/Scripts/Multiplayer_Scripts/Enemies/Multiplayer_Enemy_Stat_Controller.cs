using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

//Extended from the code of Nicolas Chatziargiriou
//extended by Matthew Whistle

public class Multiplayer_Enemy_Stat_Controller : NetworkBehaviour
{
    
    [SerializeField] NetworkVariable<float> _pHealth = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pHealthMax = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pMana = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pManaMax = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pStamina = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pStaminaMax = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pAttck = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pAttckMax = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pDefense = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pDefenseMax = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private float _experiancePoints;
    [SerializeField] private GameObject _pushBox;
    [SerializeField] private GameObject _YouDied;
    [SerializeField] private GameObject _Singleton;
    [SerializeField] private AudioClip _deadSplat;
    [SerializeField] private AudioSource _damageSoundSource;

    public int _mouseX;
    public int _mouseY;
    private bool m_isDead = false;
    private bool gaveXP = false;
    private float _respawnTimer = 0;
    private float _respawnTime = 9.0f;
    private bool showDeathTut = true;
    private void Awake()
    {
        // _Singleton.gameObject.GetComponent<PlayFabStats>().GetStatistics();
    }

    public bool isDead()
    {
        return m_isDead;
    }
    public float getExperiancePoints()
    {
        return _experiancePoints;
    }

    public void setExperiancePoints(float xp)
    {
        _experiancePoints = xp;
    }
    public float getPHealth()
    {
        return _pHealth.Value;
    }

    public void setPHealth(float health)
    {
        TakeDamageServerRpc(health);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void TakeDamageServerRpc(float health)
    {
        if (health > _pHealthMax.Value)
        {
            _pHealth.Value = _pHealthMax.Value;
            return;
        }
        _pHealth.Value = health;
    }

    public float getPHealthMax()
    {
        return _pHealthMax.Value;
    }

    public void setPHealthMax(float health)
    {
        _pHealthMax.Value = health;
    }

    public bool IsDead() { return m_isDead; }

    public void SetDead(bool _isDead) { m_isDead = _isDead; }

    public float getPMana()
    {
        return _pMana.Value;
    }

    public void setPMana(float mana)
    {
        _pMana.Value = mana;
    }

    public float getManaMax()
    {
        return _pManaMax.Value;
    }

    public void setPManaMax(float mana)
    {
        _pManaMax.Value = mana;
    }

    public float getPStamina()
    {
        return _pStamina.Value;
    }

    public void setPStamina(float Stamina)
    {
        _pStamina.Value = Stamina;
    }

    public float getStaminahMax()
    {
        return _pStaminaMax.Value;
    }

    public void setPStaminaMax(float stamina)
    {
        _pStaminaMax.Value = stamina;
    }


    public float getPAttck()
    {
        return _pAttck.Value;
    }

    public void setPAttck(float attack)
    {
        _pAttck.Value = attack;
    }

    public float getAttckMax()
    {
        return _pAttckMax.Value;
    }

    public void setPAttckMax(float attack)
    {
        _pAttckMax.Value = attack;
    }

    public float getPDefense()
    {
        return _pDefense.Value;
    }

    public void setPDefense(float defense)
    {
        _pDefense.Value = defense;
    }

    public float getPDefenseMax()
    {
        return _pDefenseMax.Value;
    }

    public void setPDefenseMax(float defense)
    {
        _pDefenseMax.Value = defense;
    }

    private void Start()
    {
        //    GameObject.FindGameObjectWithTag("xSlider").GetComponent<Slider>().value = _mouseX;
        //    GameObject.FindGameObjectWithTag("ySlider").GetComponent<Slider>().value = _mouseY;



    }
    private void Update()
    {

        if (getPHealth() <= 0)
        {
            _respawnTimer = _respawnTimer + Time.deltaTime;
            if (this.gameObject.tag != "ammo")
            {
                this.gameObject.GetComponent<Animator>().SetTrigger("isDead");
                _pushBox.gameObject.SetActive(false);

                m_isDead = true;

                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

            }

            if (this.gameObject.tag == "Enemy" && !gaveXP)
            {
                GameObject.FindWithTag("Player").GetComponent<PlayerStatsController>().setExperiancePoints(GameObject.FindWithTag("Player").GetComponent<PlayerStatsController>().getExperiancePoints() + 2);
                gaveXP = true;
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<TargetSelector>().disableTargeting();
                this.gameObject.transform.root.tag = "DeadEnemy";
                this.gameObject.GetComponent<AudioSource>().clip = _deadSplat;
                this.gameObject.GetComponent<AudioSource>().loop = false;
                this.gameObject.GetComponent<AudioSource>().volume = 20;
                this.gameObject.GetComponent<AudioSource>().Play();
                GameObject.FindWithTag("Player").GetComponentInChildren<TargetList>().removeTarget(this.gameObject);
            }

        }
        if (getPStamina() < getStaminahMax())
        {
            setPStamina(getPStamina() + 1.5f * Time.deltaTime);
        }
    }
}

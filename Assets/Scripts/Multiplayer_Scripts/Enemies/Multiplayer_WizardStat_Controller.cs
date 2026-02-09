using Unity.Netcode;
using UnityEngine;

public class Multiplayer_WizardStat_Controller : MonoBehaviour
{
    [SerializeField] NetworkVariable<float> _pHealth = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pHealthMax = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pAttck = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pAttckMax = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pDefense = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<float> _pDefenseMax = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private float _experiancePoints;
    [SerializeField] private GameObject _pushBox;
    [SerializeField] private GameObject _knightStatue;
    [SerializeField] private GameObject _hpBar;

    private bool m_isDead = false;
    private bool gaveXP = false;

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
    private void Awake()
    {


    }

    private void Update()
    {

        if (getPHealth() <= 0)
        {
            //  this.gameObject.GetComponent<Animator>().SetTrigger("isDead");

            _pushBox.gameObject.SetActive(false);
            _knightStatue.SetActive(true);
            m_isDead = true;
            this.gameObject.SetActive(false);
            _hpBar.SetActive(false);
            GameObject.FindGameObjectWithTag("SoulGirl").GetComponent<SoulsGirlDialogue>().chapter = 31;
            GameObject.FindGameObjectWithTag("SoulGirl").GetComponent<DialogueController>().recieveDialogue();


            if (this.gameObject.tag == "Enemy" && !gaveXP)
            {
                GameObject.FindWithTag("Player").GetComponent<PlayerStatsController>().setExperiancePoints(GameObject.FindWithTag("Player").GetComponent<PlayerStatsController>().getExperiancePoints() + 2);
                gaveXP = true;
            }
        }

    }
}

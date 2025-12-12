using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizrdBossStatsController : MonoBehaviour
{


    [SerializeField] private float _pHealth;
    [SerializeField] private float _pHealthMax;
    [SerializeField] private float _pAttck;
    [SerializeField] private float _pAttckMax;
    [SerializeField] private float _pDefense;
    [SerializeField] private float _pDefenseMax;
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
        return _pHealth;
    }

    public void setPHealth(float health)
    {
        if (health > _pHealthMax)
        {
            _pHealth = _pHealthMax;
            return;
        }
        _pHealth = health;

    }

    public float getPHealthMax()
    {
        Debug.Log("UPDATE");
        return _pHealthMax;
    }

    public void setPHealthMax(float health)
    {
        _pHealthMax = health;
    }

    public bool IsDead() { return m_isDead; }

    public void SetDead(bool _isDead) { m_isDead = _isDead; }

   



    public float getPAttck()
    {
        return _pAttck;
    }

    public void setPAttck(float attack)
    {
        _pAttck = attack;
    }

    public float getAttckMax()
    {
        return _pAttckMax;
    }

    public void setPAttckMax(float attack)
    {
        _pAttckMax = attack;
    }

    public float getPDefense()
    {
        return _pDefense;
    }

    public void setPDefense(float defense)
    {
        _pDefense = defense;
    }

    public float getPDefenseMax()
    {
        return _pDefenseMax;
    }

    public void setPDefenseMax(float defense)
    {
        _pDefenseMax = defense;
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

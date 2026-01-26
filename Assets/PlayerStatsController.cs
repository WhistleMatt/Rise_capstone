using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//written by Nicolas Chatziargiriou
//extended by Matthew Whistle
public class PlayerStatsController : MonoBehaviour
{
 

    [SerializeField] private float _pHealth;
    [SerializeField] private float _pHealthMax;
    [SerializeField] private float _pMana;
    [SerializeField] private float _pManaMax;
    [SerializeField] private float _pStamina;
    [SerializeField] private float _pStaminaMax;
    [SerializeField] private float _pAttck;
    [SerializeField] private float _pAttckMax;
    [SerializeField] private float _pDefense;
    [SerializeField] private float _pDefenseMax;
    [SerializeField] private float _experiancePoints;
    [SerializeField] private GameObject _pushBox;
    [SerializeField] private GameObject _YouDied;
    [SerializeField] private GameObject _Singleton;
    [SerializeField] private AudioClip _deadSplat;
    [SerializeField] private AudioSource _damageSoundSource;
 
    public  int _mouseX;
    public int _mouseY;
    private bool m_isDead = false;
    private bool gaveXP = false;
    private float _respawnTimer=0;
    private float _respawnTime=9.0f;
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
        return _pHealth;
    }

    public void setPHealth(float health)
    {
        if(health > _pHealthMax)
        {
            _pHealth = _pHealthMax;
            return;
        }
        _pHealth = health;
        
    }
    
    public float getPHealthMax()
    {
        return _pHealthMax;
    }

    public void setPHealthMax(float health)
    {
        _pHealthMax = health;
    }

    public bool IsDead() { return m_isDead; }

    public void SetDead(bool _isDead) { m_isDead= _isDead; }

    public float getPMana()
    {
        return _pMana;
    }

    public void setPMana(float mana)
    {
        _pMana = mana;
    }

    public float getManaMax()
    {
        return _pManaMax;
    }

    public void setPManaMax(float mana)
    {
        _pManaMax = mana;
    }

    public float getPStamina()
    {
        return _pStamina;
    }

    public void setPStamina(float Stamina)
    {
        _pStamina = Stamina;
    }

    public float getStaminahMax()
    {
        return _pStaminaMax;
    }

    public void setPStaminaMax(float stamina)
    {
        _pStaminaMax = stamina;
    }


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

    private void Start()
    {
        //    GameObject.FindGameObjectWithTag("xSlider").GetComponent<Slider>().value = _mouseX;
        //    GameObject.FindGameObjectWithTag("ySlider").GetComponent<Slider>().value = _mouseY;
      


    }
    private void Update()
    {
       
        if(getPHealth()<=0)
        {
            _respawnTimer = _respawnTimer + Time.deltaTime;
            if (this.gameObject.tag != "ammo")
            {
                this.gameObject.GetComponent<Animator>().SetTrigger("isDead");
                _pushBox.gameObject.SetActive(false);
             
                m_isDead = true;
             


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


                
                //show you died screen
                if (this.gameObject.transform.root.tag=="Player")
                {
                _damageSoundSource.enabled = false;
                if (_YouDied.activeSelf==false)
                GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<Canvas>().enabled = false;
                    _YouDied.SetActive(true);
                this.gameObject.GetComponent<PlayerStatsController>().setExperiancePoints(0);
                this.gameObject.GetComponentInChildren<AudioSource>().volume = 0;
                PlayFabStats.Instance.UpdateEXP(0);
                    if (_respawnTimer >= _respawnTime && GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().levelTutorial==1)
                    {
                        _YouDied.SetActive(false);
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }
                    else if (_respawnTimer >= _respawnTime && GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().levelTutorial == 0)
                    {
                 
                    if (showDeathTut)
                    {
                        GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<Canvas>().enabled = true;
                        GameObject.FindGameObjectWithTag("SoulGirl").GetComponent<SoulsGirlDialogue>().chapter = 26;
                        GameObject.FindGameObjectWithTag("SoulGirl").GetComponent<DialogueController>().recieveDialogue();
                        showDeathTut = false;
                    }
                   
                    _respawnTimer = 0;

                    }

                }

            

        }
        if (getPStamina() < getStaminahMax())
        {
            setPStamina(getPStamina() + 1.5f*Time.deltaTime);
        }
    }
}

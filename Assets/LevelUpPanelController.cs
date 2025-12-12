using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class LevelUpPanelController : MonoBehaviour
{
    [SerializeField] private GameObject levelPanelBackground;
    [SerializeField] private TextMeshProUGUI _currentHPBox;
    [SerializeField] private TextMeshProUGUI _currentManaBox;
    [SerializeField] private TextMeshProUGUI _currentStaminaBox;
    [SerializeField] private TextMeshProUGUI _currentAttackBox;
    [SerializeField] private TextMeshProUGUI _currentDefenceBox;
    [SerializeField] private TextMeshProUGUI _currentXPBox;

    [SerializeField] private TextMeshProUGUI _newHPBox;
    [SerializeField] private TextMeshProUGUI _newManaBox;
    [SerializeField] private TextMeshProUGUI _newStaminaBox;
    [SerializeField] private TextMeshProUGUI _newAttackBox;
    [SerializeField] private TextMeshProUGUI _newDefenceBox;
    [SerializeField] private TextMeshProUGUI _newXPBox;

    DialogueController _dialogueController;


    private float _currentHP;
    private float _newHP;
    private float _currentMana;
    private float _newMana;
    private float _currentStamina;
    private float _newStamina;
    private float _currentAttack;
    private float _newAttack;
    private float _currentDefence;
    private float _newDefence;
    [SerializeField] private float _currentXP;
    [SerializeField] private float _newXP;

    private PlayerInput inputMap;

    private GameObject player;
    private void Awake()
    {

      //  GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().run = true;
        //inputMap = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        //  inputMap.SwitchCurrentActionMap("Menus");
        Cursor.lockState = CursorLockMode.Confined;

        player = GameObject.FindWithTag("Player");
        updateBox();
        // _dialogueController = GameObject.FindWithTag("SoulGirl").GetComponent<DialogueController>();
        //Cursor.visible = false;
        levelPanelBackground.SetActive(true);

        
    }
    
    public void updateBox()
    {
       


        _currentHPBox.text = player.GetComponent<PlayerStatsController>().getPHealthMax().ToString();
        _currentManaBox.text = player.GetComponent<PlayerStatsController>().getManaMax().ToString();
        _currentStaminaBox.text = player.GetComponent<PlayerStatsController>().getStaminahMax().ToString();
        _currentAttackBox.text = player.GetComponent<PlayerStatsController>().getAttckMax().ToString();
        _currentDefenceBox.text = player.GetComponent<PlayerStatsController>().getPDefenseMax().ToString();
        _currentXPBox.text = player.GetComponent<PlayerStatsController>().getExperiancePoints().ToString();

        _newHPBox.text = player.GetComponent<PlayerStatsController>().getPHealthMax().ToString();
        _newManaBox.text = player.GetComponent<PlayerStatsController>().getManaMax().ToString();
        _newStaminaBox.text = player.GetComponent<PlayerStatsController>().getStaminahMax().ToString();
        _newAttackBox.text = player.GetComponent<PlayerStatsController>().getAttckMax().ToString();
        _newDefenceBox.text = player.GetComponent<PlayerStatsController>().getPDefenseMax().ToString();
        _newXPBox.text = player.GetComponent<PlayerStatsController>().getExperiancePoints().ToString();

        _currentHP = player.GetComponent<PlayerStatsController>().getPHealthMax();
        _currentMana = player.GetComponent<PlayerStatsController>().getManaMax();
        _currentStamina = player.GetComponent<PlayerStatsController>().getStaminahMax();
        _currentAttack = player.GetComponent<PlayerStatsController>().getAttckMax();
        _currentDefence = player.GetComponent<PlayerStatsController>().getPDefenseMax();
        _currentXP = player.GetComponent<PlayerStatsController>().getExperiancePoints();
        
        _newHP = player.GetComponent<PlayerStatsController>().getPHealthMax();
        _newMana = player.GetComponent<PlayerStatsController>().getManaMax();
        _newStamina = player.GetComponent<PlayerStatsController>().getStaminahMax();
        _newAttack = player.GetComponent<PlayerStatsController>().getAttckMax();
        _newDefence = player.GetComponent<PlayerStatsController>().getPDefenseMax();
        _newXP = player.GetComponent<PlayerStatsController>().getExperiancePoints();


    }
    private void Update()
    {
        
        _currentXP = player.GetComponent<PlayerStatsController>().getExperiancePoints();
        _currentXPBox.text = player.GetComponent<PlayerStatsController>().getExperiancePoints().ToString();
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void increaseHP()
    {
        if (_newXP>0)
        {
            _newHP = _newHP + 1;
            _newHPBox.text = _newHP.ToString();
            _newXP = _newXP - 1;
            _newXPBox.text = _newXP.ToString();

        }
       

        if (_newHP!=_currentHP)
        {
            _newHPBox.color = Color.red;
        }
    }
    public void decreaseHP()
    {
        if (_newHP > _currentHP)
        {
            _newHP = _newHP - 1;
            _newHPBox.text = _newHP.ToString();
            _newXP++;
            _newXPBox.text = _newXP.ToString();
        }
       
        if (_newHP == _currentHP)
        {
            _newHPBox.color = Color.white;
        }
    }


    public void increaseMana()
    {
        if (_newXP > 0)
        {
            _newMana = _newMana + 1;
            _newManaBox.text = _newMana.ToString();
            _newXP = _newXP - 1;
            _newXPBox.text = _newXP.ToString();

        }


        if (_newMana != _currentMana)
        {
            _newManaBox.color = Color.red;
        }
    }
    public void decreaseMana()
    {
        if (_newMana > _currentMana)
        {
            _newMana = _newMana - 1;
            _newManaBox.text = _newMana.ToString();
            _newXP++;
            _newXPBox.text = _newXP.ToString();
        }

        if (_newMana == _currentMana)
        {
            _newManaBox.color = Color.white;
        }
    }

    public void increaseStamina()
    {
        if (_newXP > 0)
        {
            _newStamina = _newStamina + 1;
            _newStaminaBox.text = _newStamina.ToString();
            _newXP = _newXP - 1;
            _newXPBox.text = _newXP.ToString();

        }


        if (_newStamina != _currentStamina)
        {
            _newStaminaBox.color = Color.red;
        }
    }
    public void decreaseStamina()
    {
        if (_newStamina > _currentStamina)
        {
            _newStamina = _newStamina - 1;
            _newStaminaBox.text = _newStamina.ToString();
            _newXP++;
            _newXPBox.text = _newXP.ToString();
        }

        if (_newStamina == _currentStamina)
        {
            _newStaminaBox.color = Color.white;
        }
    }

    public void increaseAttack()
    {
        if (_newXP > 0)
        {
            _newAttack = _newAttack + 1;
            _newAttackBox.text = _newAttack.ToString();
            _newXP = _newXP - 1;
            _newXPBox.text = _newXP.ToString();

        }


        if (_newAttack != _currentAttack)
        {
            _newAttackBox.color = Color.red;
        }
    }
    public void decreaseAttack()
    {
        if (_newAttack > _currentAttack)
        {
            _newAttack = _newAttack - 1;
            _newAttackBox.text = _newAttack.ToString();
            _newXP++;
            _newXPBox.text = _newXP.ToString();
        }

        if (_newAttack == _currentAttack)
        {
            _newAttackBox.color = Color.white;
        }
    }

    public void increaseDefence()
    {
        if (_newXP > 0)
        {
            _newDefence = _newDefence + 1;
            _newDefenceBox.text = _newDefence.ToString();
            _newXP = _newXP - 1;
            _newXPBox.text = _newXP.ToString();

        }


        if (_newDefence != _currentDefence)
        {
            _newDefenceBox.color = Color.red;
        }
    }
    public void decreaseDefence()
    {
        if (_newDefence > _currentDefence)
        {
            _newDefence = _newDefence - 1;
            _newDefenceBox.text = _newDefence.ToString();
            _newXP++;
            _newXPBox.text = _newXP.ToString();
        }

        if (_newDefence == _currentDefence)
        {
            _newDefenceBox.color = Color.white;
        }
    }

    public void submit()
    {

        player.GetComponent<PlayerStatsController>().setPHealthMax(_newHP);
        player.GetComponent<PlayerStatsController>().setPHealth(_newHP);

        player.GetComponent<PlayerStatsController>().setPManaMax(_newMana);
        player.GetComponent<PlayerStatsController>().setPMana(_newMana);

        player.GetComponent<PlayerStatsController>().setPStaminaMax(_newStamina);
        player.GetComponent<PlayerStatsController>().setPStamina(_newStamina);

        player.GetComponent<PlayerStatsController>().setPAttckMax(_newAttack);
        player.GetComponent<PlayerStatsController>().setPAttck(_newAttack);

        player.GetComponent<PlayerStatsController>().setPDefenseMax(_newDefence);
        player.GetComponent<PlayerStatsController>().setPDefense(_newDefence);

        player.GetComponent<PlayerStatsController>().setExperiancePoints(_newXP);
       //PlayFabStats.Instance.UpdateStats((int)_newHP, (int)_newHP, (int)player.transform.position.x, (int)player.transform.position.y, (int)player.transform.position.z, 1, (int)_newMana, (int)_newMana, (int)_newStamina, (int)_newStamina, (int)_newAttack, (int)_newAttack, (int)_newDefence, (int)_newDefence, (int)_newXP, SceneManager.GetActiveScene().buildIndex);
        
        GameObject.FindWithTag("SoulGirl").GetComponent<DialogueController>().finishPage();
        GameObject.FindWithTag("SoulGirl").GetComponent<SoulsGirlDialogue>().setChapter(2);
        GameObject.FindWithTag("SoulGirl").GetComponent<DialogueController>().recieveDialogue();
        GameObject.FindWithTag("SoulGirl").GetComponent<DialogueController>().startPage();

        Cursor.lockState = CursorLockMode.Locked;
        //inputMap.SwitchCurrentActionMap("DialogueBox");
        //  _dialogueController.RenableNext();
        GameObject.FindGameObjectWithTag("SoulGirl").GetComponent<DialogueController>().toggleLock(false);
        levelPanelBackground.SetActive(false);
        this.gameObject.SetActive(false);

      
    }
    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.SetActive(false);
        levelPanelBackground.SetActive(true);

    }
    

}

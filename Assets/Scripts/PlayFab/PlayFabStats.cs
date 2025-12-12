using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI ;
public class PlayFabStats : MonoBehaviour
{
    



    public int PlayerMaxHealth;
    public int PlayerHealth;
    public int PlayerPositionX;
    public int PlayerPositionY;
    public int PlayerPositionZ;
    public int JustStarted=0;
    public int Level;
   // public int MaxMP;
    //public int CurrentMP;
    public int MaxStamina;
    public int CurrentStamina;
    public int MaxAttack;
    public int CurrentAttack;
    public int MaxDefense;
    public int CurrentDefense;
    public int EXP;
    public float XVal;
    public float YVal;
    public int hasSeenIntro;
    public int mouseX;
    public int mouseY;
    public int tutorialComplete;
    public int levelTutorial;
    public int mouseInvert;

    private bool statsSent = false;
   /* public int tutorial
    {
        get
        {
            return _tutorial;
        }
        set
        { _tutorial = value; }
    }*/
//    private int _tutorial;
    private int timer = 0;

    public bool run;
    public static PlayFabStats Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        

        if (JustStarted==0)
        {
           
        }

        GetStatistics();

        run = true;

    }

    public void getMouseSettings()
    {
        if (GameObject.FindGameObjectWithTag("xSlider")!=null)
        {
            GameObject _xSetting = GameObject.FindGameObjectWithTag("xSlider");
            GameObject _ySetting = GameObject.FindGameObjectWithTag("ySlider");
            _xSetting.GetComponent<Slider>().value = mouseX;
            _ySetting.GetComponent<Slider>().value = mouseY;
        }
      
    }
 
     public int getPlayerStats()
    {
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Getting Player Stats: ");

        GameObject _player = GameObject.FindGameObjectWithTag("Player");
            if (JustStarted == 1)
            {
                _player.GetComponent<PlayerStatsController>().setPHealthMax(PlayerMaxHealth);
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Getting Player Stat Player Max Health: "+ PlayerMaxHealth);
            _player.GetComponent<PlayerStatsController>().setPHealth(PlayerHealth);
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Getting Player Stat Player Health: "+ PlayerHealth);
            //_player.GetComponent<PlayerStatsController>().setPManaMax(MaxMP);
        //    GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Getting Player Stat Player Max MP: "+ MaxMP);
          //  _player.GetComponent<PlayerStatsController>().setPMana(CurrentMP);
          //  GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Getting Player Stat Player Current MP: "+ CurrentMP);
            _player.GetComponent<PlayerStatsController>().setPStaminaMax(MaxStamina);
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Getting Player Stat Player Max Stamina: "+ MaxStamina);
            _player.GetComponent<PlayerStatsController>().setPStamina(CurrentStamina);
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Getting Player Stat Player Current Stamina: "+ CurrentStamina);
            _player.GetComponent<PlayerStatsController>().setPAttckMax(MaxAttack);
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Getting Player Stat Player Max Attack: "+ MaxAttack);
            _player.GetComponent<PlayerStatsController>().setPAttck(CurrentAttack);
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Getting Player Stat Player Current Attack: "+CurrentAttack);
            _player.GetComponent<PlayerStatsController>().setPDefenseMax(MaxDefense);
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Getting Player Stat Player MaxDefense: "+ MaxDefense);
            _player.GetComponent<PlayerStatsController>().setPDefense(CurrentDefense);
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Getting Player Stat Player Current  Defense: "+CurrentDefense);
            _player.GetComponent<PlayerStatsController>().setExperiancePoints(EXP);
            _player.GetComponent<PlayerStatsController>()._mouseX = mouseX;
            _player.GetComponent<PlayerStatsController>()._mouseY = mouseY;

         //   _player.GetComponent<PlayerStatsController>()._sawTutorial = tutorial;
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Getting Player Stat Player EXP: "+ EXP);
        //    Debug.Log("TAKE MY STATS : " + EXP);
            run = false;

               

            return 1;

        }
        else
        {
            return 0;
        }
    
        
    }
    private void Start()
    {


  









        //Level = SceneManager.GetActiveScene().buildIndex;
    }

    //saving the information to the player account
    /*
    public void setMouseSettings()
    {
        var req = new UpdatePlayerStatisticsRequest();
        req.Statistics = new List<StatisticUpdate>
        {

                new StatisticUpdate {StatisticName = "mouseX", Value = mouseX},
                new StatisticUpdate {StatisticName = "mouseY", Value = mouseY}

            };
        PlayFabClientAPI.UpdatePlayerStatistics(req, SetStatisticSuccess, SetStatsFailed);
        Debug.Log("SETTINGS UPDATE");
    }*/
    /*public void setSeenIntro()
    {
        var req = new UpdatePlayerStatisticsRequest();
        req.Statistics = new List<StatisticUpdate>
        {
            
                new StatisticUpdate {StatisticName = "SeenIntro", Value = hasSeenIntro},
                
            };
        PlayFabClientAPI.UpdatePlayerStatistics(req, SetStatisticSuccess, SetStatsFailed);
    }*/
    public void SetStatistics()
    {
        statsSent = false;
        var req = new UpdatePlayerStatisticsRequest();
            req.Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate {StatisticName = "PlayerMaxHealth", Value = PlayerMaxHealth},
                new StatisticUpdate {StatisticName = "PlayerHealth", Value = PlayerHealth},
                new StatisticUpdate {StatisticName = "PlayerPositionX", Value = PlayerPositionX},
                new StatisticUpdate {StatisticName = "PlayerPositionY", Value = PlayerPositionY},
                new StatisticUpdate {StatisticName = "PlayerPositionZ", Value = PlayerPositionZ},
                new StatisticUpdate {StatisticName = "JustStarted", Value = JustStarted},
                new StatisticUpdate {StatisticName = "Level", Value = Level},
                //new StatisticUpdate {StatisticName = "MaxMP", Value = MaxMP},
              //  new StatisticUpdate {StatisticName = "CurrentMP", Value = CurrentMP},
                new StatisticUpdate {StatisticName = "MaxStamina", Value = MaxStamina},
                new StatisticUpdate {StatisticName = "CurrentStamina", Value = CurrentStamina},
                new StatisticUpdate {StatisticName = "MaxAttack", Value = MaxAttack},
                new StatisticUpdate {StatisticName = "CurrentAttack", Value = CurrentAttack},
                new StatisticUpdate {StatisticName = "MaxDefense", Value = MaxDefense},
                new StatisticUpdate {StatisticName = "CurrentDefense", Value = CurrentDefense},
                new StatisticUpdate {StatisticName = "EXP", Value = EXP},
                new StatisticUpdate {StatisticName = "SeenIntro", Value = hasSeenIntro},
                new StatisticUpdate {StatisticName = "mouseX", Value = mouseX},
                new StatisticUpdate {StatisticName = "mouseY", Value = mouseY},
                new StatisticUpdate {StatisticName = "tutorialComplete", Value = tutorialComplete},
                new StatisticUpdate {StatisticName = "levelTutorial", Value = levelTutorial},
                new StatisticUpdate {StatisticName = "mouseInvert", Value = mouseInvert}


            };
        PlayFabClientAPI.UpdatePlayerStatistics(req,SetStatisticSuccess, SetStatsFailed);
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Setting Player Stat Player Max Health: " + PlayerMaxHealth);
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Setting Player Stat Player Health: " + PlayerHealth);
        //GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Setting Player Stat Player Max MP: " + MaxMP);
        //GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Setting Player Stat Player Current MP: " + CurrentMP);
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Setting Player Stat Player Max Stamina: " + MaxStamina);
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Setting Player Stat Player Current Stamina: " + CurrentStamina);
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Setting Player Stat Player Max Attack: " + MaxAttack);
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Setting Player Stat Player Current Attack: " + CurrentAttack);
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Setting Player Stat Player MaxDefense: " + MaxDefense);
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Setting Player Stat Player Current  Defense: " + CurrentDefense);
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Setting Player Stat Player EXP: " + EXP);
    }

    private void SetStatisticSuccess(UpdatePlayerStatisticsResult _result)
    {
        Debug.Log("User statistics updated");
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("PlayFab: Statistics Updated");
        statsSent = true;
    }
    public bool wereStatsSent()
    {
        return statsSent;
    }
    private void SetStatsFailed(PlayFabError _error)
    {
        //Debug.LogError(_error.GenerateErrorReport());
    }

    public void getStats()
    {

    }

    //This gets called in the player controller at start (this is used to load in data at start)
    public void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
            );
    }

    private void OnGetStatistics(GetPlayerStatisticsResult _result)
    {
        foreach (var stat in _result.Statistics)
        {
            switch (stat.StatisticName)
            {
                case "PlayerMaxHealth":
                    {
                        PlayerMaxHealth = stat.Value;
                        break;
                    }
                case "PlayerHealth":
                    {
                        PlayerHealth = stat.Value;
                        break;
                    }
                case "PlayerPositionX":
                    {
                        PlayerPositionX = stat.Value;
                        break;
                    }
                case "PlayerPositionY":
                    {
                        PlayerPositionY = stat.Value;
                        break;
                    }
                case "PlayerPositionZ":
                    {
                        PlayerPositionZ = stat.Value;
                        break;
                    }
                case "JustStarted":
                    {
                        JustStarted = stat.Value;
                        break;
                    }
                case "Level":
                    {
                        Level = stat.Value;
                        break;
                    }
                //      case "MaxMP":
                //        {
                //        MaxMP = stat.Value;
                //          break;
                //  }
                // case "CurrentMP":
                //   {
                //   CurrentMP = stat.Value;
                //     break;
                // }
                case "MaxStamina":
                    {
                        MaxStamina = stat.Value;
                        break;
                    }
                case "CurrentStamina":
                    {
                        CurrentStamina = stat.Value;
                        break;
                    }
                case "MaxAttack":
                    {
                        MaxAttack = stat.Value;
                        break;
                    }
                case "CurrentAttack":
                    {
                        CurrentAttack = stat.Value;
                        break;
                    }
                case "MaxDefense":
                    {
                        MaxDefense = stat.Value;
                        break;
                    }
                case "CurrentDefense":
                    {
                        CurrentDefense = stat.Value;
                        break;
                    }
                case "EXP":
                    {
                        EXP = stat.Value;
                        break;
                    }
                case "hasSeenIntro":
                    {
                        hasSeenIntro = stat.Value;
                        break;
                    }
                case "mouseX":
                    {
                        mouseX = stat.Value;
                        break;
                    }
                case "mouseY":
                    {
                        mouseY = stat.Value;
                        break;
                    }
                case "tutorialComplete":
                    {
                        tutorialComplete = stat.Value;
                        break;
                    }
                case "levelTutorial":
                     { 
                        levelTutorial = stat.Value;
                        break;
                      }
                case "mouseInvert":
                    {
                        mouseInvert = stat.Value;
                        break;
                    }
            }
        }
    }

        // Update is called once per frame
    void Update()
    {
        if (run)
            getPlayerStats();
        //Debug.Log($"{PlayerPositionX}, {PlayerPositionY}, {PlayerPositionZ}");
    }
    public void updateMouseInvert(int _m)
    {
        mouseInvert = _m;
        SetStatistics();
    }
    //this is called when you want to save
    public void UpdateEXP(int _xp)
    {
        EXP = _xp;
        SetStatistics();
    }
    
    public void updateMouseSettingsX (int _x)
    {
        mouseX = _x;
       // Debug.Log(_x);
        SetStatistics();
        //setMouseSettings();
    }
   public void updateMouseSettingsY(int _y)
    {
        mouseY = _y;
       // Debug.Log(_y);

       // setMouseSettings();
    }
    public void UpdateSeenIntro()
    {
        if (hasSeenIntro!=1)
        {/*
            var player = GameObject.FindGameObjectWithTag("Player").gameObject;
            var playerStats = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStatsController>();
            PlayFabStats.Instance.UpdateStats((int)playerStats.getPHealthMax(), (int)playerStats.getPHealth(), (int)player.transform.position.x, (int)player.transform.position.y,
                (int)player.transform.position.z, 1, (int)playerStats.getManaMax(), (int)playerStats.getPMana(), (int)playerStats.getStaminahMax(), (int)playerStats.getPStamina(),
                (int)playerStats.getAttckMax(), (int)playerStats.getPAttck(), (int)playerStats.getPDefenseMax(), (int)playerStats.getPDefense(), (int)playerStats.getExperiancePoints(), SceneManager.GetActiveScene().buildIndex);*/
            hasSeenIntro = 1;
            
        }
       SetStatistics();
    }

    public void updateLevelTutorial(int _tut)
    {
        levelTutorial = _tut;
        SetStatistics();
    }


    public void updateTutorial(int _tut)
    {
        tutorialComplete = _tut;
        SetStatistics();
    }
    public int getTutorial()
    {
        return tutorialComplete;
    }
    public void UpdateStats(int _maxHP, int _hp, int _posx, int _posy, int _posz, int _jStart, int _maxMP, int _currMP, int _maxStam, int _currStam, int _maxAtk, int _currAtk, int _maxDef, int _curDef, int _exp, int _index)
    {
        timer = 0;
        PlayerMaxHealth = _maxHP;
        PlayerHealth = _hp;
        PlayerPositionX = _posx;
        PlayerPositionY = _posy;
        PlayerPositionZ = _posz;
        JustStarted = _jStart;
        Level = _index;
       // Level = SceneManager.GetActiveScene().buildIndex;
        //MaxMP = _maxMP;
        //CurrentMP = _currMP;
        MaxStamina = _maxStam;
        CurrentStamina = _currStam;
        MaxAttack = _maxAtk;
        CurrentAttack = _currAtk;
        MaxDefense = _maxDef;
        CurrentDefense = _curDef;
        EXP = _exp;
        SetStatistics();
    }

    public Vector3 GetAsVector()
    {
        return new Vector3(PlayerPositionX, PlayerPositionY + 1, PlayerPositionZ);
    }
}

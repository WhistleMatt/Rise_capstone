using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizrdBossHealthBarController : MonoBehaviour
{
    public Slider _slider;
    public GameObject _boss;
    // [SerializeField]   private WizrdBossStatsController _wizStatController;
    private CompanionProperties m_companionProperties;
    int health = 99999;

    public void SetMaxHealth(float health)
    {
        _slider.maxValue = health;
    }

    public void SetHealth(float health)
    {
        _slider.value = health;
    }

    private void Start()
    {
       // _wizStatController = this.gameObject.GetComponent<WizrdBossStatsController>();
    }
    private void Awake()
    {
    

        

    }

    private void Update()
    {
        
      SetMaxHealth(_boss.GetComponent<WizrdBossStatsController>().getPHealthMax());
      SetHealth(_boss.GetComponent<WizrdBossStatsController>().getPHealth());
            
      
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarController : MonoBehaviour
{
    public Slider _slider;
    public GameObject _boss;
 
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
        /*
        SetMaxHealth(_boss.GetComponent<WizrdBossStatsController>().getPHealthMax());
        SetHealth(_boss.GetComponent<WizrdBossStatsController>().getPHealth());
        */

    }
}

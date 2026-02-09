using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardHurtBoxLogic : MonoBehaviour
{
    public HitBoxController.ePlayer id;

    [SerializeField] private WizrdBossStatsController m_single_stats_cont;
    [SerializeField] private Multiplayer_WizardStat_Controller m_multi_stat_con;

    private float damageMod;
    // Start is called before the first frame update
    /*
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("HIIIIIIIIIIT");
        HitBoxLogic hbl = other.gameObject.GetComponentInChildren<HitBoxLogic>();
        if (hbl != null)
        {
            if (id != hbl.id)
            {

                this.transform.parent.gameObject.GetComponent<WizrdBossStatsController>().setPHealth(this.transform.parent.gameObject.GetComponent<WizrdBossStatsController>().getPHealth() - 1);
            }
        }
    }*/
    public void OnTriggerEnter(Collider other)
    {
        
        HitBoxLogic hbl = other.gameObject.GetComponentInChildren<HitBoxLogic>();
        if (hbl != null)
        {
            if (id != hbl.id)
            {
                if (m_single_stats_cont != null)
                {
                    damageMod = ((float)((other.GetComponentInParent<PlayerStatsController>().getPAttck() * 0.75) - (this.transform.parent.gameObject.GetComponent<WizrdBossStatsController>().getPDefense() * 0.15)));
                    this.transform.parent.gameObject.GetComponent<WizrdBossStatsController>().setPHealth(this.transform.parent.gameObject.GetComponent<WizrdBossStatsController>().getPHealth() - 1);
                    this.GetComponent<AudioSource>().Play();
                }
                else
                {
                    damageMod = ((float)((other.GetComponentInParent<PlayerStatsController>().getPAttck() * 0.75) - (this.transform.parent.gameObject.GetComponent<Multiplayer_WizardStat_Controller>().getPDefense() * 0.15)));
                    this.transform.parent.gameObject.GetComponent<Multiplayer_WizardStat_Controller>().setPHealth(this.transform.parent.gameObject.GetComponent<Multiplayer_WizardStat_Controller>().getPHealth() - 1);
                    this.GetComponent<AudioSource>().Play();
                }
            }
        }

    }
}

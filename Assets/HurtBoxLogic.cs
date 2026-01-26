using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicolas Chatziargiriou
public class HurtBoxLogic : MonoBehaviour
{
    public HitBoxController.ePlayer id;
    // Start is called before the first frame update
    private float damageMod;
    private GameObject enemyBar;

    private PlayerStatsController m_single_stats_cont;
    private Multiplayer_Enemy_Stat_Controller m_multi_stat_con;

    private void Start()
    {
        m_single_stats_cont = this.transform.parent.gameObject.GetComponent<PlayerStatsController>();
        m_multi_stat_con = this.transform.parent.gameObject.GetComponent<Multiplayer_Enemy_Stat_Controller>();
    }

    private void Update()
    {
        if (m_single_stats_cont == null && m_multi_stat_con == null)
        {
            m_single_stats_cont = this.transform.parent.gameObject.GetComponent<PlayerStatsController>();
            m_multi_stat_con = this.transform.parent.gameObject.GetComponent<Multiplayer_Enemy_Stat_Controller>();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        
        HitBoxLogic hbl = other.gameObject.GetComponentInChildren<HitBoxLogic>();
        if(hbl !=null)
        {
            if (id!= hbl.id)
            {
                //  if (hbl.id==HitBoxController.ePlayer.p1)
                //  {

                if (m_single_stats_cont != null)
                {
                    if (other.gameObject.GetComponentInChildren<HitBoxLogic>().GetSingleStats() != null)
                    {
                        damageMod = ((float)((other.gameObject.GetComponentInChildren<HitBoxLogic>().GetSingleStats().getPAttck() * 0.75) - (m_single_stats_cont.getPDefense() * 0.15)));
                        m_single_stats_cont.setPHealth(m_single_stats_cont.getPHealth() - damageMod);
                    }
                    else
                    {
                        damageMod = ((float)((other.gameObject.GetComponentInChildren<HitBoxLogic>().GetMultiStats().getPAttck() * 0.75) - (m_single_stats_cont.getPDefense() * 0.15)));
                        m_single_stats_cont.setPHealth(m_single_stats_cont.getPHealth() - damageMod);
                    }
                        this.GetComponent<AudioSource>().Play();

                    //}
                    if (id == HitBoxController.ePlayer.p2)
                    {
                        enemyBar = GameObject.FindGameObjectWithTag("EnemyHealthBar");
                        enemyBar.GetComponent<EnemyHealthBarController>().SetMaxHealth(m_single_stats_cont.getPHealthMax());
                        enemyBar.GetComponent<EnemyHealthBarController>().SetHealth(m_single_stats_cont.getPHealth());
                        if (m_single_stats_cont.getPHealth() <= 0)
                        {
                            this.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    if (other.gameObject.GetComponentInChildren<HitBoxLogic>().GetSingleStats() != null)
                    {
                        damageMod = ((float)((other.gameObject.GetComponentInChildren<HitBoxLogic>().GetSingleStats().getPAttck() * 0.75) - (m_multi_stat_con.getPDefense() * 0.15)));
                        m_multi_stat_con.setPHealth(m_multi_stat_con.getPHealth() - damageMod);
                    }
                    else
                    {
                        damageMod = ((float)((other.gameObject.GetComponentInChildren<HitBoxLogic>().GetMultiStats().getPAttck() * 0.75) - (m_multi_stat_con.getPDefense() * 0.15)));
                        m_multi_stat_con.setPHealth(m_multi_stat_con.getPHealth() - damageMod);
                    }
                        this.GetComponent<AudioSource>().Play();

                    //}
                    if (id == HitBoxController.ePlayer.p2)
                    {
                        enemyBar = GameObject.FindGameObjectWithTag("EnemyHealthBar");
                        enemyBar.GetComponent<EnemyHealthBarController>().SetMaxHealth(m_multi_stat_con.getPHealthMax());
                        enemyBar.GetComponent<EnemyHealthBarController>().SetHealth(m_multi_stat_con.getPHealth());
                        if (m_multi_stat_con.getPHealth() <= 0)
                        {
                            this.gameObject.SetActive(false);
                        }
                    }
                }
                
            }
        }

    }

}

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
    private Multiplayer_Enemy_Stat_Controller m_multi_stat_con_enemy;
    private Network_Player_stats_controller m_multi_stat_con_player;

    private void Start()
    {
        m_single_stats_cont = this.transform.parent.gameObject.GetComponent<PlayerStatsController>();
        m_multi_stat_con_enemy = this.transform.parent.gameObject.GetComponent<Multiplayer_Enemy_Stat_Controller>();
        m_multi_stat_con_player = this.transform.parent.gameObject.GetComponent<Network_Player_stats_controller>();
    }

    private void Update()
    {
        if (m_single_stats_cont == null && m_multi_stat_con_enemy == null)
        {
            m_single_stats_cont = this.transform.parent.gameObject.GetComponent<PlayerStatsController>();
            m_multi_stat_con_enemy = this.transform.parent.gameObject.GetComponent<Multiplayer_Enemy_Stat_Controller>();
            m_multi_stat_con_player = this.transform.parent.gameObject.GetComponent<Network_Player_stats_controller>();
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
                        damageMod = ((float)((other.gameObject.GetComponentInChildren<HitBoxLogic>().GetSingleStats().getPAttck() * 0.75) - (m_multi_stat_con_enemy.getPDefense() * 0.15)));
                        m_multi_stat_con_enemy.setPHealth(m_multi_stat_con_enemy.getPHealth() - damageMod);
                    }
                    else
                    {
                        damageMod = ((float)((other.gameObject.GetComponentInChildren<HitBoxLogic>().GetMultiStats().getPAttck() * 0.75) - (m_multi_stat_con_enemy.getPDefense() * 0.15)));
                        m_multi_stat_con_enemy.setPHealth(m_multi_stat_con_enemy.getPHealth() - damageMod);
                    }
                        this.GetComponent<AudioSource>().Play();

                    //}
                    if (id == HitBoxController.ePlayer.p2)
                    {
                        enemyBar = GameObject.FindGameObjectWithTag("EnemyHealthBar");
                        enemyBar.GetComponent<EnemyHealthBarController>().SetMaxHealth(m_multi_stat_con_enemy.getPHealthMax());
                        enemyBar.GetComponent<EnemyHealthBarController>().SetHealth(m_multi_stat_con_enemy.getPHealth());
                        if (m_multi_stat_con_enemy.getPHealth() <= 0)
                        {
                            this.gameObject.SetActive(false);
                        }
                    }
                }
                
            }
        }

    }

}

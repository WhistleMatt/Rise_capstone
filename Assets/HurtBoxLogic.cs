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
    
    public void OnTriggerEnter(Collider other)
    {
        
        HitBoxLogic hbl = other.gameObject.GetComponentInChildren<HitBoxLogic>();
        if(hbl !=null)
        {
            if (id!= hbl.id)
            {
              //  if (hbl.id==HitBoxController.ePlayer.p1)
              //  {
                   
                    damageMod = ((float)((other.GetComponentInParent<PlayerStatsController>().getPAttck() * 0.75) - (this.transform.parent.gameObject.GetComponent<PlayerStatsController>().getPDefense() * 0.15)));
               // }
               // else
              //  {
                    this.transform.parent.gameObject.GetComponent<PlayerStatsController>().setPHealth(this.transform.parent.gameObject.GetComponent<PlayerStatsController>().getPHealth() - damageMod);
                this.GetComponent<AudioSource>().Play();

                //}
                if (id == HitBoxController.ePlayer.p2)
                {
                    enemyBar = GameObject.FindGameObjectWithTag("EnemyHealthBar");
                    enemyBar.GetComponent<EnemyHealthBarController>().SetMaxHealth(this.transform.parent.gameObject.GetComponent<PlayerStatsController>().getPHealthMax());
                    enemyBar.GetComponent<EnemyHealthBarController>().SetHealth(this.transform.parent.gameObject.GetComponent<PlayerStatsController>().getPHealth());
                    if (this.transform.parent.gameObject.GetComponent<PlayerStatsController>().getPHealth()<=0)
                    {
                        this.gameObject.SetActive(false);
                    }
                }
                
            }
        }

    }

}

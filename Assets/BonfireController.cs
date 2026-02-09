using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Nicolas Chatziargiriou
public class BonfireController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> EnemyTemplates = new List<GameObject>();
    [SerializeField]
    private List<GameObject> EnemyList = new List<GameObject>();
    [SerializeField]
    private bool resetonStartup = false;

    // Start is called before the first frame update
    void Start()
    {
        // Uses the enemies in the scene as a template to create the enemies in runtime
        /*  foreach (GameObject x in EnemyTemplates)
             {
                 x.SetActive(false);
             }

          foreach (GameObject x in EnemyTemplates)
             {
                 EnemyList.Add(Instantiate(x));
             }

           foreach(GameObject x in EnemyList)
            {
                 x.SetActive(true);
            }*/
        if (resetonStartup)
            resetEnemies();
        else
        {
            GameObject[] foes = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject go in foes)
            {
                EnemyList.Add(go);
            }
        }
    }

public void clearEnemyCache()
    {
        foreach (GameObject x in EnemyTemplates)
        {
            x.SetActive(false);
        }

        foreach (GameObject x in EnemyList)
        {
            Destroy(x);
        }
        EnemyList.Clear();
    }

 public void useBonfire()
    {
        resetCrates();
        resetEnemies();
        resetPlayerHealth();
        var player = GameObject.FindGameObjectWithTag("Player").gameObject;
        player.GetComponent<Potion_Singleplayer_Script>().ResetHeals();
        var playerStats = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStatsController>();
        PlayFabStats.Instance.UpdateStats((int)playerStats.getPHealthMax(), (int)playerStats.getPHealth(), (int)player.transform.position.x, (int)player.transform.position.y,
            (int)player.transform.position.z, 1, (int)playerStats.getManaMax(), (int)playerStats.getPMana(), (int)playerStats.getStaminahMax(), (int)playerStats.getPStamina(), 
            (int)playerStats.getAttckMax(), (int)playerStats.getPAttck(), (int)playerStats.getPDefenseMax(), (int)playerStats.getPDefense(), (int)playerStats.getExperiancePoints(), SceneManager.GetActiveScene().buildIndex);
    }

    private void resetPlayerHealth()
    {
        GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStatsController>().setPHealth(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsController>().getPHealthMax());
        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Bon Fire; Player Healed");
        this.gameObject.GetComponent<AudioSource>().Play();
    }

    private void resetEnemies()
    {
        GameObject[] bonfires = GameObject.FindGameObjectsWithTag("bonFire");

        foreach (GameObject b in bonfires)
        {
            b.GetComponent<BonfireController>().clearEnemyCache();
        }
        /*
        foreach (GameObject x in EnemyTemplates)
        {
            x.SetActive(false);
        }

        foreach (GameObject x in EnemyList)
        {
            Destroy(x);
        }
        EnemyList.Clear();
        */
       foreach (GameObject x in EnemyTemplates)
         {
                EnemyList.Add(Instantiate(x));
          }
        
      foreach (GameObject x in EnemyList)
         {
            x.SetActive(true);
         }
        
    }

    private void resetCrates()
    {
        GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate");
        foreach (GameObject c in crates)
        {
            c.GetComponent<CrateDestroyController>().toggleFixed();
        }
    }
}

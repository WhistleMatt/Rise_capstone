using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    private string currentScene;
    private float timer = 0;
    private float time = 3f;
    private bool beginTimer = false;
    private string level;
    public void teleport()
    {
        if (!beginTimer)
        {
            this.gameObject.GetComponent<AudioSource>().Play();
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Teleport Enabled");
            currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == "Level1")
            {
                var player = GameObject.FindGameObjectWithTag("Player").gameObject;
                //    var playerStats = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStatsController>();
                //PlayFabStats.Instance.UpdateStats((int)playerStats.getPHealthMax(), (int)playerStats.getPHealth(), 7, 0, 11, 1, (int)playerStats.getManaMax(), (int)playerStats.getPMana(), (int)playerStats.getStaminahMax(), (int)playerStats.getPStamina(), (int)playerStats.getAttckMax(), (int)playerStats.getPAttck(), (int)playerStats.getPDefenseMax(), (int)playerStats.getPDefense(), (int)playerStats.getExperiancePoints(),2);
                PlayFabStats.Instance.UpdateStats((int)player.GetComponent<PlayerStatsController>().getPHealthMax(), (int)player.GetComponent<PlayerStatsController>().getPHealth(), (int)player.transform.position.x, (int)player.transform.position.y,
                (int)player.transform.position.z, 1, (int)player.GetComponent<PlayerStatsController>().getManaMax(), (int)player.GetComponent<PlayerStatsController>().getPMana(), (int)player.GetComponent<PlayerStatsController>().getStaminahMax(), (int)player.GetComponent<PlayerStatsController>().getPStamina(),
                (int)player.GetComponent<PlayerStatsController>().getAttckMax(), (int)player.GetComponent<PlayerStatsController>().getPAttck(), (int)player.GetComponent<PlayerStatsController>().getPDefenseMax(), (int)player.GetComponent<PlayerStatsController>().getPDefense(), (int)player.GetComponent<PlayerStatsController>().getExperiancePoints(), 3);

                beginTimer = true;
                timer = 2;
                level = "Level2";
                GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Teleport Selected Level 2");
                GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Load Async Called");
                GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Load Async Called Confirmed");
                SceneManager.LoadScene("level2");
                //  SceneManager.LoadSceneAsync(level);
                //  SceneManager.LoadSceneAsync("Level2");
            }

            if (currentScene == "Level2")
            {
                var player = GameObject.FindGameObjectWithTag("Player").gameObject;
                var playerStats = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStatsController>();
                //PlayFabStats.Instance.UpdateStats((int)playerStats.getPHealthMax(), (int)playerStats.getPHealth(), 7, 0, 11, 1, (int)playerStats.getManaMax(), (int)playerStats.getPMana(), (int)playerStats.getStaminahMax(), (int)playerStats.getPStamina(), (int)playerStats.getAttckMax(), (int)playerStats.getPAttck(), (int)playerStats.getPDefenseMax(), (int)playerStats.getPDefense(), (int)playerStats.getExperiancePoints(),2);
                PlayFabStats.Instance.UpdateStats((int)player.GetComponent<PlayerStatsController>().getPHealthMax(), (int)player.GetComponent<PlayerStatsController>().getPHealth(), (int)player.transform.position.x, (int)player.transform.position.y,
                    (int)player.transform.position.z, 1, (int)player.GetComponent<PlayerStatsController>().getManaMax(), (int)player.GetComponent<PlayerStatsController>().getPMana(), (int)player.GetComponent<PlayerStatsController>().getStaminahMax(), (int)player.GetComponent<PlayerStatsController>().getPStamina(),
                    (int)player.GetComponent<PlayerStatsController>().getAttckMax(), (int)player.GetComponent<PlayerStatsController>().getPAttck(), (int)player.GetComponent<PlayerStatsController>().getPDefenseMax(), (int)player.GetComponent<PlayerStatsController>().getPDefense(), (int)player.GetComponent<PlayerStatsController>().getExperiancePoints(), 3);


                beginTimer = true;
                timer = 2;
                level = "Level1";
                GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Teleport Selected Level 2");
                GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Load Async Called");
                GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Load Async Called Confirmed");
                //SceneManager.LoadScene("level1");
                // SceneManager.LoadSceneAsync(level);
                // SceneManager.LoadSceneAsync("Level2");
            }

        }
        /*
        if (currentScene =="Level2")
        {
            var player = GameObject.FindGameObjectWithTag("Player").gameObject;
            var playerStats = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStatsController>();

            //PlayFabStats.Instance.UpdateStats((int)playerStats.getPHealthMax(), (int)playerStats.getPHealth(), 6, 0, 0, 1, (int)playerStats.getManaMax(), (int)playerStats.getPMana(), (int)playerStats.getStaminahMax(), (int)playerStats.getPStamina(), (int)playerStats.getAttckMax(), (int)playerStats.getPAttck(), (int)playerStats.getPDefenseMax(), (int)playerStats.getPDefense(), (int)playerStats.getExperiancePoints(),1);
            PlayFabStats.Instance.UpdateStats((int)playerStats.getPHealthMax(), (int)playerStats.getPHealth(), (int)player.transform.position.x, (int)player.transform.position.y,
             (int)player.transform.position.z, 1, (int)playerStats.getManaMax(), (int)playerStats.getPMana(), (int)playerStats.getStaminahMax(), (int)playerStats.getPStamina(),
             (int)playerStats.getAttckMax(), (int)playerStats.getPAttck(), (int)playerStats.getPDefenseMax(), (int)playerStats.getPDefense(), (int)playerStats.getExperiancePoints(), SceneManager.GetActiveScene().buildIndex);
        
          beginTimer = true;
          timer = 0;
        level = "Level1";
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Teleport Selected Level 1");
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Teleport Selected Level 2");
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Load Async Called");
            GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Load Async Called Confirmed");
            SceneManager.LoadScene("level1");
            //  SceneManager.LoadSceneAsync(level);
            // SceneManager.LoadSceneAsync("Level1");
        }
        */
    }
    private void Update()
    {
        if (beginTimer )//&& GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().wereStatsSent())
        {
          //  SceneManager.LoadSceneAsync(level);
            
            timer = timer + Time.deltaTime;
            if (timer>time)
            {
                beginTimer = false;
                timer = 0; 
                GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Load Async Called");
                SceneManager.LoadSceneAsync(level);
                timer = 0;
                GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Load Async Called Confirmed");
            }
        }
        
    }
}
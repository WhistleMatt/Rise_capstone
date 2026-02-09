using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMC.Runtime;
using System;

//written byt Nicolas Chatziargiriou 
//extended by Matthew Whistle


public class EnemyWander : FSMC_Behaviour
{
    private Network_Player_Controller[] m_players = {null};
    private Network_Player_Controller m_player_to_chase;
    private PlayerStatsController m_singleplayerStatController;
    private Multiplayer_Enemy_Stat_Controller m_multiStatController;

    public override void StateInit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        //m_players = GameObject.FindObjectsByType<Network_Player_Controller>(FindObjectsSortMode.InstanceID);
        m_singleplayerStatController = executer.gameObject.GetComponent<PlayerStatsController>();
        m_multiStatController = executer.gameObject.GetComponent<Multiplayer_Enemy_Stat_Controller>();
    }
    public override void OnStateEnter(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        
        bool istrue = executer.gameObject.GetComponent<EnemyPathController>().enablePathing();
        if (istrue)
        {
            executer.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
        }
    }

    public override void OnStateUpdate(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        m_players = GameObject.FindObjectsByType<Network_Player_Controller>(FindObjectsSortMode.InstanceID);
        //Debug.Log(m_players.Length);
        if (m_players.Length <= 1)
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null) return;
            if (Vector3.Distance(executer.gameObject.transform.position, playerObject.transform.position) <= 5)
            {
                executer.gameObject.GetComponent<EnemyPathController>().disablePathing();
                stateMachine.SetBool("Chase", true);
            }
        }
        else
        {
            foreach (Network_Player_Controller player in m_players)
            {
                if (Vector3.Distance(executer.gameObject.transform.position, player.transform.position) <= 5)
                {
                    m_player_to_chase = player;
                    executer.gameObject.GetComponent<EnemyPathController>().disablePathing();
                    stateMachine.SetBool("Chase", true);
                }
            }
        }
        if (m_singleplayerStatController != null)
        {
            if (m_singleplayerStatController.getPHealth() <= 0)
            {
                stateMachine.SetBool("Return", false);
                stateMachine.SetBool("Attack", false);
                stateMachine.SetBool("Chase", false);
                stateMachine.SetBool("Dead", true);
            }
        }
        else
        {
            if (m_multiStatController.getPHealth() <= 0)
            {
                stateMachine.SetBool("Return", false);
                stateMachine.SetBool("Attack", false);
                stateMachine.SetBool("Chase", false);
                stateMachine.SetBool("Dead", true);
            }
        }
    }

    public override void OnStateExit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        executer.gameObject.GetComponent<EnemyPathController>().disablePathing();
        stateMachine.SetBool("Return", false);
    }
}
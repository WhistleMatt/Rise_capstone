using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMC.Runtime;
using System;
using UnityEngine.AI;
//Nicolas Chatziargiriou
[Serializable]
public class EnemyChase : FSMC_Behaviour
{
    private bool chasing = false;

    private PlayerStatsController m_singleplayerStatController;
    private Multiplayer_Enemy_Stat_Controller m_multiStatController;
    private Network_Player_Controller m_player_to_chase;

    public override void StateInit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        m_singleplayerStatController = executer.gameObject.GetComponent<PlayerStatsController>();
        m_multiStatController = executer.gameObject.GetComponent<Multiplayer_Enemy_Stat_Controller>();
    }
    public override void OnStateEnter(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        chasing = true;
        Debug.Log("Entered Chase");
        chasing = true;
        executer.gameObject.GetComponent<NavMeshAgent>().SetDestination( GameObject.FindGameObjectWithTag("Player").transform.position);
        executer.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
    }

    public override void OnStateUpdate(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        if (m_singleplayerStatController != null)
        {
            if (executer.gameObject.GetComponent<PlayerStatsController>().getPHealth() <= 0)
            {
                stateMachine.SetBool("Return", false);
                stateMachine.SetBool("Attack", false);
                stateMachine.SetBool("Chase", false);
                stateMachine.SetBool("Dead", true);
            }
            if (Vector3.Distance(executer.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 2)
            {
                stateMachine.SetBool("Chase", false);
                stateMachine.SetBool("Attack", true);
            }

            if (Vector3.Distance(executer.gameObject.transform.position, executer.gameObject.GetComponent<EnemyPathController>().getCurrentDestination().transform.position) < 20 && chasing == true)
            {
                executer.gameObject.GetComponent<NavMeshAgent>().SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
                executer.gameObject.GetComponent<Animator>().SetBool("isWalking", true);



            }
            else
            {
                chasing = false;
                executer.gameObject.GetComponent<NavMeshAgent>().SetDestination(executer.gameObject.GetComponent<EnemyPathController>().getCurrentDestination().transform.position);
                if (Vector3.Distance(executer.gameObject.transform.position, executer.gameObject.GetComponent<EnemyPathController>().getCurrentDestination().transform.position) <= 2)
                {
                    stateMachine.SetBool("Chase", false);
                    stateMachine.SetBool("Return", true);
                }
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

            //Network_Player_Controller[] m_players = GameObject.FindObjectsByType<Network_Player_Controller>(FindObjectsSortMode.InstanceID);

            Network_Player_Controller[] m_players = GameObject.FindObjectsByType<Network_Player_Controller>(FindObjectsSortMode.InstanceID);

            //Debug.Log("there are " + m_players.Length + " players");

            bool foundAppropPlayer = false;

            float[] vectorList = new float[4];

            int index = 0;

            foreach (Network_Player_Controller player in m_players)
            {
                //Debug.Log($"Distance from {player.NetworkObjectId} is: " + Vector3.Distance(executer.gameObject.transform.position, player.gameObject.transform.position));

                vectorList[index] = Vector3.Distance(executer.gameObject.transform.position, player.gameObject.transform.position);

                index += 1;

                //if (Vector3.Distance(executer.gameObject.transform.position, player.gameObject.transform.position) <= 2)
                //{
                //m_player_to_chase = player;
                //foundAppropPlayer = true;
                //break;
                //}
            }

            float minDist = 10f;
            int minDistIndex = 0;
            for (int i = 0; i < m_players.Length; i++)
            {
                if (vectorList[i] < minDist)
                {
                    minDist = vectorList[i];
                    minDistIndex = i;
                }
            }

            if (minDist < 20 && chasing)
            {
                m_player_to_chase = m_players[minDistIndex];
                foundAppropPlayer = true;
            }
            else
            {
                foundAppropPlayer = false;
            }

            if (!foundAppropPlayer)
            {
                m_player_to_chase = null;
            }

            if (m_player_to_chase != null)
            {
                if (minDist <= 2)
                {
                    stateMachine.SetBool("Chase", false);
                    stateMachine.SetBool("Attack", true);
                }
            }
            /*
            foreach (Network_Player_Controller player in m_players)
            {
                if (Vector3.Distance(executer.gameObject.transform.position, player.gameObject.transform.position) <= 2)
                {
                    m_player_to_chase = player;
                    stateMachine.SetBool("Chase", false);
                    stateMachine.SetBool("Attack", true);
                }
                else if (Vector3.Distance(executer.gameObject.transform.position, player.gameObject.transform.position) <= 20)
                {
                    m_player_to_chase = player;
                }
            }
            */

            if (Vector3.Distance(executer.gameObject.transform.position, executer.gameObject.GetComponent<EnemyPathController>().getCurrentDestination().transform.position) < 20 && chasing == true)
            {
                executer.gameObject.GetComponent<NavMeshAgent>().SetDestination(m_player_to_chase.gameObject.transform.position);
                executer.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            }
            else
            {
                chasing = false;
                executer.gameObject.GetComponent<NavMeshAgent>().SetDestination(executer.gameObject.GetComponent<EnemyPathController>().getCurrentDestination().transform.position);
                if (Vector3.Distance(executer.gameObject.transform.position, executer.gameObject.GetComponent<EnemyPathController>().getCurrentDestination().transform.position) <= 2)
                {
                    stateMachine.SetBool("Chase", false);
                    stateMachine.SetBool("Return", true);
                }
            }
        }
        
    }

    public override void OnStateExit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {

        Debug.Log("Exited Chase");
    }
}
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
        
    }

    public override void OnStateExit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {

        Debug.Log("Exited Chase");
    }
}
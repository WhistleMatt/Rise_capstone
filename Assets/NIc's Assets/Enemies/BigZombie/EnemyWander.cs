using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMC.Runtime;
using System;

//Nicolas Chatziargiriou 



public class EnemyWander : FSMC_Behaviour
{
    public override void StateInit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
       
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
    if (Vector3.Distance(executer.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position)<=5)
        {
            executer.gameObject.GetComponent<EnemyPathController>().disablePathing();
            stateMachine.SetBool("Chase", true);
        }

    if (executer.gameObject.GetComponent<PlayerStatsController>().getPHealth()<=0)
        {
            stateMachine.SetBool("Return", false);
            stateMachine.SetBool("Attack", false);
            stateMachine.SetBool("Chase", false);
            stateMachine.SetBool("Dead", true);
        }
    }

    public override void OnStateExit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        executer.gameObject.GetComponent<EnemyPathController>().disablePathing();
        stateMachine.SetBool("Return", false);
    }
}
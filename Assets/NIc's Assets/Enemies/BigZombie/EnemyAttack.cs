using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMC.Runtime;
using System;

[Serializable]
public class EnemyAttack : FSMC_Behaviour
{
    private float timer = 0;
    private float attackTime=3;
    public override void StateInit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
    
    }
    public override void OnStateEnter(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        timer = 0;
        attackTime = 3;
    }

    public override void OnStateUpdate(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        if (executer.gameObject.GetComponent<PlayerStatsController>().getPHealth() <= 0)
        {
            stateMachine.SetBool("Return", false);
            stateMachine.SetBool("Attack", false);
            stateMachine.SetBool("Chase", false);
            stateMachine.SetBool("Dead", true);
        }
        if (Vector3.Distance(executer.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <=2)
        {

           
            executer.gameObject.GetComponent<EnemyPathController>().disablePathing();
            executer.gameObject.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);

            timer = timer + Time.deltaTime;
            if (timer > attackTime)
            {
                executer.gameObject.GetComponent<Animator>().SetTrigger("isAttacking");
                timer = 0;
                attackTime = UnityEngine.Random.Range(2, 6);
            }

        }
        else
        {

          
            stateMachine.SetBool("Attack", false);
            stateMachine.SetBool("Chase", true);

        }
    }

    public override void OnStateExit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        executer.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
    }
}
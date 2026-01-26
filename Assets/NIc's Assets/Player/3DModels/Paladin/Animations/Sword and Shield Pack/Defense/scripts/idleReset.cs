using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicolas Chatziargiriou
public class idleReset : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("performingAction", false);
        InputController inputController = animator.gameObject.GetComponent<InputController>();
        if (inputController != null)
        {
            inputController.updatePrevInput("Idle");
            inputController.RestInputCount();
            animator.gameObject./*transform.root.*/GetComponent<HitBoxController>().HitBoxDisable(0);
            animator.gameObject./*transform.root.*/GetComponent<HitBoxController>().HurtBoxEnable();
            return;
        }
        Network_Input_Controller network_input_controller = animator.gameObject.GetComponent<Network_Input_Controller>();
        if (network_input_controller != null)
        {
            network_input_controller.updatePrevInput("Idle");
            network_input_controller.RestInputCount();
            animator.gameObject./*transform.root.*/GetComponent<HitBoxController>().HitBoxDisable(0);
            animator.gameObject./*transform.root.*/GetComponent<HitBoxController>().HurtBoxEnable();
        }




    }

// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//{
//    
//}

// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
/* override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
 {


 }


 // OnStateMove is called right after Animator.OnAnimatorMove()
 //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
 //{
 //    // Implement code that processes and affects root motion
 //}

 // OnStateIK is called right after Animator.OnAnimatorIK()
 //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
 //{
 //    // Implement code that sets up animation IK (inverse kinematics)
 //}*/
}

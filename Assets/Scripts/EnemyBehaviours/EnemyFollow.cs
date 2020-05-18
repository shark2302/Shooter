using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : StateMachineBehaviour
{
    private Enemy _enemy;
   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       _enemy = animator.GetBehaviour<EnemyIdle>().GetEnemy();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_enemy == null)
            return;
        if (_enemy.GetTarget() == null)
        {
            _enemy.FindNearestEnemy();
            return;
        }
            _enemy.RotateToTarget(animator.transform, _enemy.GetTarget());
            if ((Vector3.Distance(_enemy.GetTarget().transform.position, animator.transform.position) >
                _enemy.GetRange() || !_enemy.CanShoot()) && _enemy.GetAgent().enabled)
            {
                if(_enemy.GetAgent() != null)
                    _enemy.GetAgent().speed = _enemy.GetSpeed();
                _enemy.GetAgent()?.SetDestination(_enemy.GetTarget().transform.position);
            }
            else
            {
                animator.SetBool("InRange", true);
            }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

   
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandBehaviour : StateMachineBehaviour
{
    private Enemy _enemy;
    private float _nextTimeToFire = 0f;
  
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemy = animator.GetBehaviour<EnemyIdle>().GetEnemy();
    }

    
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
        if(_enemy.GetAgent().enabled)
            _enemy.GetAgent().SetDestination(_enemy.transform.position);
        if (Time.time >= _nextTimeToFire)
        {
            _nextTimeToFire = Time.time + 1f / _enemy.GetFireRate();
            _enemy.Shoot();
        }
        if (Vector3.Distance(_enemy.GetTarget().transform.position, animator.transform.position) >
            _enemy.GetSafeDistance())
            animator.SetBool("SafeDistance", false);
        else if(Vector3.Distance(_enemy.GetTarget().transform.position, animator.transform.position) <=
                _enemy.GetPanicDistance())
            animator.SetBool("Panic", true);
        
    }

    
}

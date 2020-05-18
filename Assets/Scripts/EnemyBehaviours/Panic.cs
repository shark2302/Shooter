using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panic : StateMachineBehaviour
{
    private Enemy _enemy;

    private float _nextTimeToFire;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemy = animator.GetBehaviour<EnemyIdle>()?.GetEnemy();
        if(_enemy != null &&_enemy.GetAgent() != null)
            _enemy.GetAgent().enabled = false;
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
        if (Time.time >= _nextTimeToFire)
        {
            _nextTimeToFire = Time.time + 1f / _enemy.GetFireRate();
            _enemy.Shoot();
        }

        if (Vector3.Distance(_enemy.GetTarget().transform.position, animator.transform.position) <=
            _enemy.GetPanicDistance())
        {
            _enemy.Follow(-_enemy.GetSpeed(), _enemy.GetTarget());
        }
        else if (Vector3.Distance(_enemy.GetTarget().transform.position, animator.transform.position) >
                 _enemy.GetPanicDistance())
        {
            _enemy.GetAgent().enabled = true;
            animator.SetBool("Panic", false);
        }

    }

    
}

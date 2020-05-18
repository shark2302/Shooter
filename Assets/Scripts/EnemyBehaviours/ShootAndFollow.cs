using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ShootAndFollow : StateMachineBehaviour
{
    private Enemy _enemy;
    private float _nextTimeToFire = 0f;
   
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemy = animator.GetBehaviour<EnemyIdle>()?.GetEnemy();
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
        if (_enemy.GetTarget() != null)
        {
            if (Vector3.Distance(_enemy.GetTarget().transform.position, animator.transform.position) >
                _enemy.GetRange())
            {
                animator.SetBool("InRange", false);
            }            
            else if ((Vector3.Distance(_enemy.GetTarget().transform.position, animator.transform.position) >
                _enemy.GetSafeDistance() ||!_enemy.CanShoot()) && _enemy.GetAgent().enabled)
            {
                if(_enemy.GetAgent() != null)
                     _enemy.GetAgent().speed = _enemy.GetWalkSpeed();
                _enemy.GetAgent()?.SetDestination(_enemy.GetTarget().transform.position);
            }
            else
            {
               animator.SetBool("SafeDistance", true);
            }

            if (Time.time >= _nextTimeToFire)
            {
                _nextTimeToFire = Time.time + 1f / _enemy.GetFireRate();
                _enemy.Shoot();
            }
            
        }
    }

    
    
}

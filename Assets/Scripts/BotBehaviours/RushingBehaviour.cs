using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushingBehaviour : StateMachineBehaviour
{
    private Enemy _bot;

    private float _nextTimeToFire;
 
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bot = animator.GetBehaviour<IdleBehaviour>()?.GetBot();
       if(_bot != null && _bot.GetAgent()!= null)
            _bot.GetAgent().speed = _bot.GetSpeed();
    }

  
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_bot == null)
            return;
        if (_bot.GetTarget() == null)
            _bot.FindNearestEnemy();

        if (_bot.GetMainTarget() != null)
        {
            _bot.GetAgent()?.SetDestination(_bot.GetMainTarget().transform.position);
            if(_bot.GetTarget() == null)
                _bot.RotateToTarget(animator.transform, _bot.GetMainTarget());
            else 
                _bot.RotateToTarget(animator.transform, _bot.GetTarget());
            if (_bot.GetTarget() != null && Time.time >= _nextTimeToFire && _bot.CanShoot())
            {
                _nextTimeToFire = Time.time + 1f / _bot.GetFireRate();
                _bot.Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("IsRushing", false);
        }
    }

 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolllowingBehaviour : StateMachineBehaviour
{

    private Enemy _bot;
    private float _nextTimeToFire;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bot = animator.GetBehaviour<IdleBehaviour>().GetBot();
        if(_bot.GetAgent() != null)
            _bot.GetAgent().speed = _bot.GetSpeed();
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_bot.GetTarget() == null)
        {
            _bot.FindNearestEnemy();
        }

        if (_bot.GetPlayer() != null)
        {
            if (Vector3.Distance(_bot.GetPlayer().transform.position, animator.transform.position) >
                7)
            {
                _bot.GetAgent().SetDestination(_bot.GetPlayer().transform.position);
            }
            else
            {
                _bot.GetAgent().SetDestination(animator.transform.position);
            }
        }

        if (_bot.GetTarget() != null)
        {
            _bot.RotateToTarget(animator.transform, _bot.GetTarget());
            if (Time.time >= _nextTimeToFire && _bot.CanShoot())
            {
                _nextTimeToFire = Time.time + 1f / _bot.GetFireRate();
                _bot.Shoot();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(_bot.GetPlayer() != null)
            _bot.GetAgent().SetDestination(_bot.GetPlayer().transform.position);
            animator.SetBool("IsFollowing", false);
        }
           
    }

    
   
}

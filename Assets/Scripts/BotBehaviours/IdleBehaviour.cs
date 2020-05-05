using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    private Enemy _bot;
    private float _nextTimeToFire;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_bot == null)
            return; 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _bot.FindNearestEnemy();
            animator.SetBool("IsFollowing", true);
         
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            _bot.FindNearestEnemy();
            animator.SetBool("IsRushing", true);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            _bot.FindNearestEnemy();
            animator.SetBool("IsPatroling", true);
        }

        if (_bot.GetTarget() == null)
        {
            _bot.FindNearestEnemy();
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
        
    }
    
    public void SetBot(Enemy bot)
    {
        _bot = bot;
    }

    public Enemy GetBot()
    {
        return _bot;
    }
}

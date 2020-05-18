using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Patrol : StateMachineBehaviour
{
    private Enemy _bot;

    private Vector3[] _spots;
    private float waitTime;
    private Vector3 _curSpot;
    private float _nextTimeToFire;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bot = animator.GetBehaviour<IdleBehaviour>().GetBot();
        if (_bot.GetAgent() != null) 
            _bot.GetAgent().speed = _bot.GetWalkSpeed();
        waitTime = 2;
        Vector3 pos = animator.transform.position;
        _spots = new[]
        {
            new Vector3(pos.x, pos.y, pos.z + 5), new Vector3(pos.x + 5, pos.y, pos.z),
            new Vector3(pos.x - 5, pos.y, pos.z), new Vector3(pos.x, pos.y, pos.z - 5),
            new Vector3(pos.x - 5, pos.y, pos.z - 5), new Vector3(pos.x + 5, pos.y, pos.z + 5)
        };
        _curSpot = _spots[Random.Range(0, _spots.Length - 1)];
    }

  
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_bot.GetTarget() == null)
        {
            _bot.FindNearestEnemy();
        }
        _bot.GetAgent()?.SetDestination(_curSpot);
        if (Vector3.Distance(animator.transform.position, _curSpot) < 0.5f || waitTime < 0)
        {
            _curSpot = _spots[Random.Range(0, _spots.Length - 1)];
            waitTime = 2;
        }

        waitTime -= Time.deltaTime;
        if (_bot.GetTarget() != null)
        {
            _bot.RotateToTarget(animator.transform, _bot.GetTarget());
            if (Time.time >= _nextTimeToFire && _bot.CanShoot())
            {
                _nextTimeToFire = Time.time + 1f / _bot.GetFireRate();
                _bot.Shoot();
            }
            
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(_bot.GetPlayer() != null)
                _bot.GetAgent().SetDestination(_bot.GetPlayer().transform.position);
            animator.SetBool("IsPatroling", false);
        }
    }
    
}

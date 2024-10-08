﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : TargetEnemy
{
    public DetectionZone attackZone;

    public bool _hasTarget = false;

    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }

    public bool HasTarget { 
    get { return _hasTarget; }
    private set
        {
            _hasTarget = value;
            if (_hasTarget != false)
            {
                ChangeState(EnemyState.attack);
                animator.SetBool("hasTarget", true);
            }
            else if (_hasTarget == false)
            {
                animator.SetBool("hasTarget", false);
            }
        }
    }

    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                ChangeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
                animator.SetBool("isMoving", true);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            ChangeState(EnemyState.idle);
            animator.SetBool("isMoving", false);
        }
    }
}
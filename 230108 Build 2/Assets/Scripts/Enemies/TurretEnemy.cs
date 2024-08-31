using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : TargetEnemy
{
    public GameObject projectile;
    public float fireDelay;
    private float fireDelaySeconds;
    public bool canFire = true;

    void Update()
    {
        if (canFire == false)
        {
            fireDelaySeconds -= Time.deltaTime;
            if (fireDelaySeconds <= 0)
            {
                canFire = true;
                fireDelaySeconds = fireDelay;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, target.position);
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position,
                            transform.position) <= chaseRadius
           && Vector3.Distance(target.position,
                               transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk
              && currentState != EnemyState.stagger)
            {
                if (canFire)
                {
                    Vector3 tempVector = target.transform.position - transform.position;
                    GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
                    current.GetComponent<Projectile>().Launch(tempVector);
                    canFire = false;
                    ChangeState(EnemyState.walk);
                    animator.SetBool("wakeUp", true);
                }
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            animator.SetBool("wakeUp", false);
        }
    }
}

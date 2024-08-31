using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEnemy : Enemy
{
    public Rigidbody2D myRigidbody;

    [Header("Target Variables")]
    public Transform target;
    public float chaseRadius;
    public float attackRadius;

    [Header("Animator")]
    public Animator animator;
    //public float walkDelay;
    //float timer;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        //anim.SetBool("wakeUp", true);
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        CheckDistance();
    }

    /*public virtual void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                anim.SetBool("wakeUp", true);
                if (message.Equals("WakeUpEnded"))
                {
                    MoveCo();
                }
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            ChangeState(EnemyState.idle);
            anim.SetBool("wakeUp", false);
        }
    }

    void MoveCo()
    {
        Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        changeAnim(temp - transform.position);
        myRigidbody.MovePosition(temp);
        ChangeState(EnemyState.walk);
    }*/

    public virtual void CheckDistance()
    {
        //timer += Time.deltaTime;
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                {
                    Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                    ChangeAnim(temp - transform.position);
                    myRigidbody.MovePosition(temp);
                    ChangeState(EnemyState.walk);
                    animator.SetBool("wakeUp", true);
                }
                //if (timer > walkDelay)
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            ChangeState(EnemyState.idle);
            animator.SetBool("wakeUp", false);
        }
    }

    public void SetAnimFloat(Vector2 setVector)
    {
        animator.SetFloat("moveX", setVector.x);
        animator.SetFloat("moveY", setVector.y);
    }

    public void ChangeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if(direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, target.position);
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}

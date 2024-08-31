using System.Collections;
using UnityEngine;

public class BossCharge : AreaEnemy
{
    public float attackRate; // Time between attacks (in seconds)
    public float speedBonus;
    public float chaseDuration; // Duration of rapid chase (in seconds)
    public GameObject projectilePrefab;

    private float nextAction;
    private bool isChasing;

    private void OnEnable()
    {
        // Initialize next action time
        nextAction = Time.time + attackRate; // Random initial delay
        isChasing = false;
    }

    private void Update()
    {
        // Randomly choose an action
        if (Time.time >= nextAction)
        {
            if (Random.value < 0.5f) // 50% chance of chasing
            {
                // Apply the speed bonus
                moveSpeed += speedBonus;

                // Start a timer to reset the effect after the specified duration
                Invoke(nameof(ResetEffect), chaseDuration);

                nextAction = Time.time + attackRate;
            }
            else
            {
                Attack();
            }
        }
    }

    private void ResetEffect()
    {
        // Remove the speed bonus
        moveSpeed -= speedBonus;
    }

    private void Attack()
    {
        // Instantiate and fire a projectile towards the player
        Vector3 projectileDirection = (target.position - transform.position).normalized;
        GameObject newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody2D>().velocity = projectileDirection * 10f; // Adjust projectile speed

        // Set next action time for the next attack
        nextAction = Time.time + attackRate;
    }

    /*[Header("Charge Variables")]
    public float chargeFrequency; // Time frequency to charge (in seconds)
    public float chargeSpeed; // Charge speed
    public float chargeDuration; // Duration of charge (in seconds)

    private float currentSpeed;
    private float normalTimer;
    private float chargeTimer;
    private bool canCharge = false;

    private void OnEnable()
    {
        currentSpeed = moveSpeed;
        normalTimer = chargeFrequency;
    }

    void Update()
    {
        if (canCharge == false)
        {
            normalTimer -= Time.deltaTime;
            if (normalTimer <= 0)
            {
                chargeTimer -= Time.deltaTime;

                if (chargeTimer <= 0)
                {
                    canCharge = true;
                    currentSpeed = chargeSpeed;
                    //chargeTimer = chargeDuration;
                }
                else
                {
                    canCharge = false;
                    currentSpeed = moveSpeed;
                    //normalTimer = chargeFrequency;
                }
            }
        }
    }

    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius && boundary.bounds.Contains(target.transform.position))
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
                ChangeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
                animator.SetBool("wakeUp", true);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius || !boundary.bounds.Contains(target.transform.position))
        {
            animator.SetBool("wakeUp", false);
        }
    }*/
}


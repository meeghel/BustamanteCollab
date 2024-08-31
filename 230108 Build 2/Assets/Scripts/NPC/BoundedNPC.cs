﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundedNPC : Interactuable
{
    //Revisar por qué hereda de Sign y no de Interactable
    private Vector3 directionVector;
    private Transform myTransform;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Animator anim;
    private Transform playerPosition;
    public Collider2D bounds;
    private bool isMoving;
    //230522 agregué canMove para revisar implementar Dialog Manager
    public bool canMove;
    public bool playerInRange = false;
    private Signal context; 

    private DialogManager theDM;

    public float minMoveTime;
    public float maxMoveTime;
    private float moveTimeSeconds;
    public float minWaitTime;
    public float maxWaitTime;
    private float waitTimeSeconds;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        //theDM = FindObjectOfType<DialogManager>();
        moveTimeSeconds = Random.Range(minMoveTime, maxMoveTime);
        waitTimeSeconds = Random.Range(minWaitTime, maxWaitTime);
        //anim = GetComponent<Animator>();
        playerPosition = GameObject.FindWithTag("Player").transform;
        //myTransform = GetComponent<Transform>();
        //myRigidbody = GetComponent<Rigidbody2D>();
        ChangeDirection();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //base.Update();
        /*if (!theDM.dialogActive)
        {
            canMove = true;
        }*/
        //Revisar por qué hereda de Sign y no Interactable, o hacer propia clase
        if (!canMove)
        {
            myRigidbody.velocity = Vector2.zero;
            return;
        }

        if (isMoving)
        {
            moveTimeSeconds -= Time.deltaTime;
            if (moveTimeSeconds <= 0)
            {
                moveTimeSeconds = Random.Range(minMoveTime, maxMoveTime);
                isMoving = false;
                anim.speed = 0;
            }
            if (!playerInRange)
            {
                Move();
            }
        }
        else
        {
            waitTimeSeconds -= Time.deltaTime;
            if (waitTimeSeconds <= 0)
            {
                ChooseDifferentDirection();
                isMoving = true;
                anim.speed = 1;
                waitTimeSeconds = Random.Range(minWaitTime, maxWaitTime);
            }
        }
    }

    private void ChooseDifferentDirection()
    {
        Vector3 temp = directionVector;
        ChangeDirection();
        int loops = 0;
        while (temp == directionVector && loops < 100)
        {
            loops++;
            ChangeDirection();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            context.Raise();
            anim.speed = 0;
            playerInRange = true;
        }
    }

    private void Move()
    {
        Vector3 temp = myTransform.position + directionVector * speed * Time.deltaTime;
        if (bounds.bounds.Contains(temp))
        { 
        myRigidbody.MovePosition(temp);
        }
        else
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0:
                // Walking to the right
                directionVector = Vector3.right;
                break;
            case 1:
                // Walking up
                directionVector = Vector3.up;
                break;
            case 2:
                // Walking left
                directionVector = Vector3.left;
                break;
            case 3:
                // walking down
                directionVector = Vector3.down;
                break;
            default:
                break;
        }
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        anim.SetFloat("moveX", directionVector.x);
        anim.SetFloat("moveY", directionVector.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ChooseDifferentDirection();
    }

    public IEnumerator Interact(Transform initiator)
    {
        throw new System.NotImplementedException();
    }
}

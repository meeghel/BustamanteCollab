﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [Header("Idle")]
    [SerializeField] List<Sprite> idleDownSprites;
    [SerializeField] List<Sprite> idleUpSprites;
    [SerializeField] List<Sprite> idleRightSprites;
    [SerializeField] List<Sprite> idleLeftSprites;

    [Header("Walk")]
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;

    [SerializeField] FacingDirection defaultDirection = FacingDirection.Down;

    // Parameters
    public float MoveX { get; set; }

    public float MoveY { get; set; }

    public bool IsMoving { get; set; }

    //Agregue canMove para revisar estado Idle
    public bool canMove { get; set; }

    // States
    SpriteAnimator idleDownAnim;
    SpriteAnimator idleUpAnim;
    SpriteAnimator idleRightAnim;
    SpriteAnimator idleLeftAnim;

    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    public SpriteAnimator currentAnim;
    public FacingDirection currentDir;
    bool wasPreviouslyMoving;

    // References
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        idleDownAnim = new SpriteAnimator(idleDownSprites, spriteRenderer);
        idleUpAnim = new SpriteAnimator(idleUpSprites, spriteRenderer);
        idleRightAnim = new SpriteAnimator(idleRightSprites, spriteRenderer);
        idleLeftAnim = new SpriteAnimator(idleLeftSprites, spriteRenderer);

        walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);

        SetFacingDirection(defaultDirection);
    }

    private void Update()
    {
        var prevAnim = currentAnim;
        var prevDir = currentDir;
        
        if (IsMoving)
        {
            if (MoveX == 1)
                currentAnim = walkRightAnim;
            else if (MoveX == -1)
                currentAnim = walkLeftAnim;
            else if (MoveY == 1)
                currentAnim = walkUpAnim;
            else if (MoveY == -1)
                currentAnim = walkDownAnim;
        }
        else
        {
            if (MoveX == 1)
            {
                currentAnim = idleRightAnim;
                currentDir = FacingDirection.Right;
            }
            else if (MoveX == -1)
            {
                currentAnim = idleLeftAnim;
                currentDir = FacingDirection.Left;
            }
            else if (MoveY == 1)
            {
                currentAnim = idleUpAnim;
                currentDir = FacingDirection.Up;
            }
            else if (MoveY == -1)
            {
                currentAnim = idleDownAnim;
                currentDir = FacingDirection.Down;
            }
        }

        if (currentAnim != prevAnim || currentDir != prevDir || IsMoving != wasPreviouslyMoving)
            currentAnim.Start();

        currentAnim.HandleUpdate();
        /*if (IsMoving)
            currentAnim.HandleUpdate();
        else
            spriteRenderer.sprite = currentAnim.frames[0];*/

        wasPreviouslyMoving = IsMoving;

    }

    public void SetFacingDirection(FacingDirection dir)
    {
        if (dir == FacingDirection.Right)
        {
            MoveX = 1;
            MoveY = 0;
        }
        else if (dir == FacingDirection.Left)
        {
            MoveX = -1;
            MoveY = 0;
        }
        else if (dir == FacingDirection.Down)
        {
            MoveX = 0;
            MoveY = -1;
        }
        else if (dir == FacingDirection.Up)
        {
            MoveX = 0;
            MoveY = 1;
        }
    }

    public FacingDirection DefaultDirection
    {
        get => defaultDirection;
    }
}

public enum FacingDirection { Up, Down, Left, Right }

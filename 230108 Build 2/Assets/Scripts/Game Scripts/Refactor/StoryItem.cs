﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryItem : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] Dialog dialog;

    public void OnPlayerTriggered(PlayerController player)
    {
        //player.Character.Animator.IsMoving = false;
        player.Animator.SetBool("isMoving", false);
        StartCoroutine(DialogManagerRef.instance.ShowDialog(dialog));
    }

    public bool TriggerRepeatedly => false;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerFov : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        // TODO video #45 25:00
        //player.Character.Animator.IsMoving = false;
        GameController.Instance.OnEnterTrainersView(GetComponentInParent<TrainerController>());
    }
}

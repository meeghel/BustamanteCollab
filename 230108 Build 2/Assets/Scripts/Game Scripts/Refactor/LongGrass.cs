using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongGrass : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        //TODO revisar, no funciona random correctamente
        if (UnityEngine.Random.Range(1, 101) <= 10)
        {
            //TODO poner en Game Controller / Start: playerController.OnEncountered += StartBattle;
            //GameController.Instance.StartBattle();
            // TODO video #45 25:00
            //player.Character.Animator.IsMoving = false;
            Debug.Log("Encontraste un pokemon");
        }
    }

    public bool TriggerRepeatedly => true;
}

using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Items/Create new recovery item")]

public class RecoveryItem : ItemBase
{
    [Header("Health")]
    public FloatValue maxHealth;
    [SerializeField] private Signal healthSignal;
    [SerializeField] int hpAmount;
    [SerializeField] bool restoreMaxHP;

    [Header("Arrows")]
    [SerializeField] int arrowAmount;
    [SerializeField] bool restoreMaxArrows;

    [Header("Magic")]
    [SerializeField] int magicAmount;
    [SerializeField] bool restoreMaxMagic;

    // revisar video #57 en caso de aplicar poison, o alentar, etc
    /*[Header("Status Conditions")]
    [SerializeField] ConditionID status;
    [SerializeField] bool recoverAllStatus;*/

    [Header("Revive")]
    [SerializeField] bool revive;
    [SerializeField] bool maxRevive;

    // 231012 cambie el use al de video #57
    /*public override bool Use(GenericHealth player)
    {
        /*if (restoreMaxHealth || healthAmount > 0)
        {
            if (player.currentHealth == maxHealth.RuntimeValue)
                return false;

            if (restoreMaxHealth)
                player.Heal(maxHealth.RuntimeValue);
            else
                player.Heal(healthAmount);
        }
        //thisEvent.Invoke();
        player.Heal((float)hpAmount);
        return true;
    }*/

    public override bool Use(PlayerAttributes player)
    {
        // Revive
        if (revive || maxRevive)
        {
            if (player.HP > 0)
                return false;

            if (revive)
                player.Heal(player.MaxHP / 2);
            else if (maxRevive)
                player.Heal(player.MaxHP);

            // si implemento poison
            // player.CureStatus();
            return true;
        }

        // No other items can be used on fainted players
        if (player.HP == 0)
            return false;

        // RestoreHP
        if (restoreMaxHP || hpAmount > 0)
        {
            if (player.HP == player.MaxHP)
                return false;

            if (restoreMaxHP)
                player.Heal(player.MaxHP);
            else
                player.Heal(hpAmount);
        }
        //thisEvent.Invoke();
        //player.Heal((float)healthAmount);
        //return true;

        // Recover status (video 57)
        /*if (recoverAllStatus || status != ConditionID.none)
        {
            if(player.Status == null && player.VolatileStatus != null)
                return false;

            if (recoverAllStatus)
            {
                player.CureStatus();
                player.CureVolatileStatus();
            }
            else
            {
                if (player.Status.Id == status)
                    player.CureStatus();
                else if (player.VolatileStatus.Id == status)
                    player.ClearVolatileStatus();
                else
                    return false;
            }
        }*/

        // Restore arrows
        if (restoreMaxArrows || arrowAmount > 0)
        {
            if (player.Arrows == player.MaxArrows)
                return false;

            if (restoreMaxArrows)
                player.IncreaseArrows(player.MaxArrows);
            else
                player.IncreaseArrows(arrowAmount);
        }

        // Restore magic
        if (restoreMaxMagic || magicAmount > 0)
        {
            if (player.Gas == player.MaxGas)
                return false;

            if (restoreMaxMagic)
                player.IncreaseGas(player.MaxGas);
            else
                player.IncreaseGas(magicAmount);
        }

        return true;
    }
}

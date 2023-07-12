using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Items/Create new recovery item")]

public class RecoveryItem : ItemBase
{
    public FloatValue maxHealth;

    [Header("Health")]
    [SerializeField] int healthAmount;
    [SerializeField] bool restoreMaxHealth;

    [Header("Magic")]
    [SerializeField] int magicAmount;
    [SerializeField] bool restoreMaxMagic;

    [Header("Arrows")]
    [SerializeField] int arrowAmount;
    [SerializeField] bool restoreMaxArrow;

    // TODO para esto necesito arreglar runtime value y max health
    public override bool Use(PlayerHealth player)
    {
        /*if (restoreMaxHealth || healthAmount > 0)
        {
            if (player.currentHealth == maxHealth.RuntimeValue)
                return false;

            if (restoreMaxHealth)
                player.Heal(maxHealth.RuntimeValue);
            else
                player.Heal(healthAmount);
        }*/
        thisEvent.Invoke();
        return true;
    }

    //Tal vez usar esto para en caso de veneno, o correr mas despacio
    /*[Header("Status Conditions")]
    [SerializeField] ConditionID status;
    [SerializeField] bool recoverAllStatus;*/
}

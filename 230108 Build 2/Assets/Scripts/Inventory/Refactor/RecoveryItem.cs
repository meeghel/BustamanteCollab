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
    [SerializeField] int healthAmount;
    [SerializeField] bool restoreMaxHealth;

    [Header("Magic")]
    [SerializeField] int magicAmount;
    [SerializeField] bool restoreMaxMagic;

    [Header("Arrows")]
    [SerializeField] int arrowAmount;
    [SerializeField] bool restoreMaxArrow;

    // TODO para esto necesito arreglar runtime value y max health
    public override bool Use(GenericHealth player)
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
        //thisEvent.Invoke();
        player.Heal((float)healthAmount);
        return true;
    }

    // TODO Tal vez usar esto para en caso de veneno, o correr mas despacio
    /*[Header("Status Conditions")]
    [SerializeField] ConditionID status;
    [SerializeField] bool recoverAllStatus;*/
}

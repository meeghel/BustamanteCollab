using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new recovery item")]

public class RecoveryItem : ItemBase
{
    [Header("Health")]
    [SerializeField] int healthAmount;
    [SerializeField] bool restoreMaxHealth;

    [Header("Magic")]
    [SerializeField] int magicAmount;
    [SerializeField] bool restoreMaxMagic;

    [Header("Arrows")]
    [SerializeField] int arrowAmount;
    [SerializeField] bool restoreMaxArrow;

    //Tal vez usar esto para en caso de veneno, o correr mas despacio
    /*[Header("Status Conditions")]
    [SerializeField] ConditionID status;
    [SerializeField] bool recoverAllStatus;*/

    //Revisar si funciona, se tropicalizo de video 56-57
    public override bool Use(PlayerHealth thePlayer)
    {
        if (restoreMaxHealth || healthAmount > 0)
        {
            if (thePlayer.currentHealth >= thePlayer.maxHealth.RuntimeValue)
                return false;

            if (restoreMaxHealth)
            {
                thePlayer.currentHealth = thePlayer.maxHealth.RuntimeValue;
            }
            else
            {
                thePlayer.Heal(healthAmount);
            }
        }

        return true;
    }
}

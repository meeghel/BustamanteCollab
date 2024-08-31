using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : GenericHealth
{
    [SerializeField] private Signal healthSignal;
    // 230928 Agregue player controller para refactor
    //PlayerParty playerParty;
    //PlayerController playerController;
    PlayerAttributes player;

    public void Start()
    {
        player = GetComponentInParent<PlayerController>().Player;
    }

    // como mandar Damage al HP de Player?
    public override void Damage(float amountToDamage)
    {
        base.Damage(amountToDamage);
        // TODO resuelto, ya no es necesario
        //maxHealth.RuntimeValue = currentHealth;
        // TODO revisar si es necesario este evento, creo que no
        //OnHealthChanged?.Invoke();
        healthSignal.Raise();
    }

    /*public override void TakeDamage(float amountToDamage)
    {
        base.TakeDamage(amountToDamage);
        // TODO resuelto, ya no es necesario
        //maxHealth.RuntimeValue = currentHealth;
        // TODO revisar si es necesario este evento, creo que no
        //OnHealthChanged?.Invoke();
        healthSignal.Raise();
    }
    */
    public override void Heal(float amountToHeal)
    {
        base.Heal(amountToHeal);
        // TODO resuelto, ya no es necesario
        //maxHealth.RuntimeValue = currentHealth;
        // TODO revisar si es necesario este evento, creo que no
        //OnHealthChanged?.Invoke();
        healthSignal.Raise();
    }
}

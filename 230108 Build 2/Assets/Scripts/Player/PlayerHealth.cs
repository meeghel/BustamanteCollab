using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : GenericHealth
{
    [SerializeField] private Signal healthSignal;

    public override void Damage(float amountToDamage)
    {
        base.Damage(amountToDamage);
        maxHealth.RuntimeValue = currentHealth;
        //OnHealthChanged?.Invoke();
        healthSignal.Raise();
    }

    public override void Heal(float amountToHeal)
    {
        base.Heal(amountToHeal);
        //OnHealthChanged?.Invoke();
        healthSignal.Raise();
    }
}

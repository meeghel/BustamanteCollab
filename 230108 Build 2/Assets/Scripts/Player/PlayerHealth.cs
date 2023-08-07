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
        // TODO resuelto, ya no es necesario
        //maxHealth.RuntimeValue = currentHealth;
        // TODO revisar si es necesario este evento, creo que no
        //OnHealthChanged?.Invoke();
        healthSignal.Raise();
    }

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

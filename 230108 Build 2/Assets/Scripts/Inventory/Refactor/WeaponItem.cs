using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new weapon")]

// TODO necesito crear clase con todos los parametros, incluyendo arma

public class WeaponItem : ItemBase
{
    public override bool Use(GenericHealth player)
    {
        return true;
    }
}

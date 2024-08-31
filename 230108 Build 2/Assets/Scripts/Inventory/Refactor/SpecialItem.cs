using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new special item")]

public class SpecialItem : ItemBase
{
    [SerializeField] bool isSpecialKey;

    public override bool IsReusable => isSpecialKey;

    public bool IsSpecialKey => isSpecialKey;
}

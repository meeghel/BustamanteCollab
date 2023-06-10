using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] Sprite icon;

    public string Name => name;
    public string Description => description;
    public Sprite Icon => icon;

    //Confirmar si es sobre PlayerMovement
    public virtual bool Use(PlayerHealth thePlayer)
    {
        return false;
    }
}

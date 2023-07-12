using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]

public class ItemBase : ScriptableObject
{
    // TODO Tropicalice a sistema anterior, usando eventos en Inspector
    // TODO Falta decrease amount de items, eso va en Inventario

    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    public UnityEvent thisEvent;

    public virtual string Name => name;
    public string Description => description;
    public Sprite Icon => icon;

    public virtual bool Use(PlayerHealth player)
    {
        return false;
    }

    public virtual bool IsReusable => false;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations;

[System.Serializable]
public class ItemBase : ScriptableObject
{
    // TODO Tropicalice a sistema anterior, usando eventos en Inspector
    // TODO Falta decrease amount de items, eso va en Inventario
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] RuntimeAnimatorController animatorController;
    [SerializeField] float price;
    [SerializeField] bool canSell;
    public bool isKey;
    public bool isHeartContainer;

    public RuntimeAnimatorController AnimatorController => animatorController;


    public UnityEvent thisEvent;

    public virtual string Name => name;
    public string Description => description;
    public Sprite Icon => icon;

    public float Price => price;
    public bool CanSell => canSell;

    // 231012 cambie a que no tome un GenericHealth, sino PlayerAttributes
    public virtual bool Use(PlayerAttributes player)
    {
        return false;
    }

    public virtual bool IsReusable => false;
}

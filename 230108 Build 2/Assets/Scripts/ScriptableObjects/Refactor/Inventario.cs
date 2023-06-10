using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    [SerializeField] List<ItemSlot> slots;

    public List<ItemSlot> Slots => slots;

    //Revisar este codigo porque esta basado en inventory UI por renglon (slots)
    public void UseItem(int itemIndex, PlayerHealth thePlayer)
    {
        var item = slots[itemIndex].Item;
        bool itemUsed = item.Use(thePlayer);
    }

    public void AddItem(ItemBase item, int count = 1)
    {
        /*Agregar funciones GetCategoryFromItem y GetSlotsByCategory
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.FirstOrDefault(slots => slots.Item == item);
        if (itemSlot != null)
        {
            itemSlot.Count += count;
        }
        else
        {
            currentSlots.Add(new ItemSlot()
            {
                Item = item,
                Count = count
            });
        }*/
    }

    public static PlayerInventory GetInventory()
    {
        return FindObjectOfType<PlayerMovement>().GetComponent<PlayerInventory>();
    }
}

[Serializable]

public class ItemSlot
{
    [SerializeField] ItemBase item;
    [SerializeField] int count;

    public ItemBase Item
    {
        get => Item;
        set => item = value;
    }

    public int Count
    {
        get => count;
        set => count = value;
    }
}

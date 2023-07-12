using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ItemCategory { Items, Armas, Especial }

public class Inventario : MonoBehaviour
{
    [SerializeField] List<ItemSlot> slots;
    [SerializeField] List<ItemSlot> weaponSlots;
    [SerializeField] List<ItemSlot> specialSlots;

    List<List<ItemSlot>> allSlots;

    public event Action OnUpdated;

    private void Awake()
    {
        allSlots = new List<List<ItemSlot>>() { slots, weaponSlots, specialSlots };
    }

    // TODO Unity no me dejo hacerlo static
    public List<string> ItemCategories { get; set; } = new List<string>()
    {
        "ITEMS", "ARMAS", "ESPECIAL"
    };

    public List<ItemSlot> GetSlotsByCategory(int categoryIndex)
    {
        return allSlots[categoryIndex];
    }

    public ItemBase GetItem(int itemIndex, int categoryIndex)
    {
        var currentSlots = GetSlotsByCategory(categoryIndex);
        return currentSlots[itemIndex].Item;
    }

    //Revisar este codigo porque esta basado en inventory UI por renglon (slots)
    // TODO revisar crear clase player para meter health, magic etc (como "Pokemon")
    public ItemBase UseItem(int itemIndex, PlayerHealth player, int selectedCategory)
    {
        var item = GetItem(itemIndex, selectedCategory);
        bool itemUsed = item.Use(player);

        /*if (item is WeaponItem)
        {
            // TODO implementar otro tipo de items
        }*/

        if (itemUsed)
        {
            if (!item.IsReusable)
                RemoveItem(item, selectedCategory);

            return item;
        }

        return null;
    }

    public void AddItem(ItemBase item, int count = 1)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.FirstOrDefault(slot => slot.Item == item);
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
        }

        OnUpdated?.Invoke();
    }

    public void RemoveItem(ItemBase item, int category)
    {
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.First(slot => slot.Item == item);
        itemSlot.Count--;
        if (itemSlot.Count == 0)
            currentSlots.Remove(itemSlot);

        OnUpdated?.Invoke();
    }

    ItemCategory GetCategoryFromItem(ItemBase item)
    {
        if (item is RecoveryItem)
            return ItemCategory.Items;
        else if (item is WeaponItem)
            return ItemCategory.Armas;
        else
            return ItemCategory.Especial;
    }

    public static Inventario GetInventory()
    {
        return FindObjectOfType<PlayerController>().GetComponent<Inventario>();
        // TODO quite public static "PlayerInventory" y lo hice como video #52 23:30
        //return FindObjectOfType<PlayerMovement>().GetComponent<PlayerInventory>();
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

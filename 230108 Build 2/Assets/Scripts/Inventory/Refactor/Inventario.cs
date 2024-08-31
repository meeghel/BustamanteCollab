using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEditor.Progress;
using Ink.Parsed;
using TreeEditor;

public enum ItemCategory { Items, Armas, Especial }

public class Inventario : MonoBehaviour
{
    // TODO Revisar mover a otro lado
    //[Header("Magic")]
    public int currentKeys;
    public float maxGas = 10;
    public float currentGas;

    [SerializeField] List<ItemSlot> slots;
    [SerializeField] List<ItemSlot> weaponSlots;
    [SerializeField] List<ItemSlot> specialSlots;

    List<List<ItemSlot>> allSlots;

    [SerializeField] List<CharacterSlot> characterSlots;

    public event Action OnUpdated;

    private void Awake()
    {
        allSlots = new List<List<ItemSlot>>() { slots, weaponSlots, specialSlots };
    }

    // 231013 Revisar cambiar a propiedad de cada personaje
    public void OnEnable()
    {
        currentGas = maxGas;
    }

    public void ReduceGas(float gasCost)
    {
        currentGas -= gasCost;
    }

    public static List<string> ItemCategories { get; set; } = new List<string>()
    {
        "ITEMS", "ARMAS", "ESPECIAL"
    };

    public List<CharacterSlot> GetCharacterSlots() { return characterSlots; }

    public List<ItemSlot> GetWeaponSlots() { return weaponSlots; }

    public PlayerAttributesBase GetCharacter(int characterIndex)
    {
        var currentSlots = GetCharacterSlots();
        return currentSlots[characterIndex].Character;
    }

    public List<ItemSlot> GetSlotsByCategory(int categoryIndex)
    {
        //Debug.Log($"Category Index = {categoryIndex}");
        return allSlots[categoryIndex];
    }

    public List<ItemSlot> GetSlotsByWeaponType(WeaponType weaponType)
    {
        var currentSlots = GetWeaponSlots();

        return currentSlots.Where(slot => (slot.Item as WeaponItem) != null && (slot.Item as WeaponItem).WeaponType == weaponType).ToList();
    }

    public ItemBase GetItem(int itemIndex, int categoryIndex)
    {
        var currentSlots = GetSlotsByCategory(categoryIndex);
        return currentSlots[itemIndex].Item;
    }

    public WeaponItem GetWeapon(int itemIndex, WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Melee:
                List<ItemSlot> meleeSlots = GetWeaponSlots().Where(weaponSlot => weaponSlot.WeaponType == WeaponType.Melee).ToList();
                return meleeSlots[itemIndex].Item as WeaponItem;
            case WeaponType.Ranged:
                List<ItemSlot> rangedSlots = GetWeaponSlots().Where(weaponSlot => weaponSlot.WeaponType == WeaponType.Ranged).ToList();
                return rangedSlots[itemIndex].Item as WeaponItem;
            case WeaponType.Particle:
                List<ItemSlot> particleSlots = GetWeaponSlots().Where(weaponSlot => weaponSlot.WeaponType == WeaponType.Particle).ToList();
                return particleSlots[itemIndex].Item as WeaponItem;
            case WeaponType.Special:
                List<ItemSlot> specialSlots = GetWeaponSlots().Where(weaponSlot => weaponSlot.WeaponType == WeaponType.Special).ToList();
                return specialSlots[itemIndex].Item as WeaponItem;
        }
        return null;
    }

    //Revisar este codigo porque esta basado en inventory UI por renglon (slots)
    // TODO revisar crear clase player para meter health, magic etc (como "Pokemon")
    public ItemBase UseItem(int itemIndex, PlayerAttributes player, int selectedCategory)
    {
        var item = GetItem(itemIndex, selectedCategory);
        bool itemUsed = item.Use(player);

        // TODO implementar otro tipo de items
        /*if (item is WeaponItem)
        {
            
        }*/

        if (itemUsed)
        {
            if (!item.IsReusable)
                RemoveItem(item);

            return item;
        }

        return null;
    }

    public void AddItem(ItemBase item, int count=1)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        if (item.isKey)
        {
            currentKeys++;
        }
        else
        {
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
        }

        OnUpdated?.Invoke();
    }

    public int GetItemCount(ItemBase item)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.FirstOrDefault(slot => slot.Item == item);

        if (itemSlot != null)
            return itemSlot.Count;
        else
            return 0;
    }

    public void RemoveItem(ItemBase item, int countToRemove=1)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.First(slot => slot.Item == item);
        itemSlot.Count -= countToRemove;
        if (itemSlot.Count == 0)
            currentSlots.Remove(itemSlot);

        OnUpdated?.Invoke();
    }

    public bool HasItem(ItemBase item)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        return currentSlots.Exists(slot => slot.Item == item);
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
    WeaponType weaponType;

    public ItemBase Item
    {
        get => item;
        set => item = value;
    }

    public int Count
    {
        get => count;
        set => count = value;
    }

    public WeaponType WeaponType
    {
        get => (Item as WeaponItem).WeaponType;
        //set => (Item as WeaponItem).WeaponType = value;
    }
}

[Serializable]
public class CharacterSlot
{
    [SerializeField] PlayerAttributesBase character;

    public PlayerAttributesBase Character
    {
        get => character;
        set => character = value;
    }
}
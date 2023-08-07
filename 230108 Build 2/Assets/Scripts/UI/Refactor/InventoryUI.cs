using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryUIState { ItemSelection, Busy }

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;

    [SerializeField] Text categoryText;
    [SerializeField] Image itemIcon;
    [SerializeField] Text itemDescription;

    [SerializeField] Image upArrow;
    [SerializeField] Image downArrow;

    Action<ItemBase> onItemUsed;

    int selectedItem = 0;
    int selectedCategory = 0;

    InventoryUIState state;

    // cuantos items hay en viewport de lista, ajustar si lo cambio
    const int itemsInViewport = 8;

    List<ItemSlotUI> slotUIList;
    Inventario inventario;
    public GenericHealth player;
    RectTransform itemListRect;

    private void Awake()
    {
        inventario = Inventario.GetInventory();
        itemListRect = itemList.GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateItemList();

        inventario.OnUpdated += UpdateItemList;
    }

    void UpdateItemList()
    {
        // Clear all the existing items
        foreach (Transform child in itemList.transform)
            Destroy(child.gameObject);

        slotUIList = new List<ItemSlotUI>();
        foreach (var itemSlot in inventario.GetSlotsByCategory(selectedCategory))
        {
            var slotUIObj = Instantiate(itemSlotUI, itemList.transform);
            slotUIObj.SetData(itemSlot);

            slotUIList.Add(slotUIObj);
        }

        UpdateItemSelection();
    }

    public void HandleUpdate(Action onBack, Action<ItemBase> onItemUsed=null)
    {
        this.onItemUsed = onItemUsed;

        if (state == InventoryUIState.ItemSelection)
        {
            int prevSelection = selectedItem;
            int prevCategory = selectedCategory;

            if (Input.GetKeyDown(KeyCode.DownArrow))
                ++selectedItem;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                --selectedItem;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                ++selectedCategory;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                --selectedCategory;

            if (selectedCategory > Inventario.ItemCategories.Count - 1)
                selectedCategory = 0;
            else if (selectedCategory < 0)
                selectedCategory = Inventario.ItemCategories.Count - 1;

            selectedItem = Mathf.Clamp(selectedItem, 0, inventario.GetSlotsByCategory(selectedCategory).Count - 1);

            if (prevCategory != selectedCategory)
            {
                ResetSelection();
                categoryText.text = Inventario.ItemCategories[selectedCategory];
                UpdateItemList();
            }
            else if (prevSelection != selectedItem)
            {
                UpdateItemSelection();
            }

            if (Input.GetKeyDown(KeyCode.Z))
                // TODO revisar, agregue coroutine en video 62, 9:00
                StartCoroutine(ItemSelected());
            else if (Input.GetKeyDown(KeyCode.X))
                onBack?.Invoke();
        }
    }

    // TODO revisar en video #62, 7:00 lo hace IEnumerator; implementar cambio de armas, uso de especiales
    IEnumerator ItemSelected()
    {
        state = InventoryUIState.Busy;

        var item = inventario.GetItem(selectedItem, selectedCategory);

        if (GameController.Instance.State == GameState.Shop)
        {
            onItemUsed?.Invoke(item);
            state = InventoryUIState.ItemSelection;
            yield break;
        }

        if (selectedCategory == (int)ItemCategory.Armas)
        {
            // TODO no hay problema en dejar vacio (Robert)
        }
        else
        {
            inventario.UseItem(selectedItem, player, selectedCategory);
            StartCoroutine(UseItem());
        }
    }

    // TODO no estoy usando IEnumerator/UseItem (video #59, 3:35), sirve para dialogos
    // revisar, solo se puede usar una vez, y no funciona texto de No tendria efecto
    // revisar video 62, quita parametros true/false de DMRef
    IEnumerator UseItem()
    {
        state = InventoryUIState.Busy;

        yield return HandleSpecialItems();

        var usedItem = inventario.UseItem(selectedItem, player, selectedCategory);
        if (usedItem != null)
        {
            if(usedItem is RecoveryItem)
                yield return DialogManagerRef.instance.ShowDialogText($"Has usado {usedItem.Name.ToUpper()}", true, false);

            onItemUsed?.Invoke(usedItem);
        }
        else
        {
            if(selectedCategory == (int)ItemCategory.Items)
                yield return DialogManagerRef.instance.ShowDialogText($"¡No tendria efecto!", true, false);
        }

        DialogManagerRef.instance.CloseDialog();

        state = InventoryUIState.ItemSelection;
    }

    IEnumerator HandleSpecialItems()
    {
        var specialItem = inventario.GetItem(selectedItem, selectedCategory) as SpecialItem;
        if (specialItem == null)
            yield break;
        // TODO implementar accion de especiales
    }

    void UpdateItemSelection()
    {
        var slots = inventario.GetSlotsByCategory(selectedCategory);

        selectedItem = Mathf.Clamp(selectedItem, 0, slots.Count - 1);

        for (int i = 0; i < slotUIList.Count; i++)
        {
            if (i == selectedItem)
                slotUIList[i].NameText.color = GlobalSettings.i.HighlightedColor;
            else
                slotUIList[i].NameText.color = Color.black;
        }

        if (slots.Count > 0)
        {
            var item = slots[selectedItem].Item;
            itemIcon.sprite = item.Icon;
            itemDescription.text = item.Description;
        }

        HandleScrolling();
    }

    void HandleScrolling()
    {
        if (slotUIList.Count <= itemsInViewport) return;

        // Multiply the selectedItem by the height of the Item slot
        float scrollPos = Mathf.Clamp(selectedItem - itemsInViewport/2, 0, selectedItem) * slotUIList[0].Height;
        itemListRect.localPosition = new Vector2(itemListRect.localPosition.x, scrollPos);

        bool showUpArrow = selectedItem > itemsInViewport / 2;
        upArrow.gameObject.SetActive(showUpArrow);

        bool showDownArrow = selectedItem + itemsInViewport / 2 < slotUIList.Count;
        downArrow.gameObject.SetActive(showDownArrow);
    }

    void ResetSelection()
    {
        selectedItem = 0;

        upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(false);

        itemIcon.sprite = null;
        itemDescription.text = "";
    }
}

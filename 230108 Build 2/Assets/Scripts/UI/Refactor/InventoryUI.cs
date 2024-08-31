using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryUIState { ItemSelection, PartyScreen, Busy }

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;
    [SerializeField] PlayerController playerController;
    [SerializeField] PartyScreen partyScreen;

    [SerializeField] Text categoryText;
    [SerializeField] Image itemIcon;
    [SerializeField] Text itemDescription;
    [SerializeField] Text weaponDamage;
    [SerializeField] Text weaponType;

    [SerializeField] GameObject characterList;
    [SerializeField] CharacterSlotUI characterSlotUI;

    //[SerializeField] Text nameText;
    [SerializeField] Image characterIcon;
    [SerializeField] Text characterDescription;
    [SerializeField] Text characterAtk;
    [SerializeField] Text characterDef;
    [SerializeField] Text characterSpd;

    [SerializeField] Image upArrow;
    [SerializeField] Image downArrow;

    Action<ItemBase> onItemUsed;

    int selectedItem = 0;
    int currentMember = 0;
    int selectedCategory = 0;

    InventoryUIState state;

    // cuantos items hay en viewport de lista, ajustar si lo cambio
    const int itemsInViewport = 8;

    List<ItemSlotUI> slotUIList;
    List<CharacterSlotUI> charSlotUIList;
    Inventario inventario;
    public PlayerAttributes player;
    RectTransform itemListRect;
    RectTransform characterListRect;
    PlayerParty playerParty;

    private void Awake()
    {
        inventario = Inventario.GetInventory();
        playerParty = GetPlayerParty();
        itemListRect = itemList.GetComponent<RectTransform>();
        characterListRect = characterList.GetComponent<RectTransform>();
    }

    private void Start()
    {
        ClearObjectsFromScreen();

        UpdateItemList();
        
        partyScreen.Init();
        //UpdateCharacterList();

        inventario.OnUpdated += UpdateItemList;
        //inventario.OnUpdated += UpdateCharacterList;
    }

    void ClearObjectsFromScreen()
    {
        weaponDamage.gameObject.SetActive(false);
        weaponType.gameObject.SetActive(false);
        characterList.gameObject.SetActive(false);
        characterIcon.gameObject.SetActive(false);
        characterDescription.gameObject.SetActive(false);
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

    void UpdateCharacterList()
    {
        // Clear all the existing items
        foreach (Transform child in characterList.transform)
            Destroy(child.gameObject);

        charSlotUIList = new List<CharacterSlotUI>();
        foreach (var characterSlot in inventario.GetCharacterSlots())
        {
            var charSlotUIObj = Instantiate(characterSlotUI, characterList.transform);
            charSlotUIObj.SetData(characterSlot);

            charSlotUIList.Add(charSlotUIObj);
        }

        UpdateCharacterSelection();
    }

    public void HandleUpdate(Action onBack, Action<ItemBase> onItemUsed=null)
    {
        this.onItemUsed = onItemUsed;

        if (state == InventoryUIState.PartyScreen)
        {
            HandlePartySelection();
        }

        if (state == InventoryUIState.ItemSelection)
        {
            int prevSelection = selectedItem;
            int prevCategory = selectedCategory;
            int prevCharacter = currentMember;

            if (selectedCategory is 3)
            {
                OpenPartyScreen();
                /*itemList.gameObject.SetActive(false);
                itemIcon.gameObject.SetActive(false);
                itemDescription.gameObject.SetActive(false);
                characterList.gameObject.SetActive(true);
                characterIcon.gameObject.SetActive(true);
                characterDescription.gameObject.SetActive(true);
                characterAtk.gameObject.SetActive(true);
                characterDef.gameObject.SetActive(true);
                characterSpd.gameObject.SetActive(true);
                UpdateCharacterList();
                if (Input.GetKeyDown(KeyCode.DownArrow))
                    ++selectedCharacter;
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                    --selectedCharacter;
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    ++selectedCategory;
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    --selectedCategory;*/
            }
            else
            {
                itemList.gameObject.SetActive(true);
                itemIcon.gameObject.SetActive(true);
                itemDescription.gameObject.SetActive(true);
                characterList.gameObject.SetActive(false);
                characterIcon.gameObject.SetActive(false);
                characterDescription.gameObject.SetActive(false);
                characterAtk.gameObject.SetActive(false);
                characterDef.gameObject.SetActive(false);
                characterSpd.gameObject.SetActive(false);
                UpdateItemList();
                if (Input.GetKeyDown(KeyCode.DownArrow))
                    ++selectedItem;
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                    --selectedItem;
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    ++selectedCategory;
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    --selectedCategory;
            }

            if (selectedCategory > Inventario.ItemCategories.Count - 1)
                selectedCategory = 0;
            else if (selectedCategory < 0)
                selectedCategory = Inventario.ItemCategories.Count - 1;

            //selectedItem = Mathf.Clamp(selectedItem, 0, inventario.GetSlotsByCategory(selectedCategory).Count - 1);
            currentMember = Mathf.Clamp(currentMember, 0, inventario.GetCharacterSlots().Count - 1);

            if (prevCategory != selectedCategory)
            {
                ResetSelection();
                categoryText.text = Inventario.ItemCategories[selectedCategory];
                if (selectedCategory is 3)
                {
                    OpenPartyScreen();
                    /*UpdateCharacterList();
                    if (prevCharacter != selectedCharacter)
                    {
                        UpdateCharacterSelection();
                    }*/
                }
                else
                {
                    UpdateItemList();
                }
            }
            else if (prevSelection != selectedItem)
            {
                UpdateItemSelection();
            }

            if (selectedCategory is 1)
            {
                weaponDamage.gameObject.SetActive(true);
                weaponType.gameObject.SetActive(true);
            }
            else
            {
                weaponDamage.gameObject.SetActive(false);
                weaponType.gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Z))
                // TODO revisar, agregue coroutine en video 62, 9:00
                StartCoroutine(ItemSelected());
            else if (Input.GetKeyDown(KeyCode.X))
                onBack?.Invoke();
        }
    }

    void HandlePartySelection()
    {
        int prevCategory = selectedCategory;
        int prevCharacter = currentMember;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            ++currentMember;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            --currentMember;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            currentMember += 2;
        //++selectedCategory;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentMember -= 2;
            //--selectedCategory;

            /*if (selectedCategory > Inventario.ItemCategories.Count - 1)
                selectedCategory = 0;
            else if (selectedCategory < 0)
                selectedCategory = Inventario.ItemCategories.Count - 1;*/

        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Characters.Count - 1);

        //partyScreen.UpdateMemberSelection(currentMember);

        /*if (prevCategory != selectedCategory)
        {
            ResetSelection();
            categoryText.text = Inventario.ItemCategories[selectedCategory];
            if (selectedCategory is 3)
            {
                OpenPartyScreen();
            }
            else
            {
                UpdateItemList();
            }
        }*/

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty.Characters[currentMember];
            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("¡No puedes usar ese personaje!");
                return;
            }
            if (selectedMember == playerController.Player)
            {
                partyScreen.SetMessageText("¡Es el mismo personaje!");
                return;
            }

            // revisar orden si asi vuelvo al juego?
            partyScreen.gameObject.SetActive(false);
            state = InventoryUIState.Busy;
            StartCoroutine(SwitchCharacter(selectedMember));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);
        }
    }

    IEnumerator SwitchCharacter(PlayerAttributes newCharacter)
    {
        GameController.Instance.PauseGame(true);
        yield return Fader.i.FadeIn(0.5f);

        playerController.SetupPlayer(newCharacter);
        //SetData? ya se corrio en SetupPlayer

        yield return Fader.i.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);
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
        // TODO implementar accion de especiales y accion de armas (hacer arma seleccionada)
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
            if (item is WeaponItem)
            {
                WeaponItem weaponItem = item as WeaponItem;
                weaponDamage.text = $"Ataque: {weaponItem.Damage}";
                weaponType.text = $"Tipo: {weaponItem.WeaponType}";
            }
        }

        HandleScrolling();
    }

    void UpdateCharacterSelection()
    {
        var charSlots = inventario.GetCharacterSlots();

        currentMember = Mathf.Clamp(currentMember, 0, charSlots.Count - 1);

        for (int i = 0; i < charSlotUIList.Count; i++)
        {
            if (i == currentMember)
                charSlotUIList[i].NameText.color = GlobalSettings.i.HighlightedColor;
            else
                charSlotUIList[i].NameText.color = Color.black;
        }

        if (charSlots.Count > 0)
        {
            var character = charSlots[currentMember].Character;
            characterIcon.sprite = character.IconSprite;
            characterDescription.text = character.Description;
            characterAtk.text = $"Atq: {character.Attack}";
            characterDef.text = $"Def: {character.Defense}";
            characterSpd.text = $"Vel: {character.Speed}";
        }

        HandleCharacterScrolling();
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

    void HandleCharacterScrolling()
    {
        if (charSlotUIList.Count <= itemsInViewport) return;

        // Multiply the selectedItem by the height of the Item slot
        float scrollPos = Mathf.Clamp(currentMember - itemsInViewport / 2, 0, currentMember) * charSlotUIList[0].Height;
        characterListRect.localPosition = new Vector2(characterListRect.localPosition.x, scrollPos);

        bool showUpArrow = currentMember > itemsInViewport / 2;
        upArrow.gameObject.SetActive(showUpArrow);

        bool showDownArrow = currentMember + itemsInViewport / 2 < charSlotUIList.Count;
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

    public static PlayerParty GetPlayerParty()
    {
        return FindObjectOfType<PlayerController>().GetComponent<PlayerParty>();
    }

    public void OpenPartyScreen()
    {
        state = InventoryUIState.PartyScreen;
        //playerParty = GetPlayerParty();
        partyScreen.Init();
        //partyScreen.SetPartyData(playerParty.Characters);
        partyScreen.gameObject.SetActive(true);
    }
}

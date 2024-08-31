using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopState { Menu, Buying, Selling, Busy }

public class ShopController : MonoBehaviour
{
    [SerializeField] Vector2 shopCameraOffset;
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] ShopUI shopUI;
    [SerializeField] CoinTextManager coinTextManager;
    [SerializeField] CountSelectorUI countSelectorUI;

    public event Action OnStart;
    public event Action OnFinish;

    ShopState state;
    Inventario inventario;
    Merchant merchant;

    public static ShopController i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        inventario = Inventario.GetInventory();
    }

    public IEnumerator StartTrading(Merchant merchant)
    {
        this.merchant = merchant;

        OnStart?.Invoke();
        yield return StartMenuState();
    }

    IEnumerator StartMenuState()
    {
        state = ShopState.Menu;

        int selectedChoice = 0;
        yield return DialogManagerRef.instance.ShowDialogText("¿En que te puedo ayudar?",
            waitForInput: false,
            choices: new List<string>() { "Comprar", "Vender", "Salir" },
            onChoiceSelected: choiceIndex => selectedChoice = choiceIndex );

        if (selectedChoice == 0)
        {
            // Comprar
            yield return GameController.Instance.MoveCamera(shopCameraOffset);
            shopUI.Show(merchant.AvailableItems, (item) => StartCoroutine(BuyItem(item)), 
                () => StartCoroutine(OnBackFromBuying()));

            state = ShopState.Buying;
        }
        else if (selectedChoice == 1)
        {
            // Vender
            state = ShopState.Selling;
            inventoryUI.gameObject.SetActive(true);
        }
        else if (selectedChoice == 2)
        {
            // Salir
            yield return DialogManagerRef.instance.ShowDialogText("¡Vuelve pronto!");
            OnFinish?.Invoke();
            yield break;
        }
    }

    public void HandleUpdate()
    {
        if (state == ShopState.Selling)
        {
            inventoryUI.HandleUpdate(OnBackFromSelling, (selectedItem) => StartCoroutine(SellItem(selectedItem)));
        }
        else if (state == ShopState.Buying)
        {
            shopUI.HandleUpdate();
        }
    }

    void OnBackFromSelling()
    {
        inventoryUI.gameObject.SetActive(false);
        StartCoroutine(StartMenuState());
    }

    IEnumerator SellItem(ItemBase item)
    {
        state = ShopState.Busy;

        if (!item.CanSell)
        {
            yield return DialogManagerRef.instance.ShowDialogText("¡No lo puedes vender!");
            state = ShopState.Selling;
            yield break;
        }

        float sellingPrice = Mathf.Round(item.Price / 2);
        int countToSell = 1;

        int itemCount = inventario.GetItemCount(item);
        if (itemCount > 1)
        {
            yield return DialogManagerRef.instance.ShowDialogText("¿Cuantas me quieres vender?",
                waitForInput: false, autoClose: false);

            yield return countSelectorUI.ShowSelector(itemCount, sellingPrice,
                (selectedCount) => countToSell = selectedCount);

            DialogManagerRef.instance.CloseDialog();
        }

        sellingPrice = sellingPrice * countToSell;

        int selectedChoice = 0;
        yield return DialogManagerRef.instance.ShowDialogText($"Te doy ${sellingPrice} por eso. ¿Lo quieres vender?",
            waitForInput: false,
            choices: new List<string>() { "Si", "No" },
            onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            // Si
            inventario.RemoveItem(item, countToSell);
            Wallet.i.AddMoney(sellingPrice);
            yield return DialogManagerRef.instance.ShowDialogText($"Vendiste {item.Name} y recibiste ${sellingPrice}.");
        }

        state = ShopState.Selling;
    }

    IEnumerator BuyItem(ItemBase item)
    {
        state = ShopState.Busy;

        yield return DialogManagerRef.instance.ShowDialogText("¿Cuantos quieres comprar?",
            waitForInput: false, autoClose: false);

        int countToBuy = 1;
        yield return countSelectorUI.ShowSelector(99, item.Price,
            (selectedCount) => countToBuy = selectedCount);

        DialogManagerRef.instance.CloseDialog();

        float totalPrice = item.Price * countToBuy;

        if (Wallet.i.HasMoney(totalPrice))
        {
            int selectedChoice = 0;
            yield return DialogManagerRef.instance.ShowDialogText($"Ok, serian ${totalPrice}.",
                waitForInput: false,
                choices: new List<string>() { "Si", "No" },
                onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

            if (selectedChoice == 0)
            {
                // Si
                inventario.AddItem(item, countToBuy);
                Wallet.i.TakeMoney(totalPrice);
                yield return DialogManagerRef.instance.ShowDialogText("¡Muchas gracias!");
            }
        }
        else
        {
            yield return DialogManagerRef.instance.ShowDialogText("¡No tienes suficiente dinero!");
        }

        state = ShopState.Buying;
    }

    IEnumerator OnBackFromBuying()
    {
        yield return GameController.Instance.MoveCamera(-shopCameraOffset);
        shopUI.Close();
        StartCoroutine(StartMenuState());
    }
}

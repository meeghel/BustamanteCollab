using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinTextManager : MonoBehaviour
{
    public Inventory playerInventory;
    public TextMeshProUGUI coinDisplay;

    private void Start()
    {
        UpdateCoinCount();
        Wallet.i.OnMoneyChanged += UpdateCoinCount;
    }

    public void UpdateCoinCount()
    {
        coinDisplay.text = "" + Wallet.i.Money;
        //coinDisplay.text = "" + playerInventory.coins;
    }

}

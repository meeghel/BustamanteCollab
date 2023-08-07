using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PowerUp
{
    public Inventory playerInventory;
    [SerializeField] int amount = 1;

    // Start is called before the first frame update
    void Start()
    {
        powerupSignal.Raise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            Wallet.i.AddMoney((float)amount);
            //playerInventory.coins += amount;
            powerupSignal.Raise();
            AudioManager.i.PlaySfx(AudioId.Coin);
            Destroy(this.gameObject);
        }
    }
}

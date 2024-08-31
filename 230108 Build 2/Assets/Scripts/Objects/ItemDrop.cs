using UnityEngine;
using System;
using static UnityEditor.Progress;

[RequireComponent(typeof(Collider2D))]
public class ItemDrop : MonoBehaviour
{
    public ItemType itemType;
    [SerializeField] private string otherTag;

    [SerializeField] private int amount;
    [SerializeField] bool restoreMax;

    float amplitude = 0.2f;
    float frequency = 1f;

    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    private void Start()
    {
        posOffset = transform.position;
    }

    private void Update()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            // poner state Busy?
            if (otherTag == "Player")
            {
                PlayerController player = other.GetComponentInParent<PlayerController>();
                switch (itemType)
                {
                    case ItemType.Coin:
                        Wallet.i.AddMoney(amount);
                        AudioManager.i.PlaySfx(AudioId.Coin);
                        break;
                    case ItemType.Arrow:
                        if (restoreMax)
                            player.Player.FullArrows();
                        else
                            player.Player.IncreaseArrows(amount);
                        player.UpdateHP();
                        AudioManager.i.PlaySfx(AudioId.ItemObtained);
                        break;
                    case ItemType.Gas:
                        if(restoreMax)
                            player.Player.FullGas();
                        else
                            player.Player.IncreaseGas(amount);
                        player.UpdateHP();
                        AudioManager.i.PlaySfx(AudioId.ItemObtained);
                        break;
                    case ItemType.HP:
                        if (restoreMax)
                            player.Player.FullHeal();
                        else
                            player.Player.Heal(amount);
                        player.UpdateHP();
                        AudioManager.i.PlaySfx(AudioId.ItemObtained);
                        break;
                }
                Destroy(this.gameObject);
            }
        }
    }
}

public enum ItemType { Coin, Arrow, Gas, HP }

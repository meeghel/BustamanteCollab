using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, Interactuable
{
    [SerializeField] ItemBase item;

    public bool Used { get; set; } = false;

    public IEnumerator Interact(Transform initiator)
    {
        if (!Used)
        {
            if (item.isHeartContainer)
            {
                PlayerController player = initiator.GetComponent<PlayerController>();
                player.Player.IncreaseHearts();
                player.Player.FullHeal();
                player.UpdateHP();
            }
            else
            {
                initiator.GetComponent<Inventario>().AddItem(item);
            }

            Used = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            string playerName = initiator.GetComponent<PlayerCharacter>().Name;

            //raiseItem.Raise();
            AudioManager.i.PlaySfx(AudioId.ItemObtained, pauseMusic: true);

            yield return initiator.GetComponent<PlayerCharacter>().RaiseItem(item);

            // Confirmar si es necesario dialogo
            yield return DialogManagerRef.instance.ShowDialogText($"¡{playerName} encontro {item.Name}!");

            yield return initiator.GetComponent <PlayerCharacter>().LowerItem();

            Destroy(this.gameObject);
        }
    }
}

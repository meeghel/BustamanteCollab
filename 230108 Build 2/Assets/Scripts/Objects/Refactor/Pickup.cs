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
            initiator.GetComponent<Inventario>().AddItem(item);

            Used = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            // Confirmar si es necesario dialogo
            yield return DialogManagerRef.instance.ShowDialogText($"¡Encontraste {item.Name}!");
        }
    }
}

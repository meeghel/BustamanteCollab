using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour//, Interactuable
{
    //ver video #64 Adding Pickups
    [SerializeField] ItemBase item;
    //Aqui se fue a script Inventory y creo AddItem y GetCategoryFromItem (5:30)
    public bool Used { get; set; } = false;
    //Revisar script Inventory public ItemBase Item get set (10:15)

    //Se agrego Initiator a Interactuable, falta hacer coroutine? para agregar IEnumerator
    /*public void IEnumerator Interact(Transform initiator)
    {
        if (!Used)
        {
            initiator.GetComponent<Inventario>().AddItem(item);

            Used = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;*/

            /*Dialog Manager does not contain a definition for instance
             * Revisar que funcion llamar de DialogManager para que arranque dialogo           
            yield return DialogManager.instance.ShowDialogText($"Encontraste {item.Name}.");
        }
    }*/
}

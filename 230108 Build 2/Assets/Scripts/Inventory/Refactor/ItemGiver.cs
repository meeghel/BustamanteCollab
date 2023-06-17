using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] ItemBase item;
    [SerializeField] Dialog dialog;

    bool used = false;

    /*public IEnumerator GiveItem(PlayerMovement player)
    {
        yield return DialogManagerRef.instance.ShowDialog(dialog);

        player.GetComponent<Inventario>().AddItem(item);

        used = true;
        //yield return DialogManagerRef.instance.ShowDialogText($"Recibiste {item.Name}");
    }*/

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] ItemBase item;
    [SerializeField] int count = 1;
    [SerializeField] Dialog dialog;

    [SerializeField] bool used = false;

    public IEnumerator GiveItem(PlayerController player)
    {
        yield return DialogManagerRef.instance.ShowDialog(dialog);

        player.GetComponent<Inventario>().AddItem(item, count);

        used = true;

        AudioManager.i.PlaySfx(AudioId.ItemObtained, pauseMusic: true);

        string dialogText = $"{player.Name} recibio {item.Name}.";
        if(count > 1)
            dialogText = $"{player.Name} recibio {count} {item.Name}.";
        // TODO no funciona para plural en español!

        yield return DialogManagerRef.instance.ShowDialogText(dialogText);
    }

    public bool CanBeGiven()
    {
        return item != null && count > 0 && !used;
    }
}
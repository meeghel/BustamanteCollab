using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] ItemBase item;
    [SerializeField] int count = 1;
    [SerializeField] Dialog dialog;

    [SerializeField] bool used = false;

    public IEnumerator GiveItem(PlayerCharacter player)
    {
        yield return DialogManagerRef.instance.ShowDialog(dialog);

        player.GetComponent<Inventario>().AddItem(item, count);

        used = true;

        AudioManager.i.PlaySfx(AudioId.ItemObtained, pauseMusic: true);

        yield return player.RaiseItem(item);

        string dialogText = $"{player.Name} recibio {item.Name}.";
        if(count > 1)
            dialogText = $"{player.Name} recibio {count} {item.Name}.";
        // TODO no funciona para plural en español!

        yield return DialogManagerRef.instance.ShowDialogText(dialogText);

        yield return player.LowerItem();
    }

    public bool CanBeGiven()
    {
        return item != null && count > 0 && !used;
    }
}
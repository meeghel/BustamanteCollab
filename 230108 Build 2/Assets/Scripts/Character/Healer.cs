using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    [SerializeField] private Signal healthSignal;

    public IEnumerator Heal(Transform player, Dialog dialog)
    {
        int selectedChoice = 0;

        yield return DialogManagerRef.instance.ShowDialogText("¿Quieres descansar?",
            choices: new List<string>() { "Si", "No" },
            onChoiceSelected: (choiceIndex) => selectedChoice = choiceIndex );

        if (selectedChoice == 0)
        {
            // Si
            yield return Fader.i.FadeIn(0.5f);

            //player.GetComponentInChildren<GenericHealth>().FullHeal();
            PlayerController playerC = player.GetComponentInParent<PlayerController>();
            playerC.Player.FullHeal();
            playerC.UpdateHP();
            healthSignal.Raise();

            yield return Fader.i.FadeOut(0.5f);
            // TODO los dialogos pueden ser SerializeField para hacerlos diferentes entre NPCs
            yield return DialogManagerRef.instance.ShowDialogText($"Listo, ya te debes de sentir mejor.");
        }
        else if (selectedChoice == 1)
        {
            // No
            yield return DialogManagerRef.instance.ShowDialogText($"Ok, aqui estare cuando lo necesites.");
        }
    }
}

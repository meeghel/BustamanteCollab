using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactuable
{
    [SerializeField] Dialog dialog;

    public void Interact()
    {
        Debug.Log("Interacting with NPC");
        StartCoroutine(DialogManagerRef.instance.ShowDialog(dialog));
    }
}

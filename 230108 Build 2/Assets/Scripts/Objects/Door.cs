using System.Collections;
using UnityEngine;

public enum DoorType
{
    key,
    enemy,
    button,
    specific
}

public class Door : MonoBehaviour, Interactuable
{
    [Header("Context")]
    public Signal Context;
    public string dialog;

    /*[Header("Sign")]
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;*/

    [Header("Door Variables")]
    public DoorType thisDoorType;
    public ItemBase thisDoorKey;
    public bool open = false;
    //public Inventory playerInventory;
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;
    public GameObject contextClue;
    //public BoxCollider2D triggerCollider;

    /*private void Update()
    {
        if (isInteracting)//(Input.GetButtonDown("Check"))
        {
            if (playerInRange && thisDoorType == DoorType.key)
            {
                //Does the player have a key?
                if (playerInventory.numberOfKeys > 0)
                {
                    //Remove a player key
                    playerInventory.numberOfKeys--;
                    //If so, then call the open method
                    Open();
                }
                else if (dialogBox.activeInHierarchy)
                {
                    dialogBox.SetActive(false);
                }
                else
                {
                    dialogBox.SetActive(true);
                    dialogText.text = dialog;
                }
            }
            else if (playerInRange && thisDoorType == DoorType.specific)
            {
                //Does the player have the specific key?
                if (playerInventory.CheckForItem(thisDoorKey))
                {
                    Open();
                }
                else if (dialogBox.activeInHierarchy)
                {
                    dialogBox.SetActive(false);
                }
                else
                {
                    dialogBox.SetActive(true);
                    dialogText.text = dialog;
                }
            }
        }
    }*/

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            Context.Raise();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            Context.Raise();
            dialogBox.SetActive(false);
        }
    }*/

    public void Open()
    {
        //Turn off the door's sprite renderer
        doorSprite.enabled = false;
        //Set open to true
        open = true;
        //Turn off the door's box collider
        physicsCollider.enabled = false;
        contextClue.SetActive(false);
        // Conseguir Sfx door
        AudioManager.i.PlaySfx(AudioId.DoorOpen);
    }

    public void Close()
    {
        //Turn off the door's sprite renderer
        doorSprite.enabled = true;
        //Set open to true
        open = false;
        //Turn off the door's box collider
        physicsCollider.enabled = true;
        contextClue.SetActive(true);
        // Conseguir Sfx door
        //AudioManager.i.PlaySfx(AudioId.Door);
    }

    public IEnumerator Interact(Transform initiator)
    {
        switch (thisDoorType)
        {
            case DoorType.key:
                //Does the player have a key?
                if (initiator.GetComponent<Inventario>().currentKeys > 0)
                {
                    //Remove a player key
                    initiator.GetComponent<Inventario>().currentKeys--;
                    //If so, then call the open method
                    Open();
                }
                else
                {
                    if (dialog != null)
                        yield return DialogManagerRef.instance.ShowDialogText(dialog);
                    else
                        yield return DialogManagerRef.instance.ShowDialogText($"¡Necesitas una llave!");
                }
                break;
            case DoorType.specific:
                //Does the player have the specific key?
                if (initiator.GetComponent<Inventario>().HasItem(thisDoorKey))
                {
                    Open();
                }
                else
                {
                    if (dialog != null)
                        yield return DialogManagerRef.instance.ShowDialogText(dialog);
                    else
                        yield return DialogManagerRef.instance.ShowDialogText($"¡Necesitas la llave para esta puerta!");
                }
                break;
            case DoorType.button:
                yield return DialogManagerRef.instance.ShowDialogText($"¡No se puede abrir! ¿Tal vez hay algun boton o palanca cerca?");
                break;
            case DoorType.enemy:
                yield return DialogManagerRef.instance.ShowDialogText($"¡No se puede abrir! ¡Necesitas eliminar a los enemigos de este cuarto!");
                break;
        }
    }
}

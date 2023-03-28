using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DoorType
{
    key,
    enemy,
    button,
    specific
}

public class Door : Interactable
{

    [Header("Sign")]
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;

    [Header("Door Variables")]
    public DoorType thisDoorType;
    public Item thisDoorKey;
    public bool open = false;
    public Inventory playerInventory;
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;
    public BoxCollider2D triggerCollider;

    private void Update()
    {
        if (Input.GetButtonDown("Check"))
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
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            context.Raise();
            playerInRange = false;
            dialogBox.SetActive(false);
        }
    }

    public void Open()
    {
        //Turn off the door's sprite renderer
        doorSprite.enabled = false;
        //Set open to true
        open = true;
        //Turn off the door's box collider
        physicsCollider.enabled = false;
        triggerCollider.enabled = false;
    }

    public void Close()
    {
        //Turn off the door's sprite renderer
        doorSprite.enabled = true;
        //Set open to true
        open = false;
        //Turn off the door's box collider
        physicsCollider.enabled = true;
        triggerCollider.enabled = true;
    }

}

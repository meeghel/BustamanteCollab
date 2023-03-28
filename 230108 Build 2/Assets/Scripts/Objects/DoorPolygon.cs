using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DoorPolygonType
{
    key,
    enemy,
    button
}

public class DoorPolygon : Interactable
{

    [Header("Sign")]
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;

    [Header("Door Variables")]
    public DoorType thisDoorType;
    public bool open = false;
    public Inventory playerInventory;
    public SpriteRenderer doorSprite;
    public PolygonCollider2D physicsCollider;
    public BoxCollider2D triggerCollider;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerInRange && thisDoorType == DoorType.key)
            {
                //Does the player have a key?
                if (playerInventory.numberOfKeys > 0)
                {
                    //Remove a  player key
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

    }

}

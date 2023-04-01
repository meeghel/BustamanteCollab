using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogNPC : Interactable
{
    // Reference to the intermediate dialog value
    [SerializeField] private TextAssetValue dialogValue;
    // Reference to the NPCs dialog
    public TextAsset myDialog;

    // Notification to send to the canvases to activate and check
    //dialog
    [SerializeField] private Notification branchingDialogNotification;
    // Specify Player GameObject
    //[SerializeField] public GameObject player;
    // Sprites for different angles
    //[SerializeField] public SpriteRenderer spriteRenderer;
    //[SerializeField] public Sprite defaultSprite;
    //[SerializeField] public Sprite[] spriteArray;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //spriteRenderer.sprite = defaultSprite;
        if (playerInRange)
        {
            if (Input.GetButtonDown("Check"))
            {
                //ChangeSprite();
                dialogValue.value = myDialog;
                Story myStory = new Story(myDialog.text);
                int noneDialogs = myStory.currentText.Length;
                Debug.Log(noneDialogs);
                int numChoices = myStory.currentChoices.Count;
                Debug.Log(numChoices);
                branchingDialogNotification.Raise();
            }
        }
    }

   /*private void ChangeSprite()
    {
        Vector3 playerPos = player.transform.position;
        if (Mathf.Abs(playerPos.x) > Mathf.Abs(playerPos.y))
        {
            if (playerPos.x > 0)
            {
                spriteRenderer.sprite = spriteArray[1];
            }
            else if (playerPos.x < 0)
            {
                spriteRenderer.sprite = spriteArray[3];
            }
        }
        else if (Mathf.Abs(playerPos.x) < Mathf.Abs(playerPos.y))
        {
            if (playerPos.y > 0)
            {
                spriteRenderer.sprite = spriteArray[2];
            }
            else if (playerPos.y < 0)
            {
                spriteRenderer.sprite = spriteArray[0];
            }
        }
    }*/
}

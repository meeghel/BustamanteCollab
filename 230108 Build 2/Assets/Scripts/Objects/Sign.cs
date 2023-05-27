using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : Interactable
{
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        dialogText.text = dialog;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isInteracting)//Input.GetButtonDown("Check") && playerInRange)
        {
            dialogBox.SetActive(true);
        }
        else
        {
            dialogBox.SetActive(false);
        }

        //Revisar si esto es necesario, solo sería necesario si Player 
        //se puede mover durante diálogo
        if (!playerInRange)
        {
            isInteracting = false;
        }
        /*if (dialogBox.activeInHierarchy)
        {
            dialogBox.SetActive(false);
        }else{
            dialogBox.SetActive(true);
            dialogText.text = dialog;
        }*/
    }
    /*private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            context.Raise();
            playerInRange = false;
            dialogBox.SetActive(false);
        }
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHolder : Interactable
{
    public string dialog;

    public string[] dialogueLines;
    public bool beginDialog = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isInteracting)
        {
            if (beginDialog)
            {
                DialogManager.instance.StartDialog(dialogueLines, 0);
                beginDialog = false;
            }
            else
            {
                DialogManager.instance.ContinueDialog();
            }
        }
        else
        {
            beginDialog = true;
        }
    }

    /*void dialogInstance()
    {
        //dManager.ShowBox(dialog);
        if (!dManager.dialogActive)
        {
            dManager.dialogLines = dialogueLines;
            dManager.currentLine = 0;
            dManager.ShowDialog();
        }
        /*if (isInteracting)//Input.GetButtonDown("Check") && playerInRange)
        {

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
    }*/
}

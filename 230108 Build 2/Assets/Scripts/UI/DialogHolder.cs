using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHolder : Interactable
{
    public string dialog;
    public DialogManager dManager;

    public string[] dialogueLines;

    // Start is called before the first frame update
    void Start()
    {
        dManager = FindObjectOfType<DialogManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteracting)
        {
            dialogInstance();
        }
    }

    void dialogInstance()
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
        }*/

        //Revisar si esto es necesario, solo sería necesario si Player 
        //se puede mover durante diálogo
        if (!playerInRange)
        {
            isInteracting = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dBox;
    public Text dText;

    public bool dialogActive;

    public string[] dialogLines;
    public int currentLine;

    //TODO Revisar refactorizar y hacer un controlador del Player, y no en "Player Movement"
    private PlayerMovement thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        //Revisar si no es FindObjectWithTag, dependerá de refactorizar
        thePlayer = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogActive && Input.GetKeyDown(KeyCode.Space))
        {
            //dialogBox.SetActive(false);
            //dialogActive = false;

            currentLine++;
        }

        if(currentLine >= dialogLines.Length)
        {
            dBox.SetActive(false);
            dialogActive = false;

            currentLine = 0;
            thePlayer.canMove = true;
        }

        dText.text = dialogLines[currentLine];
    }

    //Revisar si esta función tiene uso todavía
    /*public void ShowBox(string dialog)
    {
        dialogActive = true;
        dialogBox.SetActive(true);
        dialogText.text = dialog;
    }*/

    public void ShowDialog()
    {
        dialogActive = true;
        dBox.SetActive(true);
        thePlayer.canMove = false;
    }
}

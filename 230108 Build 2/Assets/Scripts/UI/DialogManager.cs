using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    //eran publicos
    [SerializeField] GameObject dBox;
    [SerializeField] Text dText;
    public Text cancelText;

    public bool dialogActive;

    public string[] dialogLines;
    public int currentLine;

    //TODO Revisar refactorizar y hacer un controlador del Player, y no en "Player Movement"
    private PlayerMovement thePlayer;

    //quitar singleton
    public static DialogManager instance { get; private set; }
    public bool isTalking = false;

    void Awake()
    {
        if (instance != null)
        {
            return;
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Revisar si no es FindObjectWithTag, dependerá de refactorizar
        thePlayer = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Revisar si esta función tiene uso todavía
    /*public void ShowBox(string dialog)
    {
        dialogActive = true;
        dialogBox.SetActive(true);
        dialogText.text = dialog;
    }*/

    //No estaba funcionando, revisar si se vuelve a implementar
    /*public IEnumerator ShowDialogText(string text, bool waitForInput=true, bool autoClose=true)
    {
        //StartDialog(); Que funcion llamo?
        if (waitForInput)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        if (autoClose)
        {
            ResetDialog();
        }
    }*/

    /*private void ShowDialog()
    {
        dialogActive = true;
        dBox.SetActive(true);
    }

    public void StartDialog(string[] DialogLines, int CurrentLine)
    {
        ResetDialog();
        ShowDialog();
        dialogLines = DialogLines;
        currentLine = CurrentLine;
        dText.text = dialogLines[currentLine];
    }

    public void ContinueDialog()
    {
        if (dialogLines == null)
        {
            return;
        }

        if (dialogActive && Input.GetKeyDown(KeyCode.Space))
        {
            //dialogBox.SetActive(false);
            //dialogActive = false;

            currentLine++;
            isTalking = true;
        }

        if (isTalking)
        {
            if (currentLine >= dialogLines.Length)
            {
                cancelText.color = Color.red;
            }
            else
            {
                dText.text = dialogLines[currentLine];
            }
        }
    }

    public void ResetDialog()
    {
        isTalking = false;
        currentLine = 0;
        dialogLines = null;
        dBox.SetActive(false);
        dialogActive = false;
    }*/
}
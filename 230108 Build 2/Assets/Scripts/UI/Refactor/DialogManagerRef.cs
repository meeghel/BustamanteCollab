using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManagerRef : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    public static DialogManagerRef instance { get; private set; }

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

    /*Dialog dialog;
    Action onDialogFinished;
    int currentLine = 0;
    bool isTyping;*/

    public bool IsShowing { get; private set; }

    public IEnumerator ShowDialogText(string text, bool waitForInput=true, bool autoClose=true)
    {
        OnShowDialog?.Invoke();
        IsShowing = true;
        dialogBox.SetActive(true);

        yield return TypeDialog(text);
        if (waitForInput)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        if (autoClose)
        {
            CloseDialog();
        }

    }

    public void CloseDialog()
    {
        dialogBox.SetActive(false);
        IsShowing = false;
        OnCloseDialog?.Invoke();
    }

    public IEnumerator ShowDialog(Dialog dialog/*, Action onFinished=null*/)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();
        IsShowing = true;
        /*this.dialog = dialog;
        onDialogFinished = onFinished;*/
        dialogBox.SetActive(true);

        foreach (var line in dialog.Lines)
        {
            yield return TypeDialog(line);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        //StartCoroutine(TypeDialog(dialog.Lines[0]));
        //dialogText.text = dialog.Lines[0];
        dialogBox.SetActive(false);
        IsShowing = false;
        OnCloseDialog?.Invoke();
    }

    //esto esta funcionando? revisar HandleUpdate
    public void HandleUpdate()
    {
        /*if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            ++currentLine;
            if(currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                IsShowing = false;
                dialogBox.SetActive(false);
                onDialogFinished?.Invoke();
                OnCloseDialog?.Invoke();
            }
        }*/
    }

    public IEnumerator TypeDialog(string line)
    {
        //isTyping = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        //isTyping = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBox : MonoBehaviour
{

    public GameObject Box;
    public bool isBoxActive = false;

    public void ToggleBox()
    {
        if (!isBoxActive)
        {
            isBoxActive = true;
            Box.SetActive(isBoxActive);
        }
        else
        {
            isBoxActive = false;
            Box.SetActive(isBoxActive);
        }
    }
}

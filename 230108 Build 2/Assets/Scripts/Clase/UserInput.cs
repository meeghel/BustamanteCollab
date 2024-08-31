using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    InputHandle handle;
    InputHandleReceiver handleReceiver;

    public DialogBox Box;
    BoxSwitch boxSwitch;

    // Start is called before the first frame update
    void Start()
    {
        ICommands inputHandleCommand = new InputHandleCommand(handle);
        handleReceiver = new InputHandleReceiver(inputHandleCommand);

        ICommands turnOnDialogBox = new BoxSwitchCommand(Box);
        boxSwitch = new BoxSwitch(turnOnDialogBox);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            boxSwitch.ToggleBox();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            handleReceiver.StorePosition(mousePos.x, mousePos.y, mousePos.z);
        }
    }
}

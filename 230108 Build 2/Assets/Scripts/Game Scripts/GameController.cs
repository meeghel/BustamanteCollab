using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Dialog }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Camera worldCamera;

    GameState state;

    private void Awake()
    {
        //ConditionsDB.Init();
    }

    // Start is called before the first frame update
    private void Start()
    {
        DialogManagerRef.instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };

        DialogManagerRef.instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;
        };
    }

    // Update is called once per frame
    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManagerRef.instance.HandleUpdate();
        }
    }
}

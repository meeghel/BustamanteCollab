using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Dialog, Menu, Inventory, Cutscene, Paused }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Camera worldCamera;
    [SerializeField] InventoryUI inventoryUI;


    GameState state;
    GameState prevState;

    public SceneDetails CurrentScene { get; private set; }

    public SceneDetails PrevScene { get; private set; }

    MenuController menuController;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        menuController = GetComponent<MenuController>();

        //Para quitar el mouse del juego (tutorial dejo mouse)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //ConditionsDB.Init();
    }

    // Start is called before the first frame update
    private void Start()
    {
        DialogManagerRef.instance.OnShowDialog += () =>
        {
            prevState = state;
            state = GameState.Dialog;
        };

        DialogManagerRef.instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
                state = prevState;
        };

        menuController.onBack += () =>
        {
            state = GameState.FreeRoam;
        };

        menuController.onMenuSelected += OnMenuSelected;
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            prevState = state;
            state = GameState.Paused;
        }
        else
        {
            state = prevState;
        }
    }

    public void OnEnterTrainersView(TrainerController trainer)
    {
        state = GameState.Cutscene;
        StartCoroutine(trainer.TriggerTrainerBattle(playerController));
    }

    // Update is called once per frame
    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                menuController.OpenMenu();
                state = GameState.Menu;
            }
        }
        else if (state == GameState.Dialog)
        {
            DialogManagerRef.instance.HandleUpdate();
        }
        else if (state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }
        else if (state == GameState.Inventory)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            inventoryUI.HandleUpdate(onBack);
        }
    }

    public void SetCurrentScene(SceneDetails currScene)
    {
        PrevScene = CurrentScene;
        CurrentScene = currScene;
    }

    void OnMenuSelected(int selectedItem)
    {
        if (selectedItem == 0)
        {
            // Continuar
        }
        else if (selectedItem == 1)
        {
            // Inventario
            inventoryUI.gameObject.SetActive(true);
            state = GameState.Inventory;
        }
        else if (selectedItem == 2)
        {
            // TODO Guardar
            state = GameState.FreeRoam;

            // TODO Agregar boton para Load
            //meter freeroam en Load tambien
        }
        else if (selectedItem == 3)
        {
            // TODO Terminar Juego
        }
        else if (selectedItem == 4)
        {
            // TODO Reset
        }

    }
}

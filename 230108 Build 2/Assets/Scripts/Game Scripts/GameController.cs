using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Dialog, Menu, Cutscene, Paused }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Camera worldCamera;

    GameState state;

    GameState stateBeforePause;

    public SceneDetails CurrentScene { get; private set; }

    public SceneDetails PrevScene { get; private set; }

    MenuController menuController;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        menuController = GetComponent<MenuController>();

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
            stateBeforePause = state;
            state = GameState.Paused;
        }
        else
        {
            state = stateBeforePause;
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
        }
        else if (selectedItem == 2)
        {
            // TODO Guardar
            // TODO Agregar boton para Load
        }
        else if (selectedItem == 3)
        {
            // TODO Terminar Juego
        }
        else if (selectedItem == 4)
        {
            // TODO Reset
        }

        state = GameState.FreeRoam;
    }
}

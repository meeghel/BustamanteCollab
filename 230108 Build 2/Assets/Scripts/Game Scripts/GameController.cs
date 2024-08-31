using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{ 
    FreeRoam,
    Dialog,
    Menu,
    Inventory,
    Cutscene,
    Paused,
    Shop,
    CharacterSelection,
    PrimaryWeaponSelection,
    SecondaryWeaponSelection,
    PartyScreen
}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Camera worldCamera;
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] CharacterManager characterManager;
    [SerializeField] WeaponManager weaponManager;
    //[SerializeField] WeaponManager secondaryWeapon;
    bool isSecondary = false;

    GameState state;
    GameState prevState;
    //public static GameState CurrentState { get; private set; }

    //int prevCharacter;
    //int selectedCharacter;

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

        ItemDB.Init();
        QuestDB.Init();
    }

    // Start is called before the first frame update
    private void Start()
    {
        partyScreen.Init();

        DialogManagerRef.instance.OnShowDialog += () =>
        {
            prevState = state;
            state = GameState.Dialog;
        };

        DialogManagerRef.instance.OnDialogFinished += () =>
        {
            if (state == GameState.Dialog)
                state = prevState;
        };

        menuController.onBack += () =>
        {
            state = GameState.FreeRoam;
        };

        menuController.onMenuSelected += OnMenuSelected;

        partyScreen.OnStart += () =>
        {
            state = GameState.PartyScreen;
        };
        //partyScreen.PartyScreenState += PSInstance;
        partyScreen.OnFinish += () =>
        {
            state = GameState.FreeRoam;
        };

        ShopController.i.OnStart += () => state = GameState.Shop;
        ShopController.i.OnFinish += () => state = GameState.FreeRoam;

        //WeaponManager.i.OnStart += () => state = GameState.WeaponSelection;
        //WeaponManager.i.OnFinish += () => state = GameState.FreeRoam;
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            prevState = state;
            state = GameState.Paused;

            // refactor enemy
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                //enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                enemy.GetComponent<Enemy>().enabled = false;
                //enemy.GetComponent<Enemy>().currentState = EnemyState.idle;
            }
        }
        else
        {
            state = prevState;
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                //enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                enemy.GetComponent<Enemy>().enabled = true;
                //enemy.GetComponent<Enemy>().currentState = EnemyState.idle;
            }
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
            Debug.Log("GameState.FreeRoam");
            playerController.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                menuController.OpenMenu();
                state = GameState.Menu;
            }
            else if (Input.GetKey(KeyCode.RightControl))
            {
                characterManager.OpenCharacterManager();
                state = GameState.CharacterSelection;
            }
            // Cambiar Update para tener Manager separado para Primary y Secondary Weapon
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                weaponManager.OpenPrimaryManager();
                state = GameState.PrimaryWeaponSelection;
            }
            else if (Input.GetKey(KeyCode.RightShift))
            {
                isSecondary = true;
                weaponManager.OpenSecondaryManager();
                state = GameState.SecondaryWeaponSelection;
            }
        }
        else if (state == GameState.Dialog)
        {
            Debug.Log("GameState.Dialog");
            DialogManagerRef.instance.HandleUpdate();
        }
        else if (state == GameState.Menu)
        {
            Debug.Log("GameState.Menu");
            menuController.HandleUpdate();
        }
        else if (state == GameState.Inventory)
        {
            Debug.Log("GameState.Inventory");
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = GameState.FreeRoam;
                Debug.Log("GameState.FreeRoam");
            };

            inventoryUI.HandleUpdate(onBack);
        }
        else if (state == GameState.PartyScreen)
        {
            Debug.Log("GameState.PartyScreen");
            Action onSelected = () =>
            {
                partyScreen.gameObject.SetActive(false);
                state = GameState.FreeRoam;
                Debug.Log("GameState.FreeRoam");
            };

            Action onBack = () =>
            {
                partyScreen.gameObject.SetActive(false);
                menuController.OpenMenu();
                state = GameState.Menu;
            };

            partyScreen.HandleUpdate(onSelected, onBack);
        }
        else if (state == GameState.Shop)
        {
            Debug.Log("GameState.Shop");
            ShopController.i.HandleUpdate();
        }
        else if (state == GameState.CharacterSelection)
        {
            Debug.Log("GameState.CharacterSelection");
            Action onBack = () =>
            {
                characterManager.CloseCharacterManager();
                state = GameState.FreeRoam;
                Debug.Log("GameState.FreeRoam");
            };

            characterManager.HandleUpdate(onBack);
        }
        else if (state == GameState.PrimaryWeaponSelection)
        {
            Debug.Log("GameState.PrimaryWeaponSelection");
            Action onBack = () =>
            {
                weaponManager.ClosePrimaryManager();
                state = GameState.FreeRoam;
                Debug.Log("GameState.FreeRoam");
            };

            weaponManager.PrimaryHandleUpdate(onBack);
        }
        else if (state == GameState.SecondaryWeaponSelection)
        {
            Debug.Log("GameState.SecondaryWeaponSelection");
            Action onBack = () =>
            {
                weaponManager.CloseSecondaryManager();
                state = GameState.FreeRoam;
                Debug.Log("GameState.FreeRoam");
            };

            weaponManager.SecondaryHandleUpdate(onBack);
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
            // Personajes
            partyScreen.gameObject.SetActive(true);
            partyScreen.SetPartyData(playerController.GetComponent<PlayerParty>().Characters);
            state = GameState.PartyScreen;
        }
        else if (selectedItem == 3)
        {
            // TODO Guardar
            state = GameState.FreeRoam;

            // TODO Agregar boton para Load
            //meter freeroam en Load tambien
        }
        else if (selectedItem == 4)
        {
            // TODO Terminar Juego
        }
        else if (selectedItem == 5)
        {
            // TODO Reset
        }
    }

    public IEnumerator MoveCamera(Vector2 moveOffset, bool waitForFadeOut=false)
    {
        yield return Fader.i.FadeIn(0.5f);

        worldCamera.transform.position += new Vector3(moveOffset.x, moveOffset.y);

        if (waitForFadeOut)
            yield return Fader.i.FadeOut(0.5f);
        else
            StartCoroutine(Fader.i.FadeOut(0.5f));
    }

    public GameState State => state;

    /*public static void ChangeState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log($"GameState: {newState}");
    }*/

    /*public void PSInstance()
    {
        state = GameState.PartyScreen;
        Debug.Log("GameState.PartyScreen");
    }*/
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState { Walk, Attack, Interact, Stagger, Idle, Fainted }
public enum WeaponState { Idle, Attack }

public class PlayerController : MonoBehaviour
{
    public float OffsetY { get; private set; } = 0.3f;

    [SerializeField] Text characterName;
    [SerializeField] Image characterIcon;
    [SerializeField] GameObject primaryWeapon;
    [SerializeField] GameObject secondaryWeapon;
    
    //[SerializeField] GameObject secondaryWeapon;
    //[SerializeField] Text characterLevel;
    [SerializeField] HeartManager heartManager;
    [SerializeField] WeaponManager weaponManager;

    [SerializeField] PlayerHud playerHud;
    [SerializeField] GasManager gasManager;
    [SerializeField] CharacterManager characterManager;
    [SerializeField] AnimatorController animatorController;
    [SerializeField] PartyScreen partyScreen;
    //[SerializeField] PlayerCharacter character;

    public PlayerState state;
    public WeaponState weaponState;
    GameState gameState;
    public float moveSpeed;
    
    private Rigidbody2D myRigidbody;
    // anterior es Vector3, Refactor es Vector2
    private Vector2 input;

    //TODO quitar moveSpeed de aqui
    //public float moveSpeed;

    //TODO estos eventos los quita v#40 23:00, sistema triggerables
    //public event Action OnEncountered;
    //public event Action<Collider2D> OnEnterTrainersView;

    private bool isMoving;
    public bool canMove;
    //private Vector2 input;

    //TODO quitar uso de PlayerCharacter, todo tener aqui
    Animator animator;
    Animator weaponAnimator;
    WeaponController secondaryWeaponCtrl;
    //FlameThrower flameThrower;
    Inventario inventario;
    PlayerCharacter character;
    PlayerParty playerParty;

    public PlayerAttributes Player { get; set; }
    public WeaponItem SecondaryWeapon { get; set; }

    private void Awake()
    {
        //TODO quitar uso de PlayerCharacter, todo tener aqui
        animator = GetComponent<Animator>();
        //weaponAnimator = primaryWeapon.GetComponent<Animator>();
        //flameThrower = secondaryWeapon.GetComponent<FlameThrower>();
        inventario = Inventario.GetInventory();
        character = GetComponent<PlayerCharacter>();
        playerParty = GetComponent<PlayerParty>();
        myRigidbody = GetComponent<Rigidbody2D>();
        SetPositionAndSnapToTile(transform.position);
    }

    private void Start()
    {
        Setup(playerParty);
        SetData(Player);
        weaponAnimator = primaryWeapon.GetComponent<Animator>();
        secondaryWeaponCtrl = secondaryWeapon.GetComponent<WeaponController>();

        state = PlayerState.Idle;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        weaponState = WeaponState.Idle;
        weaponAnimator.SetFloat("moveX", 0);
        weaponAnimator.SetFloat("moveY", -1);
        canMove = true;
    }

    // video 14 mete parametro player
    // revisar si tengo que meter this.playerParty para asignar desde selector
    // Setup para asignar datos al PlayerUnit (imagen)
    public void Setup(PlayerParty playerParty)
    {
        this.playerParty = playerParty;

        SetupPlayer(playerParty.GetHealthyCharacter());

        //playerHud.SetData(Player);
        //animator.runtimeAnimatorController = Player.Base.AnimatorController;

        //this.playerParty = playerParty;
        //Player = playerParty.GetHealthyCharacters();

        //Player = player;
        //SetData(Player);
        // aqui pone la imagen del Player / Enemy Unit
    }

    public void SetupPlayer(PlayerAttributes player)
    {
        //Player = new PlayerAttributes(_base, level);

        Player = player;
        playerHud.SetData(Player);
        gasManager.SetData(Player);
        characterManager.UpdateData(Player);
        animator.runtimeAnimatorController = Player.Base.AnimatorController;

        //currentPlayer = Player.Base.Name;
        //level = Player.Level;

        //this.playerParty = playerParty;
        //Player = playerParty.GetHealthyCharacters();

        //Player = player;
        //SetData(Player);
        // aqui pone la imagen del Player / Enemy Unit
    }

    // SetData para asignar datos en Hud UI (nombre, nivel, vida)
    public void SetData(PlayerAttributes player)
    {
        //Player = player;

        characterName.text = player.Base.Name;
        //characterLevel.text = "Lvl " + player.Level;
        //animator.runtimeAnimatorController = player.AnimatorController;
        heartManager.SetHearts(player.MaxHP / 2);
        weaponManager.ArrowCount.text = "" + Player.Arrows;
    }

    public void UpdateHP()
    {
        //Player = player;

        playerHud.UpdateHP();
        heartManager.SetHearts(Player.MaxHP / 2);
        heartManager.UpdateHearts(Player.HP/2, Player.MaxHP/2);
        weaponManager.ArrowCount.text = "" + Player.Arrows;
        gasManager.AddGas();
    }

    public void UpdatePlayer(PlayerAttributes player)
    {
        Player = player;
        playerHud.SetData(Player);
        characterManager.UpdateData(Player);
        animator.runtimeAnimatorController = Player.Base.AnimatorController;
        heartManager.UpdateHearts(Player.HP / 2, Player.MaxHP / 2);
        weaponManager.ArrowCount.text = "" + Player.Arrows;
    }

    public IEnumerator PlayerFainted()
    {
        state = PlayerState.Fainted;
        //character.IsMoving = false;
        GameController.Instance.PauseGame(true);

        // poner if para confirmar que haya jugadores en PlayerParty. si no, se acaba el juego.
        var nextPlayer = playerParty.GetHealthyCharacter();
        if (nextPlayer != null)
        {
            int selectedChoice = 0;

            yield return DialogManagerRef.instance.ShowDialogText($"¡{Player.Base.Name} no puede seguir! ¿Quieres continuar?",
                choices: new List<string>() { "Si", "No" },
                onChoiceSelected: (choiceIndex) => selectedChoice = choiceIndex);

            yield return new WaitForEndOfFrame();

            if (selectedChoice == 0)
            {
                // Si
                //GameController.Instance.State = GameState.PartyScreen;
                //inventoryUI.HandleUpdate(OnBackFromFainted);
                OpenPartyScreen();
                //state = PlayerState.Idle;
            }
            else if (selectedChoice == 1)
            {
                // No (GAME OVER)
                gameObject.SetActive(false);
                //yield return DialogManagerRef.instance.ShowDialogText($"Ok, aqui estare cuando lo necesites.");
            }
        }
        else
        {
            // (GAME OVER)
            gameObject.SetActive(false);
        }

        GameController.Instance.PauseGame(false);
    }

    void OpenPartyScreen()
    {
        //state = InventoryUIState.PartyScreen;
        //playerParty = GetPlayerParty();
        partyScreen.CalledFrom = GameState.Dialog;
        partyScreen.gameObject.SetActive(true);
        //partyScreen.StartPartyScreen();
        partyScreen.Init();
        partyScreen.SetPartyData(playerParty.Characters);
        //partyScreen.HandleUpdate(OnBackFromFainted, null);
        //partyScreen.CalledFrom = gameState;
    }

    public void SetPositionAndSnapToTile(Vector2 pos)
    {
        pos.x = Mathf.Floor(pos.x) + 0.5f;
        pos.y = Mathf.Floor(pos.y) + 0.5f + OffsetY;

        transform.position = pos;
    }

    public void HandleUpdate()
    {
        if (!character.IsMoving)
        {
            state = PlayerState.Idle;
            weaponState = WeaponState.Idle;

            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // remove diagonal movement
            if (input.x != 0) input.y = 0;

            // anterior es Vector3, Refactor es Vector2
            if (input != Vector2.zero)
            {
                state = PlayerState.Walk;
                weaponState = WeaponState.Idle;
                StartCoroutine(character.Move(input, OnMoveOver));
            }
        }

        character.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Z))
            StartCoroutine(Interact());

        // Version original DESDE AQUI
        /*if (!canMove)
        {
            myRigidbody.velocity = Vector2.zero;
            return;
            //Revisar agregar anim.speed = zero
        }
        // Is the player in an interaction
        if (currentState == PlayerState.Interact)
        {
            return;
        }

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // remove diagonal movement
        if (input.x != 0) input.y = 0;

        if (currentState == PlayerState.Walk || currentState == PlayerState.Idle)//&& change != Vector3.zero)
        {
            Move(input);
            //animator.SetBool("isMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(Interact());
        }*/
        // Version original HASTA ACA

        // Implementar ataque y ataque especial
        if (Input.GetKeyUp(KeyCode.X) && state != PlayerState.Attack && state != PlayerState.Stagger)
        {
            state = PlayerState.Attack;
            weaponState = WeaponState.Attack;
            StartCoroutine(character.AttackCo());
            state = PlayerState.Walk;
            weaponState = WeaponState.Idle;
        }

        if (SecondaryWeapon != null && SecondaryWeapon.WeaponType is WeaponType.Ranged)
        {
            if (Input.GetKeyDown(KeyCode.C) && state != PlayerState.Attack && state != PlayerState.Stagger)
            {
                state = PlayerState.Attack;
                weaponState = WeaponState.Attack;

                StartCoroutine(character.RangedAttackCo());
                state = PlayerState.Walk;
                weaponState = WeaponState.Idle;
            }
        }

        if (SecondaryWeapon != null && SecondaryWeapon.WeaponType is WeaponType.Particle)
        {
            if (Input.GetKey(KeyCode.C) && state != PlayerState.Attack && state != PlayerState.Stagger)
            {
                state = PlayerState.Attack;
                weaponState = WeaponState.Attack;

                secondaryWeaponCtrl.ActiveWeapon.Shoot();
                secondaryWeaponCtrl.ActiveWeapon.Setup(ChooseFlameDirection());

                gasManager.UpdateData(Player);
                Player.ReduceGas(SecondaryWeapon.GasCost);
            }
            else
            {
                secondaryWeaponCtrl.ActiveWeapon.Stop();
                state = PlayerState.Walk;
                weaponState = WeaponState.Idle;
            }
        }

        if (state == PlayerState.Fainted)
        {
            partyScreen.HandleUpdate(OnBackFromFainted,null);
        }
    }  

    Vector3 ChooseFlameDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    void OnBackFromFainted()
    {
        partyScreen.gameObject.SetActive(false);

        //state = PlayerState.Idle;
        //inventoryUI.gameObject.SetActive(false);
        //StartCoroutine(StartMenuState());
    }

    IEnumerator Interact()
    {
        //TODO revisar en character.Animator en vez de GetFloat("moveX" y "moveY") en vez de MoveX/Y
        //var facingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"), 0f);
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.6f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            yield return collider.GetComponent<Interactuable>()?.Interact(transform);
        }
    }

    /*IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;

        CheckForEncounters();
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }

        return true;
    }*/
    IPlayerTriggerable currentlyInTrigger;

    private void OnMoveOver()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, OffsetY/*character.OffsetY*/), 0.2f, GameLayers.i.TriggerableLayers);

        IPlayerTriggerable triggerable = null;
        foreach (var collider in colliders)
        {
            triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable != null)
            {
                if (triggerable == currentlyInTrigger && !triggerable.TriggerRepeatedly)
                    break;
                // TODO video #45 25:00 implementar animator en player
                //character.Animator.IsMoving = false;
                animator.SetBool("isMoving", false);
                triggerable.OnPlayerTriggered(this);
                currentlyInTrigger = triggerable;
                break;
            }
        }

        // TODO Revisar en clase
        /*if (colliders.Count() == 0 || triggerable != currentlyInTrigger)
            currentlyInTrigger = null;*/
    }

    void Move(Vector3 moveVec, Action OnMoveOver = null)
    {
        if (moveVec != Vector3.zero)
        {
            moveVec.Normalize();
            myRigidbody.MovePosition(transform.position + moveVec * moveSpeed * Time.deltaTime);
            moveVec.x = Mathf.Round(moveVec.x);
            moveVec.y = Mathf.Round(moveVec.y);
            animator.SetFloat("moveX", moveVec.x);
            animator.SetFloat("moveY", moveVec.y);
            animator.SetBool("isMoving", true);

            /*var targetPos = transform.position;
            targetPos.x += moveVec.x;
            targetPos.y += moveVec.y;

            if (!IsPathClear(targetPos))
                yield break;*/

            //change.x = Mathf.Clamp(moveVec.x, -1f, 1f);
            //change.y = Mathf.Clamp(moveVec.y, -1f, 1f);

            //animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
            //animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

            //IsMoving = true;

            /*while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPos;*/

            //IsMoving = false;

            OnMoveOver?.Invoke();
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    /*public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }*/

    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;

        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer | GameLayers.i.EnemyLayer) == true)
            return false;

        return true;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.EnemyLayer) != null)
        {
            return false;
        }

        return true;
    }

    public void LookTowards(Vector3 targetPos)
    {
        var xdiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        var ydiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        if (xdiff == 0 || ydiff == 0)
        {
            input.x = Mathf.Clamp(xdiff, -1f, 1f);
            input.y = Mathf.Clamp(ydiff, -1f, 1f);

            //animator.MoveX = Mathf.Clamp(xdiff, -1f, 1f);
            //animator.MoveY = Mathf.Clamp(ydiff, -1f, 1f);
        }
        else
            Debug.LogError("Error in Look Towards: You can't ask the character to look diagonally");
    }

    /*public string Name {
        get => name;
    }

    public Sprite Sprite {
        get => sprite;
    }*/

    //public PlayerCharacter Character => character;
    public Animator Animator => animator;
}

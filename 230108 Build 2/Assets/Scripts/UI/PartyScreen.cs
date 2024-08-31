using Ink;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//public enum PartyScreenState { CharacterSelection, Busy }

public class PartyScreen : MonoBehaviour
{
    [SerializeField] Image characterIcon;
    [SerializeField] Text characterName;
    [SerializeField] Text characterDescription;
    [SerializeField] Text messageText;
    [SerializeField] PlayerController playerController;
    [SerializeField] HeartManager heartManager;

    [SerializeField] GameObject partyList;
    [SerializeField] PartyMemberUI partyMemberUI;

    [SerializeField] Image upArrow;
    [SerializeField] Image downArrow;

    //PartyMemberUI[] memberSlots;
    List<PartyMemberUI> memberSlots;
    PlayerParty playerParty;
    List<PlayerAttributes> characters;
    Action<PlayerAttributes> changeCharacter;
    RectTransform partyListRect;

    /// <summary>
    /// PartyScreen can be called from different state like Menu, WeaponSelection, etc
    /// </summary>
    public GameState? CalledFrom { get; set; }

    int selection = 0;

    public PlayerAttributes SelectedMember => characters[selection];

    // cuantos items hay en viewport de lista, ajustar si lo cambio
    const int itemsInViewport = 4;

    //PartyScreenState state;

    public event Action OnStart;
    //public event Action PartyScreenState;
    public event Action OnFinish;

    private void Awake()
    {
        playerParty = GetPlayerParty();
        partyListRect = partyList.GetComponent<RectTransform>();
    }

    private void Start()
    {
        OnStart?.Invoke();
    }

    /*public IEnumerator StartPartyScreen()
    {
        OnStart?.Invoke();
        yield return null;
    }*/

    public void Init()
    {
        //OnStart?.Invoke();
        //ChangeState();

        //240228
        // En video #15 usa getcomponents y metodo SetPartyData
        // porque solo tiene 6 espacios. si no se usan se deshabilitan
        // Init lo llama para inicializarlo cuando va a iniciar la batalla
        // como un tipo Instance sin mostrar todavia
        // no muestra Party Screen hasta que llama OpenPartyScreen en BattleSystem

        //memberSlots = GetComponentsInChildren<PartyMemberUI>();

        // Clear all the existing items
        foreach (Transform child in partyList.transform)
            Destroy(child.gameObject);

        playerParty = GetPlayerParty();

        memberSlots = new List<PartyMemberUI>();
        foreach (var memberSlot in playerParty.Characters)
        {
            var partyMemberUIObj = Instantiate(partyMemberUI, partyList.transform);
            partyMemberUIObj.SetData(memberSlot);

            memberSlots.Add(partyMemberUIObj);
        }

        //UpdateCharacterSelection();
        //UpdateMemberSelection(selection);
    }

    public void HandleUpdate(Action onSelected, Action onBack, Action<PlayerAttributes> changeCharacter=null)
    {
        this.changeCharacter = changeCharacter;
        var prevSelection = selection;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            ++selection;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            --selection;

        selection = Mathf.Clamp(selection, 0, playerParty.Characters.Count - 1);

        //UpdateCharacterSelection();
        if(selection != prevSelection)
            UpdateMemberSelection(selection);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty.Characters[selection];
            if (selectedMember.HP <= 0)
            {
                SetMessageText("¡No puedes usar ese personaje!");
                return;
            }
            if (selectedMember == playerController.Player)
            {
                SetMessageText("¡Es el mismo personaje!");
                return;
            }

            // revisar orden si asi vuelvo al juego?
            //state = InventoryUIState.Busy;
            StartCoroutine(SwitchCharacter(selectedMember, onSelected));

            CalledFrom = null;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            //OnFinish?.Invoke();
            onBack?.Invoke();
            CalledFrom = null;
        }
    }

    /*void UpdateCharacterSelection()
    {
        var charSlots = playerParty.Characters;

        selection = Mathf.Clamp(selection, 0, charSlots.Count - 1);

        for (int i = 0; i < memberSlots.Count; i++)
        {
            if (i == selection)
            {
                memberSlots[i].NameText.color = GlobalSettings.i.HighlightedColor;
                memberSlots[i].LevelText.color = GlobalSettings.i.HighlightedColor;
            }
            else
            {
                memberSlots[i].NameText.color = Color.black;
                memberSlots[i].LevelText.color = Color.black;
            }
        }

        if (charSlots.Count > 0)
        {
            var character = charSlots[selection];
            characterIcon.sprite = character.Base.IconSprite;
            characterName.text = character.Base.Name;
            characterDescription.text = character.Base.Description;
        }

        HandleScrolling();
    }*/

    void HandleScrolling()
    {
        if (memberSlots.Count <= itemsInViewport) return;

        // Multiply the selectedItem by the height of the Item slot
        float scrollPos = Mathf.Clamp(selection - itemsInViewport / 2, 0, selection) * memberSlots[0].Height;
        partyListRect.localPosition = new Vector2(partyListRect.localPosition.x, scrollPos);

        bool showUpArrow = selection > itemsInViewport / 2;
        upArrow.gameObject.SetActive(showUpArrow);

        bool showDownArrow = selection + itemsInViewport / 2 < memberSlots.Count;
        downArrow.gameObject.SetActive(showDownArrow);
    }

    IEnumerator SwitchCharacter(PlayerAttributes newCharacter, Action backToGame)
    {
        GameController.Instance.PauseGame(true);
        yield return Fader.i.FadeIn(0.5f);

        playerController.UpdatePlayer(newCharacter);

        //SetData? ya se corrio en SetupPlayer

        yield return Fader.i.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);

        //yield return new WaitForSeconds(1);
        //OnFinish?.Invoke();
        backToGame?.Invoke();
    }

    // no se va a ocupar setpartydata, para eso estara Init (Confirmar esto)
    public void SetPartyData(List<PlayerAttributes> characters)
    {
        this.characters = characters;

        for (int i = 0; i < memberSlots.Count; i++)
        {
            if (i < characters.Count)
                memberSlots[i].SetData(characters[i]);
            else
                memberSlots[i].gameObject.SetActive(false);
        }

        OnStart?.Invoke();
        UpdateMemberSelection(selection);

        messageText.text = "Escoge tu personaje";
    }

    // no se usara update member selection, se usara update character selection (Confirmar esto)
    public void UpdateMemberSelection(int selection)
    {
        //var charSlots = inventario.GetCharacterSlots();
        var charSlots = playerParty.Characters;

        for (int i = 0;i < memberSlots.Count; i++)
        {
            if (i == selection)
                memberSlots[i].SetSelected(true);
            else
                memberSlots[i].SetSelected(false);
        }

        if (charSlots.Count > 0)
        {
            var character = charSlots[selection];
            characterIcon.sprite = character.Base.IconSprite;
            characterName.text = character.Base.Name;
            characterDescription.text = character.Base.Description;

            /*var character = charSlots[selection].Character;
            characterIcon.sprite = character.IconSprite;
            characterDescription.text = character.Description;
            characterAtk.text = $"Atq: {character.Attack}";
            characterDef.text = $"Def: {character.Defense}";
            characterSpd.text = $"Vel: {character.Speed}";*/
        }

        HandleScrolling();
    }

    public static PlayerParty GetPlayerParty()
    {
        return FindObjectOfType<PlayerController>().GetComponent<PlayerParty>();
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}

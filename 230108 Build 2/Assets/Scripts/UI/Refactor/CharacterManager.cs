using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum CharacterManagerState { CharacterSelection, Busy }

public class CharacterManager : MonoBehaviour
{
    [SerializeField] Text characterName;
    [SerializeField] Image characterIcon;
    //[SerializeField] PlayerParty playerParty;
    //[SerializeField] Text characterLevel;
    [SerializeField] PlayerController playerController;
    //[SerializeField] HeartManager heartManager;
    //[SerializeField] AnimatorController animatorController;
    //[SerializeField] PlayerCharacter character;

    [SerializeField] Image leftArrow;
    [SerializeField] Image rightArrow;

    public event Action OnStart;
    //public event Action OnFinish;

    CharacterManagerState state;

    public int currentMember = 0;
    //public string activeCharacter;
    public bool selectionChange = false;

    //List<PlayerAttributes> characters = new List<PlayerAttributes>();
    Inventario inventario;
    PlayerParty playerParty;
    PlayerAttributes _player;
    //PlayerController playerController;
    //Animator animator;
    //PlayerCharacter character;

    private void Awake()
    {
        //playerController = GetComponent<PlayerController>();
        //animator = GetComponentInChildren<Animator>();
        //character = GetComponentInChildren<PlayerCharacter>();
        //animator = this.transform.GetChild(selectedCharacter).GetComponent<Animator>();
        //character = this.transform.GetChild(selectedCharacter).GetComponent<PlayerCharacter>();
        //animator = this.gameObject.transform.GetChild(selectedCharacter).GetComponent<Animator>();
        //character = this.gameObject.transform.GetChild(selectedCharacter).GetComponent<PlayerCharacter>();
    }

    private void Start()
    {
        //inventario = Inventario.GetInventory();
        playerParty = GetPlayerParty();
        //SwitchCharacter();
    }

    public void OpenCharacterManager()
    {
        leftArrow.gameObject.SetActive(true);
        rightArrow.gameObject.SetActive(true);
    }

    public void HandleUpdate(Action onBack)
    {
        OnStart?.Invoke();
        state = CharacterManagerState.CharacterSelection;

        //selectionChange = false;
        int prevSelection = currentMember;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMember;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMember;
        else if (Input.GetKeyUp(KeyCode.RightControl))
            onBack?.Invoke();

        if (currentMember > playerParty.Characters.Count - 1)
            currentMember = 0;
        else if (currentMember < 0)
            currentMember = playerParty.Characters.Count - 1;

        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Characters.Count - 1);

        if (prevSelection != currentMember)
        {
            selectionChange = true;
            SelectCharacter(currentMember);
        }
        /*else if (prevSelection == selectedCharacter)
            selectionChange = false;*/
    }

    public void CloseCharacterManager()
    {
        if (selectionChange == true)
        {
            var newCharacter = playerParty.Characters[currentMember];
            GameController.Instance.PauseGame(true);
            //no funciona Fader por ser void (hacer IEnumerator)
            Fader.i.FadeIn(0.5f);

            /*var slots = inventario.GetCharacterSlots();
            selectedCharacter = Mathf.Clamp(selectedCharacter, 0, slots.Count - 1);

            if (slots.Count > 0)
            {
                var character = slots[selectedCharacter].Character;
                //activeCharacter = character.GameObject;
            }*/

            playerController.UpdatePlayer(newCharacter);
            //SwitchCharacter();

            /*var newCharacter = this.transform.GetChild(selectedCharacter);
            animator = newCharacter.GetComponent<Animator>();
            character = newCharacter.GetComponent<PlayerCharacter>();
            activeCharacter = newCharacter.GetComponent<PlayerCharacter>().Name;*/

            //no funciona Fader por ser void (hacer IEnumerator)
            Fader.i.FadeOut(0.5f);
            GameController.Instance.PauseGame(false);
        }

        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
    }

    /*void SelectCharacter(int characterIndex)
    {
        var characters = playerParty.Characters;
        selectedCharacter = Mathf.Clamp(selectedCharacter, 0, characters.Count - 1);

        if (characters.Count > 0)
        {
            var character = characters[selectedCharacter].Character;
            characterIcon.sprite = character.IconSprite;
            characterName.text = character.Name;
        }
    }*/

    // 230923 Antes del cambio a PlayerParty (slots en Inventario)
    void SelectCharacter(int characterIndex)
    {
        var charSlots = playerParty.Characters;
        currentMember = Mathf.Clamp(currentMember, 0, charSlots.Count - 1);

        if (charSlots.Count > 0)
        {
            var character = charSlots[currentMember];
            characterIcon.sprite = character.Base.IconSprite;
            characterName.text = character.Base.Name;
        }
    }

    void SwitchCharacter()
    {
        int i = 0;
        var characters = playerController.GetComponent<PlayerParty>();
        /*if (i != selectedCharacter)
            playerController.SetData(characters[selectedCharacter].Character);*/

        /*foreach (Transform character in transform)
        {
            if (i == selectedCharacter)
                character.gameObject.SetActive(true);
            else
                character.gameObject.SetActive(false);
            i++;
        }*/
    }

    /*public AnimatorController AnimatorController
    {
        get => animatorController;
    }

    public PlayerCharacter Character
    {
        get => character;
    }*/

    public static PlayerParty GetPlayerParty()
    {
        return FindObjectOfType<PlayerController>().GetComponent<PlayerParty>();
    }

    public void UpdateData(PlayerAttributes player)
    {
        _player = player;

        characterIcon.sprite = _player.Base.IconSprite;
        characterName.text = _player.Base.Name;

        //var newCharacter = playerParty.Characters[currentMember];

        //currentMember = Mathf.Clamp(currentMember, 0, charSlots.Count - 1);

        /*if (charSlots.Count > 0)
        {
            var character = charSlots[currentMember];
            characterIcon.sprite = character.Base.IconSprite;
            characterName.text = character.Base.Name;
        }*/
    }
}

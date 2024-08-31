using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitch : MonoBehaviour
{
    public GameObject activeCharacter;
    public int selectedCharacter = 0;

    // Revisar Inventario, como agregar nuevos characters (Scriptable Objects)
    [SerializeField] GameObject character1;
    [SerializeField] GameObject character2;

    //Animator animator;
    //Character character;
    //SpriteRenderer spriteRenderer;
    //PlayerController playerController;
    //PlayerAttributesBase playerAttributes;

    private void Awake()
    {
        //animator = activeCharacter.GetComponent<Animator>();
        //character = activeCharacter.AddComponent<Character>();
        //spriteRenderer = activeCharacter.GetComponent<SpriteRenderer>();
        //playerController = activeCharacter.GetComponent<PlayerController>();
        //playerAttributes = activeCharacter.GetComponent<PlayerAttributesBase>();
    }

    private void Start()
    {
        SwitchCharacter();
    }

    public void SwitchCharacter()
    {
        int i = 0;
        foreach (Transform character in transform)
        {
            if(i == selectedCharacter)
                character.gameObject.SetActive(true);
            else
                character.gameObject.SetActive(false);
            i++;
        }
    }
}

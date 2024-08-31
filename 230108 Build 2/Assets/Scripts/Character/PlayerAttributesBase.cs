using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Attributes", menuName = "Player Attributes/Create new Player Attributes")]

public class PlayerAttributesBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite mainSprite;
    [SerializeField] Sprite iconSprite;

    [SerializeField] AnimatorOverrideController animatorController;
    //[SerializeField] PlayerCharacter character;

    [Header("Base Stats")]
    [SerializeField] float maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int speed;

    [Header("Attack")]
    [SerializeField] WeaponItem mainWeapon;
    [SerializeField] WeaponItem secondWeapon;
    [SerializeField] int maxArrows;
    [SerializeField] float maxGas;

    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public Sprite MainSprite
    {
        get { return mainSprite; }
    }

    public Sprite IconSprite
    {
        get { return iconSprite; }
    }

    public AnimatorOverrideController AnimatorController
    {
        get { return animatorController; }
    }

    /*public PlayerCharacter Character
    {
        get { return character; }
    }*/

    //230919 Cambie a que todo fuera integer para que funcione Linq en PlayerParty
    public float MaxHP
    {
        get { return maxHP; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Defense
    {
        get { return defense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public WeaponItem MainWeapon
    {
        get { return mainWeapon; }
        set { mainWeapon = value; }
    }

    public WeaponItem SecondWeapon
    {
        get { return secondWeapon; }
        set { secondWeapon = value; }
    }

    public int MaxArrows
    {
        get { return maxArrows; }
    }

    public float MaxGas
    {
        get { return maxGas; }
    }
}

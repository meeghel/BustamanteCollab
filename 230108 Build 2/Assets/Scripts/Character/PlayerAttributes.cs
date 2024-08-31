using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public class PlayerAttributes
{
    [SerializeField] PlayerAttributesBase _base;
    //Revisar si sera necesario el nivel o si se hace por pocion. por nivel pudieran variar mas los incrementos.
    [SerializeField] int level;
    //[SerializeField] int hearts;

    public PlayerAttributesBase Base { get { return _base; } }
    public int Level { get { return level; } }
    //public int Hearts { get { return hearts; } }

    //public PlayerAttributesBase Base { get; set; }
    //public int Level { get; set; }

    public float HP { get; set; }
    public int Arrows { get; set; }
    public float Gas { get; set; }

    /*public PlayerAttributes(PlayerAttributesBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        HP = MaxHP;
    }*/

    public void Init()
    {
        HP = MaxHP;
        Gas = MaxGas;
        Arrows = MaxArrows;
    }

    //Falta hacer formulas (Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5;)
    public int Attack
    {
        get { return Base.Attack * Level; }
    }

    public int Defense
    {
        get { return Base.Defense * Level; }
    }

    public int Speed
    {
        get { return Base.Speed * Level; }
    }

    public float MaxHP
    {
        get { return Base.MaxHP * Level; }
    }

    public float MaxGas
    {
        get { return Base.MaxGas * Level; }
    }

    public int MaxArrows
    {
        get { return Base.MaxArrows * Level; }
    }

    public void IncreaseHearts()
    {
        ++level;
    }

    public bool TakeDamage(float amountToDamage)
    {
        Debug.Log("Si detecta Damage");
        //int damage = Mathf.FloorToInt(amountToDamage);
        //HP = Mathf.Clamp(HP - amountToDamage, 0, MaxHP);
        HP -= amountToDamage;
        if(HP <= 0)
        {
            HP = 0;
            return true;
        }

        return false;
    }

    public void Heal(float amountToHeal)
    {
        //Debug.Log($"Si detecta Heal. AmountToHeal = {amountToHeal}");
        HP = Mathf.Clamp(HP + amountToHeal, 0, MaxHP);
        //HP += amountToHeal;
    }

    public virtual void FullHeal()
    {
        //Debug.Log("Si detecta Full Heal");

        HP = MaxHP;
    }

    public void ReduceGas(float gasCost)
    {
        Gas -= gasCost * .02f;
    }

    public void IncreaseGas(float gasAmount)
    {
        Gas += gasAmount;
    }

    public void FullGas()
    {
        Gas = MaxGas;
    }

    public void ReduceArrows(int arrowCost)
    {
        Arrows -= arrowCost;
    }

    public void IncreaseArrows(int arrowAmount)
    {
        //Arrows = Mathf.Clamp(Arrows + arrowAmount, 0, Arrows);
        Arrows += arrowAmount;
    }

    public void FullArrows()
    {
        Arrows = MaxArrows;
    }

    /*public WeaponItem MainWeapon
    {
        get { return Base.MainWeapon; }
    }

    public WeaponItem SecondWeapon
    {
        get { return Base.SecondWeapon; }
    }

    public AnimatorController AnimatorController
    {
        get { return Base.AnimatorController; }
    }

    public PlayerCharacter Character
    {
        get { return Base.Character; }
    }*/
}

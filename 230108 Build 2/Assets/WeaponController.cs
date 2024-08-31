using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[DisallowMultipleComponent]
public class WeaponController : MonoBehaviour
{
    public bool isSecondary;

    WeaponItem activeWeapon;
    Animator animator;
    Inventario inventario;
    SpriteRenderer spriteRenderer;

    [SerializeField] private Transform weaponParent;

    public WeaponItem ActiveWeapon
    {
        get => activeWeapon;
        set => activeWeapon = value;
    }

    public void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventario = GetComponentInParent<Inventario>();
        SetupWeapon();
    }

    public void Start()
    {
        // revisar como no usar spriteRenderer false
        spriteRenderer.enabled = false;

        /*if (ActiveWeapon == null)
        {
            Debug.LogError($"No WeaponItem found for Primary");
            return;
        }*/
    }

    public void SetupWeapon()
    {
        if (isSecondary)
        {
            activeWeapon = inventario.GetWeapon(0, WeaponType.Ranged);
        }
        else
        {
            activeWeapon = inventario.GetWeapon(0, WeaponType.Melee);
        }

        animator.runtimeAnimatorController = activeWeapon.AnimatorController;
    }

    public void UpdateWeapon(WeaponItem weapon)
    {
        if (isSecondary)
        {
            if (activeWeapon.Model != null)
            {
                Destroy(activeWeapon.Model);
            }

            activeWeapon = weapon;
            animator.runtimeAnimatorController = activeWeapon.AnimatorController;

            if (weapon.WeaponType is WeaponType.Particle)
            {
                // en video LlamAcademy lo puso directo en Start
                weapon.Spawn(weaponParent, this);
            }
        }
        else
        {
            activeWeapon = weapon;
            animator.runtimeAnimatorController = activeWeapon.AnimatorController;
        }
    }
}

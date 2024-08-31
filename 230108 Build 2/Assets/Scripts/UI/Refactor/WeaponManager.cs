using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponManagerState { PrimaryWeaponSelection, SecondaryWeaponSelection, Busy }

public class WeaponManager : MonoBehaviour
{
    [Header("Primary Weapon")]
    [SerializeField] WeaponController primaryWeaponCtrl;
    [SerializeField] Image primaryWeaponIcon;
    [SerializeField] Image primaryLeftArrow;
    [SerializeField] Image primaryRightArrow;

    [Header("Secondary Weapon")]
    [SerializeField] WeaponController secondaryWeaponCtrl;
    [SerializeField] Image secondaryWeaponIcon;
    [SerializeField] Image secondaryLeftArrow;
    [SerializeField] Image secondaryRightArrow;
    [SerializeField] Text arrowCount;

    [Header("")]
    [SerializeField] bool isSecondary;

    public event Action OnStart;
    //public event Action OnFinish;

    WeaponManagerState state;

    int selectedPrimWeapon = 0;
    int selectedSecWeapon = 0;
    //int selectedCategory = 1;

    //List<WeaponItem> weapons = new List<WeaponItem>();
    Inventario inventario;
    PlayerController playerController;
    PlayerCharacter character;

    public Image PrimaryWeaponIcon
    {
        get { return primaryWeaponIcon; }
        set { primaryWeaponIcon = value; }
    }

    public Image SecondaryWeaponIcon
    {
        get { return secondaryWeaponIcon; }
        set { secondaryWeaponIcon = value; }
    }

    public Text ArrowCount => arrowCount;
    //public bool IsSecondary => isSecondary;

    /*public static WeaponManager i { get; private set; }
    private void Awake()
    {
        i = this;
    }*/

    private void Start()
    {
        inventario = Inventario.GetInventory();
        playerController = FindObjectOfType<PlayerController>();
        character = FindObjectOfType<PlayerController>().GetComponent<PlayerCharacter>();
        
        if (primaryWeaponCtrl.ActiveWeapon != null)
            SelectPrimaryWeapon(0);

        if (secondaryWeaponCtrl.ActiveWeapon != null)
            SelectSecondaryWeapon(0);

        //PrimaryWeaponIcon.sprite = primaryWeaponCtrl.ActiveWeapon.Icon;
        //SecondaryWeaponIcon.sprite = secondaryWeaponCtrl.ActiveWeapon.Icon;
    }

    public void OpenPrimaryManager()
    {
        primaryLeftArrow.gameObject.SetActive(true);
        primaryRightArrow.gameObject.SetActive(true);
    }

    public void ClosePrimaryManager()
    {
        primaryLeftArrow.gameObject.SetActive(false);
        primaryRightArrow.gameObject.SetActive(false);
    }

    public void OpenSecondaryManager()
    {
        secondaryLeftArrow.gameObject.SetActive(true);
        secondaryRightArrow.gameObject.SetActive(true);
    }

    public void CloseSecondaryManager()
    {
        secondaryLeftArrow.gameObject.SetActive(false);
        secondaryRightArrow.gameObject.SetActive(false);
    }

    // Cambiar Update para tener Manager separado para Primary y Secondary Weapon
    public void PrimaryHandleUpdate(Action onBack)
    {
        OnStart?.Invoke();
        state = WeaponManagerState.PrimaryWeaponSelection;

        int prevSelection = selectedPrimWeapon;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++selectedPrimWeapon;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --selectedPrimWeapon;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            onBack?.Invoke();

        if (selectedPrimWeapon > inventario.GetSlotsByWeaponType(WeaponType.Melee).Count - 1)
            selectedPrimWeapon = 0;
        else if (selectedPrimWeapon < 0)
            selectedPrimWeapon = inventario.GetSlotsByWeaponType(WeaponType.Melee).Count - 1;

        selectedPrimWeapon = Mathf.Clamp(selectedPrimWeapon, 0, inventario.GetSlotsByWeaponType(WeaponType.Melee).Count - 1);

        if (prevSelection != selectedPrimWeapon)
        {
            SelectPrimaryWeapon(selectedPrimWeapon);
        }

        /*if (selectedWeapon > inventario.GetSlotsByCategory(1).Count - 1)
            selectedWeapon = 0;
        else if (selectedWeapon < 0)
            selectedWeapon = inventario.GetSlotsByCategory(1).Count - 1;

        selectedWeapon = Mathf.Clamp(selectedWeapon, 0, inventario.GetSlotsByCategory(1).Count - 1);

        if (prevSelection != selectedWeapon)
        {
            SelectWeapon(selectedWeapon);
        }*/
    }

    public void SecondaryHandleUpdate(Action onBack)
    {
        OnStart?.Invoke();
        state = WeaponManagerState.SecondaryWeaponSelection;

        int prevSelection = selectedSecWeapon;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++selectedSecWeapon;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --selectedSecWeapon;
        else if (Input.GetKeyUp(KeyCode.RightShift))
            onBack?.Invoke();

        List<ItemSlot> slots = new List<ItemSlot>();
        slots.AddRange(inventario.GetSlotsByWeaponType(WeaponType.Ranged));
        slots.AddRange(inventario.GetSlotsByWeaponType(WeaponType.Particle));
        slots.AddRange(inventario.GetSlotsByWeaponType(WeaponType.Special));

        if (selectedSecWeapon > slots.Count - 1)
            selectedSecWeapon = 0;
        else if (selectedSecWeapon < 0)
            selectedSecWeapon = slots.Count - 1;

        selectedSecWeapon = Mathf.Clamp(selectedSecWeapon, 0, slots.Count - 1);

        if (prevSelection != selectedSecWeapon)
        {
            SelectSecondaryWeapon(selectedSecWeapon);
        }
    }

    void SelectPrimaryWeapon(int weaponIndex)
    {
        //var slots = inventario.GetSlotsByWeaponType(WeaponType.Melee);
        List<ItemSlot> slots = new List<ItemSlot>();
        slots.AddRange(inventario.GetSlotsByWeaponType(WeaponType.Melee));

        selectedPrimWeapon = Mathf.Clamp(selectedPrimWeapon, 0, slots.Count - 1);

        if (slots.Count > 0)
        {
            var weapon = slots[selectedPrimWeapon].Item as WeaponItem;
            primaryWeaponCtrl.UpdateWeapon(weapon);
            primaryWeaponIcon.sprite = weapon.Icon;
            //character.SecondaryWeapon = weapon;
            //playerController.SecondaryWeapon = weapon;
        }
    }

    void SelectSecondaryWeapon(int weaponIndex)
    {
        //var slots = inventario.GetSlotsByWeaponType(WeaponType.Ranged);
        List<ItemSlot> slots = new List<ItemSlot>();
        slots.AddRange(inventario.GetSlotsByWeaponType(WeaponType.Ranged));
        slots.AddRange(inventario.GetSlotsByWeaponType(WeaponType.Particle));
        slots.AddRange(inventario.GetSlotsByWeaponType(WeaponType.Special));

        selectedSecWeapon = Mathf.Clamp(selectedSecWeapon, 0, slots.Count - 1);

        if (slots.Count > 0)
        {
            var weapon = slots[selectedSecWeapon].Item as WeaponItem;
            secondaryWeaponCtrl.isSecondary = true;
            secondaryWeaponCtrl.UpdateWeapon(weapon);
            secondaryWeaponIcon.sprite = weapon.Icon;
            character.SecondaryWeapon = weapon;
            playerController.SecondaryWeapon = weapon;

            // Cambiar esto para tener Primary y Secondary Weapon siempre
            /*if (weapon.WeaponType is WeaponType.Ranged || weapon.WeaponType is WeaponType.Particle)
            {
                character.SecondaryWeapon = weapon;
                playerController.SecondaryWeapon = weapon;
            }*/
        }
    }
}

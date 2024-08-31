using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotUI : MonoBehaviour
{
    [SerializeField] Text nameText;

    RectTransform rectTransform;

    private void Awake()
    {
        
    }

    public Text NameText => nameText;

    public float Height => rectTransform.rect.height;

    public void SetData(CharacterSlot characterSlot)
    {
        rectTransform = GetComponent<RectTransform>();
        nameText.text = characterSlot.Character.Name;
    }
}

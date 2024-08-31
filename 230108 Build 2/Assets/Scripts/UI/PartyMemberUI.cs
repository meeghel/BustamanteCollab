using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Text characterAtk;
    [SerializeField] Text characterDef;
    [SerializeField] Text characterSpd;

    RectTransform rectTransform;

    public Text NameText => nameText;
    public Text LevelText => levelText;

    public float Height => rectTransform.rect.height;

    public void SetData(PlayerAttributes player)
    {
        rectTransform = GetComponent<RectTransform>();
        nameText.text = player.Base.Name;
        levelText.text = "Lvl " + player.Level;
        hpBar.SetHP(player.HP / player.MaxHP);
        characterAtk.text = $"Atq: {player.Attack}";
        characterDef.text = $"Def: {player.Defense}";
        characterSpd.text = $"Vel: {player.Speed}";
    }

    public void SetSelected(bool selected)
    {
        if (selected)
            nameText.color = GlobalSettings.i.HighlightedColor;
        else
            nameText.color = Color.black;
    }
}

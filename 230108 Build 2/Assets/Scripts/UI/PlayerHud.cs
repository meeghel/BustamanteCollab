using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] HeartManager heartManager;

    PlayerAttributes _player;

    public void SetData(PlayerAttributes player)
    {
        _player = player;

        nameText.text = player.Base.Name;
        levelText.text = "Lvl " + player.Level;
        heartManager.SetHearts(player.MaxHP / 2);
        hpBar.SetHP(player.HP / player.MaxHP);
    }

    public void UpdateData(PlayerAttributes player)
    {
        _player = player;

        nameText.text = player.Base.Name;
        levelText.text = "Lvl " + player.Level;
        //heartManager.SetHearts(player.MaxHP / 2);
        hpBar.SetHP(player.HP / player.MaxHP);
    }

    public void UpdateHP()
    {
        //heartManager.UpdateHearts(_player.HP, _player.MaxHP);
        levelText.text = "Lvl " + _player.Level;
        hpBar.SetHP(_player.HP / _player.MaxHP);
    }
}

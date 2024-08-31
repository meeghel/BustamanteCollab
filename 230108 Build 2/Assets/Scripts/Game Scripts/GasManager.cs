using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasManager : MonoBehaviour
{

    public Slider gasSlider;
    public Inventario inventario;

    PlayerAttributes _player;
    //PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        //gasSlider.maxValue = inventario.maxMagic;
        //gasSlider.value = inventario.maxMagic;
        //inventario.currentMagic = inventario.maxMagic;
    }

    public void SetData(PlayerAttributes player)
    {
        _player = player;
        gasSlider.maxValue = player.MaxGas;
        gasSlider.value = player.Gas;
    }

    public void UpdateData(PlayerAttributes player)
    {
        _player = player;
        gasSlider.value = player.Gas;
    }

    public void AddGas()
    {
        //magicSlider.value += 1;

        //playerInventory.currentMagic += 1;
        //magicImage.FillAmount = (playerInventory.currentMagic / playerInventory.maxMagic);
        gasSlider.value = _player.Gas;
        if (gasSlider.value > gasSlider.maxValue)
        {
            gasSlider.value = gasSlider.maxValue;
            //inventario.currentGas = inventario.maxGas;
        }
    }

    public void DecreaseGas()
    {
        //magicSlider.value -= 1;
        //playerInventory.currentMagic -= 1;
        gasSlider.value = _player.Gas;
        if(gasSlider.value < 0)
        {
            gasSlider.value = 0;
            _player.Gas = 0;
        }
    }


}

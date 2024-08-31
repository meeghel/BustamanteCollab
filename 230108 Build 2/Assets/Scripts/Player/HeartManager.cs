using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfFullHeart;
    public Sprite emptyHeart;
    public FloatValue heartContainers;
    public GenericHealth playerCurrentHealth;

    // Use this for initialization
    /*void Start () {
        heartContainers.RuntimeValue = heartContainers.initialValue;
        InitHearts();
    }

    public void InitHearts()
    {
        for (int i = 0; i < heartContainers.RuntimeValue; i ++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
    }*/

    public void SetHearts(float hContainers)
    {
        foreach (Image child in hearts)
            child.gameObject.SetActive(false);

        for (int i = 0; i < hContainers; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
    }

    public void UpdateHearts(float currentHP, float maxHP)
    {
        /*InitHearts();
        float tempHealth = playerCurrentHealth.currentHealth / 2;
        //float tempHealth = playerCurrentHealth.RuntimeValue / 2;
        for (int i = 0; i < heartContainers.RuntimeValue; i ++)
        {
            if(i <= tempHealth-1)
            {
                //Full Heart
                hearts[i].sprite = fullHeart;
            }else if( i >= tempHealth)
            {
                //empty heart
                hearts[i].sprite = emptyHeart;
            }else{
                //half full heart
                hearts[i].sprite = halfFullHeart;
            }
        }*/

        /*foreach (Image child in hearts)
            gameObject.SetActive(false);*/

        //SetHearts(maxHP);
        //float tempHealth = currentHP;
        //int hContainers = Mathf.FloorToInt(maxHP);
        //float tempHealth = playerCurrentHealth.currentHealth / 2;
        //float tempHealth = playerCurrentHealth.RuntimeValue / 2;
        for (int i = 0; i < maxHP; i++)
        {
            if (i <= currentHP-1)
            {
                //Full Heart
                hearts[i].sprite = fullHeart;
            }
            else if (i >= currentHP)
            {
                //empty heart
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                //half full heart
                hearts[i].sprite = halfFullHeart;
            }
        }
    }
}

﻿using UnityEngine;

public class GenericHealth : MonoBehaviour
{
    public FloatValue maxHealth;
    public float currentHealth;

    [Header("Death Effects")]
    public GameObject deathEffect;
    private float deathEffectDelay = 1f;
    public LootTable thisLoot;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth.initialValue;
        maxHealth.RuntimeValue = maxHealth.initialValue;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Heal(float amountToHeal)
    {
        Debug.Log($"Si detecta Heal. AmountToHeal = {amountToHeal}");

        currentHealth += amountToHeal;
        // TODO esto resetea el efecto, cambiar a que no pueda sobre pasar el RuntimeValue
        /*if(currentHealth > maxHealth.RuntimeValue)
        {
            currentHealth = maxHealth.RuntimeValue;
        }*/
    }

    public virtual void FullHeal()
    {
        Debug.Log("Si detecta Full Heal");

        currentHealth = maxHealth.RuntimeValue;
    }

    public virtual void Damage(float amountToDamage)
    {
        Debug.Log("Si detecta Damage");
        currentHealth -= amountToDamage;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            DeathEffect();
            MakeLoot();
            this.transform.parent.gameObject.SetActive(false);
        }
    }

    public virtual void InstantDeath()
    {
        currentHealth = 0;
        DeathEffect();
        MakeLoot();
        this.transform.parent.gameObject.SetActive(false);
    }

    private void MakeLoot()
    {
        if (thisLoot != null)
        {
            PowerUp current = thisLoot.LootPowerup();
            if (current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }

    private void DeathEffect()
    {
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDelay);
        }
    }
}

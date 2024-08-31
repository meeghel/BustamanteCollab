using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new weapon")]

// TODO necesito crear clase con todos los parametros, incluyendo arma
[System.Serializable]
public class WeaponItem : ItemBase
{
    [SerializeField] WeaponType weaponType;

    [SerializeField] float damage;
    [SerializeField] float range;
    [SerializeField] float gasCost;
    [SerializeField] Arrow arrow;
    //[SerializeField] FlameThrower flameThrower;

    [SerializeField] GameObject modelPrefab;
    public Vector3 spawnPoint;
    //public Vector3 spawnRotation;

    [SerializeField] float fireRate;

    private MonoBehaviour activeMonoBehaviour;
    private GameObject model;
    private float lastShootTime;
    private ParticleSystem shootSystem;

    //[SerializeField] RuntimeAnimatorController animatorController;

    public float Damage => damage;
    public WeaponType WeaponType => weaponType;
    //public RuntimeAnimatorController AnimatorController => animatorController;

    public Arrow Arrow => arrow;
    public float GasCost => gasCost;
    //public FlameThrower FlameThrower => flameThrower;
    public GameObject Model => model;

    public override bool Use(PlayerAttributes player)
    {
        return true;
    }

    public void Spawn(Transform parent, MonoBehaviour ActiveMonoBehaviour)
    {
        this.activeMonoBehaviour = ActiveMonoBehaviour;
        lastShootTime = 0;

        model = Instantiate(modelPrefab);
        model.transform.SetParent(parent, false);
        model.transform.localPosition = spawnPoint;

        shootSystem = model.GetComponentInChildren<ParticleSystem>();
        shootSystem.Stop();
    }

    public void Setup(Vector3 direction)
    {
        model.transform.rotation = Quaternion.Euler(direction);
    }

    public void Shoot()
    {
        if (Time.time > fireRate + lastShootTime)
        {
            lastShootTime = Time.time;
            shootSystem.Play();

            //Vector3 shootDirection = shootSystem.transform.forward; // + new Vector3 Random.Range Spread
            //shootDirection.Normalize();
        }
    }

    public void Stop()
    {
        shootSystem.Stop();
    }
}

public enum WeaponType { Melee, Ranged, Particle, Special }

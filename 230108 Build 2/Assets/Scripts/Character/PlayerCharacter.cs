using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] Sprite sprite;
    [SerializeField] GameObject primaryWeapon;
    //public Transform attackPoint;
    
    //public float attackRange = 0.5f;
    //public int attackDamage;

    //[SerializeField] GameObject particleWeapon;
    [SerializeField] Text arrowCount;
    
    public float moveSpeed;

    public bool IsMoving {  get; private set; }

    public float OffsetY { get; private set; } = 0.3f;

    public Text ArrowCount { get; set; }

    Animator animator;
    Animator weaponAnimator;
    Inventario inventario;
    PlayerController player;
    FlameThrower flameThrower;

    // TODO MAGIC Player magic should be part of the magic system?
    public Signal reduceMagic;
    public GameObject projectile;
    public WeaponItem SecondaryWeapon { get; set; }

    private Rigidbody2D myRigidbody;

    public SpriteRenderer receivedItemSprite;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inventario = Inventario.GetInventory();
        player = GetComponent<PlayerController>();
        weaponAnimator = primaryWeapon.GetComponent<WeaponController>().GetComponent<Animator>();
        //flameThrower = particleWeapon.GetComponent<FlameThrower>();
    }

    private void Start()
    {
        //arrowCount.text = "" + player.Player.Arrows;
        //particleWeapon.gameObject.SetActive(false);
    }

    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null)
    {
        moveVec.x = Mathf.Clamp(moveVec.x, -1f, 1f);
        moveVec.y = Mathf.Clamp(moveVec.y, -1f, 1f);
        // formula de videos anteriores
        //moveVec.x = Mathf.Round(moveVec.x);
        //moveVec.y = Mathf.Round(moveVec.y);
        animator.SetFloat("moveX", moveVec.x);
        animator.SetFloat("moveY", moveVec.y);
        weaponAnimator.SetFloat("moveX", moveVec.x);
        weaponAnimator.SetFloat("moveY", moveVec.y);

        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;

        if (!IsPathClear(targetPos))
            yield break;

        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        IsMoving = false;

        OnMoveOver?.Invoke();
    }

    public IEnumerator AttackCo()
    {
        // Arreglar animacion usando layers
        primaryWeapon.GetComponent<SpriteRenderer>().enabled = true;
        animator.SetBool("attacking", true);
        weaponAnimator.SetBool("attacking", true);
        /*Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, GameLayers.i.EnemyLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            //Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<GenericHealth>().Damage(attackDamage);
        }*/
        yield return null;
        animator.SetBool("attacking", false);
        weaponAnimator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        primaryWeapon.GetComponent<SpriteRenderer>().enabled = false;
    }

    /*private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }*/

    public IEnumerator RangedAttackCo()
    {
        // TODO hacer animacion de special attack y cambiar parametro (SpecialAttack)
        animator.SetBool("attacking", true);
        yield return null;
        // TODO agregar funcion de special attack
        MakeProjectile();
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
    }

    public IEnumerator ParticleAttackCo()
    {
        // TODO hacer animacion de special attack y cambiar parametro (SpecialAttack)
        animator.SetBool("attacking", true);
        yield return null;
        // TODO agregar funcion de special attack
        UseFlamethrower();
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
    }

    public IEnumerator RaiseItem(ItemBase item)
    {
        animator.SetBool("receive item", true);
        receivedItemSprite.sprite = item.Icon;
        yield return null;
        
        /*if (playerInventory.currentItem != null)
        {
            if (currentState != PlayerState.Interact)
            {
                animator.SetBool("receive item", true);
                currentState = PlayerState.Interact;
                receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                animator.SetBool("receive item", false);
                currentState = PlayerState.Idle;
                receivedItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }*/
    }

    public IEnumerator LowerItem()
    {
        animator.SetBool("receive item", false);
        receivedItemSprite.sprite = null;
        yield return null;
    }

    private void MakeProjectile()
    {
        /*if (inventario.currentMagic > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            Arrow arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.Setup(temp, ChooseArrowDirection());
            inventario.ReduceMagic(arrow.arrowCost);
            reduceMagic.Raise();
        }*/

        if (player.Player.Arrows > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            Arrow arrow = Instantiate(SecondaryWeapon.Arrow, transform.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.Setup(temp, ChooseArrowDirection());
            player.Player.ReduceArrows(arrow.arrowCost);
            arrowCount.text = "" + player.Player.Arrows;
            //reduceMagic.Raise();
        }
    }

    private void UseFlamethrower()
    {
        if (player.Player.Gas > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            //FlameThrower flameThrower = Instantiate(SecondaryWeapon.FlameThrower, transform.position,Quaternion.identity).GetComponent<FlameThrower>();
            flameThrower.Setup(ChooseArrowDirection());
            player.Player.ReduceGas(flameThrower.gasCost);

            // formula para reflejar gas en barra, ver video pokemon
        }
        else if (player.Player.Gas <= 0)
        {
            flameThrower.TurnOffFlame();
        }
    }

    // TODO ABILITY This should also be part of the ability
    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    public void HandleUpdate()
    {
        animator.SetBool("isMoving", IsMoving);
    }

    IPlayerTriggerable currentlyInTrigger;
    private void OnMoveOver()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, OffsetY/*character.OffsetY*/), 0.2f, GameLayers.i.TriggerableLayers);

        IPlayerTriggerable triggerable = null;
        foreach (var collider in colliders)
        {
            triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable != null)
            {
                if (triggerable == currentlyInTrigger && !triggerable.TriggerRepeatedly)
                    break;
                // TODO video #45 25:00 implementar animator en player
                //character.Animator.IsMoving = false;
                animator.SetBool("isMoving", false);
                triggerable.OnPlayerTriggered(GetComponentInParent<PlayerController>());
                currentlyInTrigger = triggerable;
                break;
            }
        }

        // TODO Revisar en clase
        /*if (colliders.Count() == 0 || triggerable != currentlyInTrigger)
            currentlyInTrigger = null;*/
    }

    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;

        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer | GameLayers.i.EnemyLayer) == true)
            return false;

        return true;
    }

    public string Name
    {
        get => name;
    }

    public Sprite Sprite
    {
        get => sprite;
    }

    public Animator Animator
    {
        get => animator;
    }
}

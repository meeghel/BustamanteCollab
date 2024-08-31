using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum PlayerState { walk, attack, interact, stagger, idle }

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] Sprite sprite;

    public float OffsetY { get; private set; } = 0.3f;

    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;

    //Agregue esto para refactor Interactuable (#26 6:30)
    public LayerMask interactableLayer;

    public bool canMove;
    public bool IsMoving { get; set; }

    // TODO HEALTH
    /*
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    */

    public VectorValue startingPosition;

    // TODO INVENTORY Referencia al arco
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;

    // TODO HEALTH Player hit should be part of the health system?
    public Signal playerHit;

    // TODO MAGIC Player magic should be part of the magic system?
    public Signal reduceMagic;

    // TODO IFRAME Break off the iframe into its own script
    [Header("IFrame")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;

    // TODO ABILITY Break this off with the player ability system
    [Header("Projectiles")]
    public GameObject projectile;
    public Item bow;


    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.Walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;
        canMove = true;
    }

    public void SetPositionAndSnapToTile(Vector2 pos)
    {
        pos.x = Mathf.Floor(pos.x) + 0.5f;
        pos.y = Mathf.Floor(pos.y) + 0.5f + OffsetY;

        transform.position = pos;
    }

    private void Update()
    {
        if (!canMove)
        {
            myRigidbody.velocity = Vector2.zero;
            return;
            //Revisar agregar anim.speed = zero
        }
        // Is the player in an interaction
        if (currentState == PlayerState.Interact)
        {
            return;
        }
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.X) && currentState != PlayerState.Attack && currentState != PlayerState.Stagger)
        {
            StartCoroutine(AttackCo());
        }
        // TODO ABILITY
        else if (Input.GetButtonDown("Second Weapon") && currentState != PlayerState.Attack && currentState != PlayerState.Stagger)
        {
            if (playerInventory.CheckForItem(bow))
            {
                StartCoroutine(SecondAttackCo());
            }
        }
        //Agregue esto para refactor Interactuable (#26 6:30)
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(Interact());
        }
        //Hasta aqui
        else if (currentState == PlayerState.Walk || currentState == PlayerState.Idle)
        {
            UpdateAnimationAndMove();
        }
    }

    //Agregue esto para refactor Interactuable (#26 6:30)
    IEnumerator Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if (collider != null)
        {
            yield return collider.GetComponent<Interactuable>()?.Interact(transform);
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.Attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        if(currentState != PlayerState.Interact)
        {
            currentState = PlayerState.Walk;
        }
    }

    // TODO ABILITY
    private IEnumerator SecondAttackCo()
    {
        //animator.SetBool("attacking", true);
        currentState = PlayerState.Attack;
        yield return null;
        MakeArrow();
        //animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.Interact)
        {
            currentState = PlayerState.Walk;
        }
    }

    // TODO ABILITY This should be part of the ability itself
    private void MakeArrow()
    {
        if(playerInventory.currentMagic > 0) 
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            Arrow arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.Setup(temp, ChooseArrowDirection());
            playerInventory.ReduceMagic(arrow.arrowCost);
            reduceMagic.Raise();
        }
    }

    // TODO ABILITY This should also be part of the ability
    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX"))* Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    public void RaiseItem()
    {
        if(playerInventory.currentItem != null)
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
        }
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            change.x = Mathf.Round(change.x);
            change.y = Mathf.Round(change.y);
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
    }

    // TODO KNOCKBACK move the knockback to its own script
    public void Knock(float knockTime)
    {
        StartCoroutine(KnockCo(knockTime));
        /*
        // TODO HEALTH
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            // TODO HEALTH
            playerHit.Raise();

        }
        else
        {
            this.gameObject.SetActive(false);
        }
        */
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            StartCoroutine(FlashCo());
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.Idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }

    // TODO IFRAME move the player flashing to its own script
    private IEnumerator FlashCo()
    {
        int temp = 0;
        triggerCollider.enabled = false;
        while(temp < numberOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        triggerCollider.enabled = true;
    }

    public string Name
    {
        get => name;
    }

    public Sprite Sprite
    {
        get => sprite;
    }

    public Animator Animator => animator;
}

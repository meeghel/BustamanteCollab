using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class TreasureChest : MonoBehaviour, Interactuable
{
    //[Header("Contents")]
    //public Item contents;
    [SerializeField] ItemBase contents;
    [SerializeField] GameObject trigger;
    //public Inventory playerInventory;
    public bool Used { get; set; } = false;
    //public bool isOpen;
    //public BoolValue storedOpen;

    /*[Header("Signals and Dialog")]
    public Signal raiseItem;
    public GameObject dialogBox;
    public Text dialogText;*/

    [Header("Animation")]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //isOpen = storedOpen.RuntimeValue;
        /*if (isOpen)
        {
            animator.SetBool("opened", true);
        }*/
        if (Used)
        {
            animator.SetBool("opened", true);
            trigger.SetActive(false);
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        if (isInteracting)//(Input.GetButtonDown("Check") && playerInRange)
        {
            if (!isOpen)
            {
                // Open the chest
                OpenChest();
            }
            else
            {
                // Chest is already open
                ChestAlreadyOpen();
            }
        }
    }

    public void OpenChest()
    {
        // Dialog window on
        dialogBox.SetActive(true);
        // dialog text = contents text
        dialogText.text = contents.itemDescription;
        // add contents to the inventory
        playerInventory.AddItem(contents);
        playerInventory.currentItem = contents;
        // Raise the signal to the player to animate
        raiseItem.Raise();
        // raise the context clue
        context.Raise();
        // set the chest to opened
        isOpen = true;
        animator.SetBool("opened", true);
        storedOpen.RuntimeValue = isOpen;
    }

    public void ChestAlreadyOpen()
    {
            // Dialog off
            dialogBox.SetActive(false);
            // raise the signal to the player to stop animating
            raiseItem.Raise();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !Used)
        {
            context.Raise();
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !Used)
        {
            context.Raise();
            playerInRange = false;
        }
    }*/

    public IEnumerator Interact(Transform initiator)
    {
        if (!Used)
        {
            if (contents.isHeartContainer)
            {
                PlayerController player = initiator.GetComponent<PlayerController>();
                player.Player.IncreaseHearts();
                player.Player.FullHeal();
                player.UpdateHP();
                initiator.GetComponentInParent<ContextClue>().ChangeContext();
            }
            else
            {
                initiator.GetComponent<Inventario>().AddItem(contents);
                initiator.GetComponentInParent<ContextClue>().ChangeContext();
            }

            Used = true;

            //GetComponent<SpriteRenderer>().enabled = false;
            //GetComponent<BoxCollider2D>().enabled = false;
            animator.SetBool("opened", true);

            string playerName = initiator.GetComponent<PlayerCharacter>().Name;

            //raiseItem.Raise();
            AudioManager.i.PlaySfx(AudioId.ItemObtained, pauseMusic: true);

            yield return initiator.GetComponent<PlayerCharacter>().RaiseItem(contents);

            yield return DialogManagerRef.instance.ShowDialogText($"¡{playerName} encontro {contents.Name}!");

            yield return initiator.GetComponent<PlayerCharacter>().LowerItem();

            initiator.GetComponentInParent<ContextClue>().ChangeContext();
            trigger.SetActive(false);
        }
    }
}

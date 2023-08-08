using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactuable
{
    [SerializeField] Dialog dialog;

    [Header("Quests")]
    [SerializeField] QuestBase questToStart;
    [SerializeField] QuestBase questToComplete;

    [Header("Movement")]
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;

    NPCRefState state;
    float idleTimer = 0f;
    int currentPattern = 0;
    Quest activeQuest;

    FacingDirection defaultDir;

    Character character;
    ItemGiver itemGiver;
    Healer healer;
    Merchant merchant;

    private void Awake()
    {
        character = GetComponent<Character>();
        itemGiver = GetComponent<ItemGiver>();
        healer = GetComponent<Healer>();
        merchant = GetComponent<Merchant>();
        defaultDir = character.Animator.DefaultDirection;
    }

    public IEnumerator Interact(Transform initiator)
    {
        if (state == NPCRefState.Idle)
        {
            state = NPCRefState.Dialog;
            character.LookTowards(initiator.position);

            if (questToComplete != null)
            {
                var quest = new Quest(questToComplete);
                yield return quest.CompleteQuest(initiator);
                questToComplete = null;

                Debug.Log($"{quest.Base.Name} completed");
            }

            if (itemGiver != null && itemGiver.CanBeGiven())
            {
                yield return itemGiver.GiveItem(initiator.GetComponent<PlayerController>());
            }
            else if (questToStart != null)
            {
                activeQuest = new Quest(questToStart);
                yield return activeQuest.StartQuest();
                questToStart = null;

                if (activeQuest.CanBeCompleted())
                {
                    yield return activeQuest.CompleteQuest(initiator);
                    activeQuest = null;
                }
            }
            else if (activeQuest != null)
            {
                if (activeQuest.CanBeCompleted())
                {
                    yield return activeQuest.CompleteQuest(initiator);
                    activeQuest = null;
                }
                else
                {
                    yield return DialogManagerRef.instance.ShowDialog(activeQuest.Base.InProgressDialogue);
                }
            }
            else if (healer != null)
            {
                yield return healer.Heal(initiator, dialog);
            }
            else if (merchant != null)
            {
                yield return merchant.Trade();
            }
            else
            {
                yield return DialogManagerRef.instance.ShowDialog(dialog);
            }

            character.Animator.SetFacingDirection(defaultDir);

            idleTimer = 0f;
            state = NPCRefState.Idle;
        }

        Debug.Log("Interacting with NPC");
    }

    private void Update()
    {
        character.HandleUpdate();

        if (state == NPCRefState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0f;
                if(movementPattern.Count > 0)
                {
                    StartCoroutine(Walk());
                }
            }
        }
        return;
    }

    IEnumerator Walk()
    {
        state = NPCRefState.Walking;

        var oldPos = transform.position;

        yield return character.Move(movementPattern[currentPattern]);

        if (transform.position != oldPos)
            currentPattern = (currentPattern + 1) % movementPattern.Count;
            
        state = NPCRefState.Idle;
    }
}

public enum NPCRefState { Idle, Walking, Dialog }
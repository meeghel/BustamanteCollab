using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactuable
{
    [SerializeField] Dialog dialog;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;

    NPCRefState state;
    float idleTimer = 0f;
    int currentPattern = 0;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Interact(Transform initiator)
    {
        if (state == NPCRefState.Idle)
        {
            state = NPCRefState.Dialog;
            character.LookTowards(initiator.position);

            StartCoroutine(DialogManagerRef.instance.ShowDialog(dialog, () =>
            {
                idleTimer = 0f;
                state = NPCRefState.Idle;
            }));
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
                //TODO Revisar hacer else - Coroutine Idle y llamar character.Idle
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
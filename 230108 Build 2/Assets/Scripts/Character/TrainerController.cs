using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour, Interactuable
{
    // TODO Falta implementar en Game Controller
    [SerializeField] string name;
    //[SerializeField] Sprite sprite;
    [SerializeField] Dialog dialog;
    [SerializeField] Dialog dialogAfterBattle;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject fov;

    [SerializeField] AudioClip trainerAppearsClip;

    // State
    bool battleLost = false;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        SetFovRotation(character.Animator.DefaultDirection);
    }

    private void Update()
    {
        character.HandleUpdate();
    }

    public IEnumerator Interact(Transform initiator)
    {
        character.LookTowards(initiator.position);

        if (!battleLost)
        {
            AudioManager.i.PlayMusic(trainerAppearsClip);

            yield return DialogManagerRef.instance.ShowDialog(dialog);
            Debug.Log("Starting Trainer Battle");
        }
        else
        {
            yield return DialogManagerRef.instance.ShowDialog(dialogAfterBattle);
        }
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        AudioManager.i.PlayMusic(trainerAppearsClip);

        // Show Exclamation
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        // Walk towards the player
        var diff = player.transform.position - transform.position;
        var moveVec = diff - diff.normalized;
        moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));

        yield return character.Move(moveVec);
        //TODO falta animar

        // Show dialog
        yield return DialogManagerRef.instance.ShowDialog(dialog);
        Debug.Log("Starting Trainer Battle");
    }

    public void SetFovRotation(FacingDirection dir)
    {
        float angle = 0f;
        if (dir == FacingDirection.Right)
            angle = 90f;
        else if (dir == FacingDirection.Up)
            angle = 180f;
        else if (dir == FacingDirection.Left)
            angle = 270f;

        fov.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }
}

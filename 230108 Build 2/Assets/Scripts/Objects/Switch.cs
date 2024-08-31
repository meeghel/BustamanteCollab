using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, Interactuable
{
    public bool isLever;
    public bool hasDialog;
    public bool active;
    public BoolValue storedValue;
    public Sprite activeSprite;
    private SpriteRenderer mySprite;
    public Door thisDoor;
    public GameObject contextClue;

    [SerializeField] Dialog dialog;

    //240109 agregue Awake para resetear bools, revisar cuando se implemente save
    private void Awake()
    {
        storedValue.initialValue = false;
        storedValue.RuntimeValue = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        active = storedValue.RuntimeValue;
        if (active)
        {
            ActivateSwitch();
        }
    }

    public IEnumerator Interact(Transform initiator)
    {
        if (hasDialog)
            yield return DialogManagerRef.instance.ShowDialog(dialog);
        if (isLever && !active)
            ActivateSwitch();
        yield return null;
    }

    public void ActivateSwitch()
    {
        active = true;
        storedValue.RuntimeValue = active;
        thisDoor.Open();
        mySprite.sprite = activeSprite;
        contextClue.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Is it the player?
        if (other.CompareTag("Player"))
        {
            if (!isLever && !active)
                ActivateSwitch();
        }
    }
}

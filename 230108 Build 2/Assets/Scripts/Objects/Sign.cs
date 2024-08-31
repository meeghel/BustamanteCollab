using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Sign : MonoBehaviour, Interactuable
{
    [SerializeField] Dialog dialog;
    public Signal context;
    public bool playerInRange;
    public float areaDetection = 1.5f;

    public IEnumerator Interact(Transform initiator)
    {
        yield return DialogManagerRef.instance.ShowDialog(dialog);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            context.Raise();
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            context.Raise();
            playerInRange = false;
        }
    }

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, areaDetection);
    }*/
}

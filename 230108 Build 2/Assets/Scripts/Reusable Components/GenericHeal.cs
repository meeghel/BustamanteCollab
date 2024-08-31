using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GenericHeal : MonoBehaviour
{
    [SerializeField] private float heal;
    [SerializeField] private string otherTag;

    /*public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            GenericHealth temp = other.GetComponent<GenericHealth>();
            if (temp)
            {
                temp.Heal(heal);
            }
        }
    }*/

    // Se puede simplificar poniendo CompareTag("Player")
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            // poner state Busy?
            if (otherTag == "Player")
            {
                PlayerController player = other.GetComponentInParent<PlayerController>();
                player.Player.Heal(heal);
                player.UpdateHP();
                AudioManager.i.PlaySfx(AudioId.ItemObtained);
                Destroy(this.gameObject);
            }
        }
    }
}

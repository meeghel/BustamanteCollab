using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GenericHeal : MonoBehaviour
{
    [SerializeField] private float heal;
    [SerializeField] private string otherTag;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            GenericHealth temp = other.GetComponent<GenericHealth>();
            if (temp)
            {
                temp.Heal(heal);
            }
        }
    }
}

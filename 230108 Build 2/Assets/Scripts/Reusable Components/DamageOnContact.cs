using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{

    [SerializeField] private string otherString;
    [SerializeField] private float damage;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(otherString))
        {
            //First check to see if there is health:
            GenericHealth temp = other.gameObject.GetComponent<GenericHealth>();
            if (temp)
            {
                temp.Damage(damage);
            }
            Destroy(this.gameObject);
        }
    }
}

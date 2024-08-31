using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GenericDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private string otherTag;

    /*public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            GenericHealth temp = other.GetComponent<GenericHealth>();
            if (temp)
            {
                //temp.Damage(damage);
                temp.TakeDamage(damage);
            }
        }
    }*/

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            // poner state Busy?
            if(otherTag == "Player")
            {
                PlayerController player = other.GetComponentInParent<PlayerController>();
                bool isFainted = player.Player.TakeDamage(damage);
                player.UpdateHP();

                if (isFainted)
                {
                    StartCoroutine(player.PlayerFainted());
                    //player.transform.gameObject.SetActive(false);

                    //DialogBox para decir que se desamayo y si quiere cambiar de personaje?
                    //yield return dialogBox.TypeDialog($"¡{player.Base.Name} no puede seguir!");
                }
            }
            else
            {
                GenericHealth temp = other.GetComponent<GenericHealth>();
                if (temp)
                {
                    temp.Damage(damage);
                    //temp.TakeDamage(damage);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public Signal context;
    public bool playerInRange;
    public bool isInteracting = false;
    public float areaDetection = 1.5f;
    GameObject PlayerRef;
    RigidbodyConstraints2D originalConstraints;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vamos a revisar si el jugador está en el rango del objeto
        if (playerInRange)
        {
            //Revisar guardar constraints originales para regresar a ellas.
            //originalConstraints = PlayerRef.GetComponent<Rigidbody2D>().constraints;
            if (Input.GetKey(KeyCode.E))
            {
                isInteracting = true;
                GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().canMove = false;
                GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().currentState = PlayerState.idle;
                GameObject.FindWithTag("NPC").GetComponent<NPCBounded>().canMove = false;
                GameObject.FindWithTag("NPC").GetComponent<Animator>().speed = 0;
                //PlayerRef.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                isInteracting = false;
                GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().canMove = true;
                GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().currentState = PlayerState.walk;
                GameObject.FindWithTag("NPC").GetComponent<NPCBounded>().canMove = true;
                GameObject.FindWithTag("NPC").GetComponent<Animator>().speed = 1;
                //PlayerRef.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            //PlayerRef = other.gameObject;
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, areaDetection);
    }

}

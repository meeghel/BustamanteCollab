using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public Signal context;
    public bool playerInRange;
    public bool isInteracting = false;
    public float areaDetection = 1.5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Vamos a revisar si el jugador está en el rango del objeto
        if (playerInRange)
        {
            if (Input.GetKey(KeyCode.E))
            {
                isInteracting = true;
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                isInteracting = false;
            }
        }
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, areaDetection);
    }

}

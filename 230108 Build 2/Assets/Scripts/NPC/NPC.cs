using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject dropItem;
    bool canDropItem;
    public DialogNPC Dialog;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Si entro");
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject obj = Instantiate(dropItem, transform.position, Quaternion.identity);
            LevelManager.instance.items.Add(obj);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

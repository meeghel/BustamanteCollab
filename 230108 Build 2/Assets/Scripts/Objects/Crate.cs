using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public float pushForce = 10f;
    private Rigidbody2D myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direction = collision.GetContact(0).normal;
            myRigidbody.AddForce(-direction * pushForce, ForceMode2D.Impulse);
        }
    }*/

    void Update()
    {
        Vector3 lastPosition = transform.position;

        if (transform.position != lastPosition)
        {
            //AudioManager.i.PlaySfx(AudioId.PushCrate);
            Debug.Log("Position Changed!");
            lastPosition = transform.position;
        }
    }
}

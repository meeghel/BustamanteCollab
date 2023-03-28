using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{

    public float speed;
    public Rigidbody2D myRigidbody;
    public float lifetime;
    private float lifetimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        lifetimeCounter = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(Vector2 velocity, Vector3 direction)
    {
        myRigidbody.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);
    }

    void OnCollisionEnter2D(Collision2D Col)
    {
        Destroy(this.gameObject);
    }
}

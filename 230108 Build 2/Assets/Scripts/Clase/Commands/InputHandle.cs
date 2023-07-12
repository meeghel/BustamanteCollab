using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandle : MonoBehaviour
{
    Vector3 Position;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StorePosition(float x, float y, float z)
    {
        Position.x = x;
        Position.y = y;
        Position.z = z;

        Debug.Log("Position: " + Position);
    }

    public Vector3 GetPosition()
    {
        return Position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * 0.1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * 0.1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * 0.1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * 0.1f;
        }
    }
}

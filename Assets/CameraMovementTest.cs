using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var speed = Input.GetButton("Jump") ? 75.0f : 15.0f;

        transform.position += 
            (transform.forward * Input.GetAxis("Vertical") +
            transform.right * Input.GetAxis("Horizontal")) * speed * Time.deltaTime;

        var euler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(
            euler.x - Input.GetAxis("Mouse Y"),
            euler.y + Input.GetAxis("Mouse X"),
            0);

    }
}

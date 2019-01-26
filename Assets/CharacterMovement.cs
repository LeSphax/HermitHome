using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public float speed = 100f;
    public float upSpeed = 1f;

    public GameObject footLeft;
    public GameObject footRight;
    // Start is called before the first frame update
    void Start()
    {

    }

    void LimitFootRotation(GameObject foot)
    {
        if (foot.transform.eulerAngles.z > 180 && foot.transform.eulerAngles.z < 270)
        {
            foot.transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (foot.transform.eulerAngles.z > 270 && foot.transform.eulerAngles.z <= 360)
        {
            foot.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        LimitFootRotation(footLeft);
        LimitFootRotation(footRight);

        if (Input.GetKey(KeyCode.A))
        {
            footLeft.GetComponent<Rigidbody>().AddTorque(Vector3.forward * speed, ForceMode.VelocityChange);
        }
        else if (footLeft.transform.eulerAngles.z < 180 && footLeft.transform.eulerAngles.z > 1)
        {
            footLeft.transform.Rotate(Vector3.back * 1f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            footRight.GetComponent<Rigidbody>().AddTorque(Vector3.forward * speed, ForceMode.VelocityChange);
        }
        else if (footRight.transform.eulerAngles.z < 180 && footRight.transform.eulerAngles.z > 1)
        {
            footRight.transform.Rotate(Vector3.back * 1f);
        }

        //if (Input.GetKey(KeyCode.W)){
        //    GetComponent<Rigidbody>().AddForce(Vector3.forward * speed, ForceMode.Acceleration);
        //}
        //if (Input.GetKey(KeyCode.A)){
        //    GetComponent<Rigidbody>().AddForce(Vector3.left * speed, ForceMode.Acceleration);
        //}
        //if (Input.GetKey(KeyCode.S)){
        //    GetComponent<Rigidbody>().AddForce(Vector3.back * speed, ForceMode.Acceleration);
        //}
        //if (Input.GetKey(KeyCode.D)){
        //    GetComponent<Rigidbody>().AddForce(Vector3.right * speed, ForceMode.Acceleration);
        //}
    }
}

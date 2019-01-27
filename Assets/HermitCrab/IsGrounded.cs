using UnityEngine;

public class IsGrounded : MonoBehaviour
{

    public static bool isGrounded = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}

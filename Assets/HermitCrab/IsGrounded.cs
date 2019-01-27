using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    public static int groundCount = 0;
    public static bool isGrounded => groundCount > 0;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            groundCount++;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            groundCount--;
        }
    }
}

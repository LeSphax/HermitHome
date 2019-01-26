using UnityEngine;

public class GrabGround : MonoBehaviour
{
    public GameObject foot;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger on " + other.name);
        if (other.tag == "Ground")
        {
            Debug.Log("Trigger on Ground ");
            //HingeJoint tempHinge = foot.AddComponent<HingeJoint>();
            //tempHinge.anchor = foot.transform.InverseTransformPoint(transform.position);
        }
    }
}

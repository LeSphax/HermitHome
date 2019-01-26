using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class GrabGround : MonoBehaviour
{
    public EventInstance m_grabSound;

    [HideInInspector]
    public KeyCode downCode;
    public HingeJoint tempHinge = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            Debug.Log("Trigger on Ground ");
            if (Input.GetKey(downCode) && tempHinge == null)
            {
                m_grabSound.start();

                tempHinge = gameObject.AddComponent<HingeJoint>();
                tempHinge.anchor = transform.InverseTransformPoint(collision.GetContact(0).point);
                tempHinge.connectedAnchor = collision.GetContact(0).point;
                tempHinge.axis = Vector3.forward;
                tempHinge.useMotor = true;
                tempHinge.useLimits = true;
                JointLimits l = new JointLimits();
                l.min = -70;
                l.max = 70;
                tempHinge.limits = l;
                JointMotor m = new JointMotor();
                m.targetVelocity = 300;
                m.force = 1000;
                tempHinge.motor = m;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(downCode) && tempHinge !=null)
        {
            Destroy(tempHinge);
            tempHinge = null;

            
        }
    }
}

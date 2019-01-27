using UnityEngine;

public class ArmMovement : MonoBehaviour
{

    public float speed = 1000f;

    public GameObject fullArm;

    public KeyCode upCode;
    public KeyCode downCode;
    [SerializeField]
    private Rigidbody arm;
    [SerializeField]
    private Rigidbody forearm;
    private GrabGround grabGround;
    // Start is called before the first frame update
    void Start()
    {
        //arm = fullArm.transform.Find("Arm").GetComponent<Rigidbody>();
        //forearm = fullArm.transform.Find("Forearm").GetComponent<Rigidbody>();
        grabGround = forearm.GetComponent<GrabGround>();
        grabGround.downCode = downCode;
        HingeJoint armJoint = gameObject.AddComponent<HingeJoint>();
        armJoint.connectedBody = arm;
        armJoint.anchor = transform.InverseTransformPoint(arm.transform.position);
        float angle = fullArm.transform.eulerAngles.y;
        angle = angle > 180 ? angle - 360 : angle;
        float proportion = angle / 90;

        armJoint.axis = Vector3.forward * (1- Mathf.Abs(proportion)) + Vector3.right * proportion;
        armJoint.useLimits = true;
        JointLimits l = new JointLimits();
        l.min = -90;
        l.max = 0;
        armJoint.limits = l;
    }

    void Update()
    {
        if (Input.GetKey(upCode))
        {
            arm.AddRelativeTorque(transform.forward * speed, ForceMode.VelocityChange);
        }
        else if (Input.GetKey(downCode))
        {
            if (grabGround.tempHinge == null)
            {
                arm.AddRelativeTorque(transform.forward * -speed, ForceMode.VelocityChange);
            }
        }
        else
        {
            arm.velocity = Vector3.zero;
            arm.angularVelocity = Vector3.zero;
            forearm.velocity = Vector3.zero;
            forearm.angularVelocity = Vector3.zero;
        }
    }
}

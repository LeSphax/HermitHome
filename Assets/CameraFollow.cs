using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public float targetDistance = 10;
    public float targetXAngle = 30f;
    public float targetYAngle = 30f;
    public float lerpAmount = 0.05f;
    
    private void Awake()
    {
    }

    private void FixedUpdate()
    {
        //personalTransform.rotation = Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0);

        var diff = target.transform.position - transform.position;
        var currentQuat = Quaternion.LookRotation(-diff, Vector3.up);
        var currentDistance = diff.magnitude;

        var targetRotation = Quaternion.Euler(-targetXAngle, target.transform.rotation.eulerAngles.y + targetYAngle, 0);
        
        var slerpQuat = Quaternion.Slerp(currentQuat, targetRotation, lerpAmount);
        var lerpDistance = Mathf.Lerp(currentDistance, targetDistance, lerpAmount);
            
        transform.position = target.transform.position + slerpQuat * Vector3.forward * lerpDistance;
        transform.LookAt(target.transform);

        /*
        currentQuat = Quaternion.Slerp(currentQuat, 
            Quaternion.Euler(targetXAngle, target.transform.rotation.eulerAngles.y, 0), 
            0.1f);

        Vector3 targetPoint = target.transform.position + currentQuat * Vector3.forward * currentDistance;

        transform.position = Vector3.Lerp(transform.position, targetPoint, 0.05f);
        transform.LookAt(target.transform);
        */
    }
}

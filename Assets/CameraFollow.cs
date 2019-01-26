using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    private Transform personalTransform;


    private void Awake()
    {
        personalTransform = new GameObject().transform;
    }

    private void FixedUpdate()
    {
        personalTransform = target.transform;
        personalTransform.rotation = Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0);
        Vector3 targetPoint = personalTransform.TransformPoint(15, 15, 15);
        targetPoint.y = 15;
        transform.position = Vector3.Lerp(transform.position, personalTransform.TransformPoint(20, 20, 0), 0.01f);
        transform.LookAt(target.transform);
    }
}

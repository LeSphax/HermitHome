﻿using UnityEngine;

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
        //personalTransform.rotation = Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0);
        Vector3 targetPoint = personalTransform.TransformPoint(15, 15, 15) / target.transform.localScale.x;
        targetPoint.y = 15;
        transform.position = Vector3.Lerp(transform.position, targetPoint, 0.01f);
        transform.LookAt(target.transform);
    }
}

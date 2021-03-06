﻿using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class Collector: MonoBehaviour {

    [SerializeField]
    private GameObject m_placementTarget = null;


    void OnTriggerEnter(Collider other) {
        var collectible = other.GetComponentInParent<CollectibleInfo>();
        Debug.Log(collectible?.name ?? "No collectible :(");
        if (collectible == null)
            return;

        Collect(collectible);
    }
    void Collect(CollectibleInfo collectible) {
        var placementCollider = m_placementTarget.GetComponentInChildren<Collider>();
        Vector3 dir = Random.onUnitSphere;

        while (dir.y > 0.0)
        {
            dir.y = -dir.y;
        }
        
        
        var ray = new Ray(placementCollider.transform.position + -dir * 10f, dir);

        RaycastHit hitinfo;

        if (!Physics.Raycast(ray, out hitinfo, 10.0f, 1 << 9))
        {
            Debug.LogError("Don't know where to put the collectible");
            return;
        }

        var rigidBody = collectible.GetComponent<Rigidbody>();
        if (rigidBody != null) {
            Destroy(rigidBody);
        }

        var colTransf = collectible.transform;
        colTransf.parent = placementCollider.transform;
        colTransf.position = hitinfo.point;
        colTransf.rotation = Quaternion.FromToRotation(Vector3.up, hitinfo.normal) *
            Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        TopScreenText.SetText(collectible.m_name + ": " + collectible.m_infoText);

        GetComponent<StudioEventEmitter>().EventInstance.setParameterValue("PickUp", 1f);
        GetComponent<StudioEventEmitter>().EventInstance.start();

    }
}
